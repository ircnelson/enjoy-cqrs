using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cars.Core;
using Cars.EventSource.Projections;
using Cars.EventStore.MongoDB;
using Cars.Testing.Shared.EventStore;
using Cars.Testing.Shared.StubApplication.Domain.BarAggregate.Projections;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Xunit;

namespace Cars.MongoDB.IntegrationTests.EventStore
{
    public class MongoEventStoreTests : IDisposable
    {
        private static readonly ITextSerializer _bsonSerializer = new BsonTextSerializer();

        public const string CategoryName = "Integration";
        public const string CategoryValue = "MongoDB";
        public const string DatabaseName = "enjoycqrs";

        private readonly MongoClient _mongoClient;

        static MongoEventStoreTests()
        {
            var pack = new ConventionPack
            {
                new CamelCaseElementNameConvention(),
                new IgnoreExtraElementsConvention(true)
            };

            ConventionRegistry.Register("camelCase", pack, t => true);

            BsonSerializer.RegisterSerializer(typeof(DateTime), DateTimeSerializer.LocalInstance);

            BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;
        }

        public MongoEventStoreTests()
        {
            var mongoHost = Environment.GetEnvironmentVariable("MONGODB_HOST");

            if (string.IsNullOrWhiteSpace(mongoHost)) throw new ArgumentNullException("The variable 'MONGODB_HOST' was not configured.");

            _mongoClient = new MongoClient($"mongodb://{mongoHost}");

            _mongoClient.DropDatabase(DatabaseName);
        }

        [Trait(CategoryName, CategoryValue)]
        [Theory, MemberData(nameof(InvalidStates))]
        public void Should_validate_constructor_parameters(MongoClient mongoClient, string database, MongoEventStoreSetttings setttings)
        {
            Action action = () => new MongoEventStore(mongoClient, database, setttings);

            action.ShouldThrowExactly<ArgumentNullException>();
        }
        

        [Trait(CategoryName, CategoryValue)]
        [Fact]
        public void Should_use_default_settings()
        {
            var defaultSettings = new MongoEventStoreSetttings();

            using (var eventStore = new MongoEventStore(_mongoClient, DatabaseName))
            {
                AssertionExtensions.Should((string) eventStore.Settings.EventsCollectionName).Be(defaultSettings.EventsCollectionName);
                AssertionExtensions.Should((string) eventStore.Settings.SnapshotsCollectionName).Be(defaultSettings.SnapshotsCollectionName);
            }
        }

        [Trait(CategoryName, CategoryValue)]
        [Theory]
        [InlineData("events", null)]
        [InlineData(null, "snapshots")]
        public void Should_validate_settings(string eventCollectionName, string snapshotCollectionName)
        {
            var defaultSettings = new MongoEventStoreSetttings
            {
                EventsCollectionName = eventCollectionName,
                SnapshotsCollectionName = snapshotCollectionName
            };
            
            Action action = () => new MongoEventStore(new MongoClient(), DatabaseName, defaultSettings);

            action.ShouldThrowExactly<ArgumentNullException>();
        }

        [Trait(CategoryName, CategoryValue)]
        [Fact]
        public void Should_use_custom_settings()
        {
            var customSettings = new MongoEventStoreSetttings
            {
                EventsCollectionName = "MyEvents",
                SnapshotsCollectionName = "MySnapshots"
            };

            using (var eventStore = new MongoEventStore(_mongoClient, DatabaseName, customSettings))
            {
                AssertionExtensions.Should((string) eventStore.Settings.EventsCollectionName).Be(customSettings.EventsCollectionName);
                AssertionExtensions.Should((string) eventStore.Settings.SnapshotsCollectionName).Be(customSettings.SnapshotsCollectionName);
            }
        }

        [Trait(CategoryName, CategoryValue)]
        [Fact]
        public void Should_create_database()
        {
            using (var eventStore = new MongoEventStore(_mongoClient, DatabaseName))
            {
                var database = eventStore.Client.GetDatabase(DatabaseName);

                AssertionExtensions.Should((object) database).NotBeNull();

                AssertionExtensions.Should((string) eventStore.Database).Be(DatabaseName);
            }
        }

        [Trait(CategoryName, CategoryValue)]
        [Fact]
        public async Task Test_events()
        {
            using (var eventStore = new MongoEventStore(_mongoClient, DatabaseName))
            {
                var eventStoreTestSuit = new EventStoreTestSuit(eventStore, new ProjectionSerializer(_bsonSerializer));

                var aggregate = await eventStoreTestSuit.EventTestsAsync();

                using (var projectionRepository = new MongoProjectionRepository(_mongoClient, DatabaseName))
                {
                    var projection = await projectionRepository.GetAsync<BarProjection>(nameof(BarProjection), aggregate.Id);

                    AssertionExtensions.Should((Guid) projection.Id).Be(aggregate.Id);
                    AssertionExtensions.Should((string) projection.LastText).Be(aggregate.LastText);
                    AssertionExtensions.Should((string) projection.UpdatedAt.ToString("G")).Be(aggregate.UpdatedAt.ToString("G"));
                    AssertionExtensions.Should((int) projection.Messages.Count).Be(aggregate.Messages.Count);
                }

                using (var projectionRepository = new MongoProjectionRepository<BarProjection>(_mongoClient, DatabaseName))
                {
                    var projections = await projectionRepository.FindAsync(e => e.Id == aggregate.Id);

                    Enumerable.Count(projections).Should().BeGreaterOrEqualTo(1);
                }
            }
        }
        
        [Trait(CategoryName, CategoryValue)]
        [Fact]
        public async Task Test_snapshot()
        {
            using (var eventStore = new MongoEventStore(_mongoClient, DatabaseName))
            {
                var eventStoreTestSuit = new EventStoreTestSuit(eventStore, new ProjectionSerializer(_bsonSerializer));

                await eventStoreTestSuit.SnapshotTestsAsync();
            }
        }

        [Trait(CategoryName, CategoryValue)]
        [Fact]
        public async Task When_any_exception_be_thrown()
        {
            using (var eventStore = new MongoEventStore(_mongoClient, DatabaseName))
            {
                var eventStoreTestSuit = new EventStoreTestSuit(eventStore, new ProjectionSerializer(_bsonSerializer));

                await eventStoreTestSuit.DoSomeProblemAsync();
            }
        }

        public static IEnumerable<object[]> InvalidStates => new[]
        {
            new object[] { null, "dbname", new MongoEventStoreSetttings() },
            new object[] { new MongoClient(), null, new MongoEventStoreSetttings() },
            new object[] { new MongoClient(), "dbname", null }
        };

        public void Dispose()
        {
            _mongoClient.DropDatabase(DatabaseName);
        }
    }
}