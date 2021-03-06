﻿using EnjoyCQRS.EventSource;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Linq;
using Xunit;
using System;
using FluentAssertions;
using System.Collections.Concurrent;
using EnjoyCQRS.Projections;
using EnjoyCQRS.Projections.InMemory;
using EnjoyCQRS.UnitTests.Shared;
using Scrutor;
using EnjoyCQRS.UnitTests.Shared.StubApplication.Domain.UserAggregate.Projections;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using EnjoyCQRS.Stores;
using EnjoyCQRS.Core;
using EnjoyCQRS.Stores.InMemory;

namespace EnjoyCQRS.IntegrationTests
{
    [Trait("Integration", "WebApi")]
    public class UserTests
    {
        private ConcurrentDictionary<string, ConcurrentDictionary<string, byte[]>> _projections = new ConcurrentDictionary<string, ConcurrentDictionary<string, byte[]>>();
        
        [Fact(Skip = "Needs refactor")]
        public async Task Should_create_user()
        {
            InMemoryStores stores = null;

            // Arrange
            Func<IServiceProvider, Tuple<ITransaction, ICompositeStores>> compositeStoresFactory = (c) => {
                stores = new InMemoryStores(c.GetService<ProjectionRebuilder>());

                return new Tuple<ITransaction, ICompositeStores>(stores, stores);
            };

            var client = TestServerFactory(compositeStoresFactory);

            var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Post, "/command/user"));

            var result = await response.Content.ReadAsStringAsync();

            var aggregateId = ExtractAggregateIdFromResponseContent(result);

            stores?.Events.Count(e => e.AggregateId == aggregateId).Should().Be(1);

            _projections.Count().Should().Be(2);

        }

        [Fact(Skip = "Needs refactor")]
        public async Task Should_update_user()
        {
            // Arrange

            InMemoryStores stores = null;

            Func<IServiceProvider, Tuple<ITransaction, ICompositeStores>> compositeStoresFactory = (c) => {
                stores = new InMemoryStores(c.GetService<ProjectionRebuilder>());

                return new Tuple<ITransaction, ICompositeStores>(stores, stores);
            };

            var client = TestServerFactory(compositeStoresFactory);

            var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Post, "/command/user"));
            var result = await response.Content.ReadAsStringAsync();
            var aggregateId = ExtractAggregateIdFromResponseContent(result);

            // Act

            var json = JsonConvert.SerializeObject(new { Name = "N" });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            response = await client.PutAsync($"/command/user/{aggregateId}", content);

            result = await response.Content.ReadAsStringAsync();

            // Assert
            stores?.Events.Count(e => e.AggregateId == aggregateId).Should().Be(2);

            _projections.Count().Should().Be(2);

            _projections.Values.SelectMany(e => e.Keys).Count(e => e.EndsWith(aggregateId.ToString())).Should().Be(2);
        }

        private HttpClient TestServerFactory(
            Func<IServiceProvider, Tuple<ITransaction, ICompositeStores>> compositeStoresFactory)
        {
            var builder = new WebHostBuilder()
                .UseStartup<Startup>()
                .ConfigureServices(services => {
                   
                    services.AddScoped<IEventsMetadataService, EventsMetadataService>();
                    //services.AddScoped<ITransaction>(provider =>compositeStoresFactory(provider).Item1);
                    //services.AddSingleton<IEventStore>(provider => compositeStoresFactory(provider).Item2);
                    //services.AddSingleton<ISnapshotStore>(provider => compositeStoresFactory(provider).Item2);
                    //services.AddSingleton<IProjectionStoreV1>(provider => compositeStoresFactory(provider).Item2);

                    services.AddScoped(c => new ProjectionRebuilder(c.GetService<IProjectionStore>(), c.GetServices<IProjector>()));

                    services.AddTransient<IProjectionStrategy, NewtonsoftJsonProjectionStrategy>();
                    services.AddTransient<IProjectionStore>(c => new MemoryProjectionStore(c.GetRequiredService<IProjectionStrategy>(), _projections));
                    services.AddTransient(typeof(IProjectionWriter<,>), typeof(MemoryProjectionReaderWriter<,>));
                    services.AddTransient(typeof(IProjectionReader<,>), typeof(MemoryProjectionReaderWriter<,>));
                    
                    services.Scan(e =>
                        e.FromAssemblyOf<FooAssembler>()
                            .AddClasses(c => c.AssignableTo<IProjector>())
                            .AsImplementedInterfaces());
                });

            var testServer = new TestServer(builder);

            return testServer.CreateClient();
        }

        private Guid ExtractAggregateIdFromResponseContent(string content)
        {
            var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);

            var aggregateId = Guid.Parse(dict["aggregateId"].ToString());

            return aggregateId;
        }
    }    
}
