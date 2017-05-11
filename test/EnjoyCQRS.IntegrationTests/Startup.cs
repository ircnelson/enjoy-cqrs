using Cars.Commands;
using Cars.Core;
using Cars.Events;
using Cars.EventSource;
using Cars.EventSource.Projections;
using Cars.EventSource.Snapshots;
using Cars.EventSource.Storage;
using Cars.Logger;
using Cars.MessageBus;
using Cars.MessageBus.InProcess;
using Cars.Testing.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Scrutor;

namespace Cars.IntegrationTests
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //TODO: Move it to another class and simplify for consumer

            services.AddSingleton<ILoggerFactory, NoopLoggerFactory>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ISession, Session>();
            services.AddScoped<ICommandDispatcher, CustomCommandDispatcher>();
            services.AddScoped<IEventPublisher, EventPublisher>();

            services.AddTransient<ISnapshotStrategy, IntervalSnapshotStrategy>();
            services.AddTransient<IRepository, Repository>();
            services.AddTransient<IEventRouter, CustomEventRouter>();
            services.AddTransient<IEventSerializer, EventSerializer>();
            services.AddTransient<ISnapshotSerializer, SnapshotSerializer>();
            services.AddTransient<ITextSerializer, JsonTextSerializer>();
            services.AddTransient<IProjectionSerializer, ProjectionSerializer>();

            services.Scan(e =>
                e.FromAssemblyOf<FooAssembler>()
                    .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
                    .AsImplementedInterfaces());

            services.Scan(e =>
                e.FromAssemblyOf<FooAssembler>()
                    .AddClasses(c => c.AssignableTo(typeof(IEventHandler<>)))
                    .AsImplementedInterfaces());

            services.AddRouting();
            services.AddMvc();
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvc();
        }
    }
}
