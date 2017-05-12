using System;
using System.Reflection;
using Cars.Commands;
using Cars.Events;
using Cars.EventSource;
using Cars.EventSource.Snapshots;
using Cars.EventSource.Storage;
using Cars.EventStore.MongoDB;
using Cars.Logger;
using Cars.MessageBus.InProcess;
using Grace.DependencyInjection;

namespace Cars.Grace
{
    public static class RegistrationExtensions
    {
        public static void RegisterDefaults(this IInjectionScope scope)
        {
            scope.Configure(config =>
            {
                config.Export<EventSerializer>().ByInterfaces().Lifestyle.Singleton();
                config.Export<SnapshotSerializer>().ByInterfaces().Lifestyle.Singleton();
                config.Export<NoopLoggerFactory>().ByInterfaces().Lifestyle.Singleton();
                config.Export<IntervalSnapshotStrategy>().ByInterfaces().Lifestyle.Singleton();
                config.Export<BsonTextSerializer>().ByInterfaces().Lifestyle.Singleton();
                config.Export<MongoEventStore>().ByInterfaces().Lifestyle.Singleton();

                config.Export<UnitOfWork>().ByInterfaces().Lifestyle.SingletonPerScope();
                config.Export<Session>().ByInterfaces().Lifestyle.SingletonPerScope();
                config.Export<Repository>().ByInterfaces().Lifestyle.SingletonPerScope();
                config.Export<EventPublisher>().ByInterfaces().Lifestyle.SingletonPerScope();

                config.Export<GraceCommandDispatcher>().ByInterfaces().Lifestyle.SingletonPerScope();
                config.Export<GraceEventRouter>().ByInterfaces().Lifestyle.SingletonPerScope();

            });
        }

        public static void RegisterCommandHandlersInAssembly<T>(this IInjectionScope scope)
        {
            RegisterCommandHandlersInAssembly(scope, typeof(T));
        }

        public static void RegisterCommandHandlersInAssembly(this IInjectionScope scope, Type typeInAssembly)
        {
            RegisterCommandHandlersInAssembly(scope, typeInAssembly.GetTypeInfo().Assembly);
        }

        public static void RegisterCommandHandlersInAssembly(this IInjectionScope scope, Assembly assembly)
        {
            scope.Configure(config =>
            {
                config.Export(assembly.GetExportedTypes()).BasedOn(typeof(ICommandHandler<>)).ByInterfaces().Lifestyle.SingletonPerScope();
            });
        }

        public static void RegisterEventHandlersInAssembly<T>(this IInjectionScope scope)
        {
            RegisterEventHandlersInAssembly(scope, typeof(T));
        }

        public static void RegisterEventHandlersInAssembly(this IInjectionScope scope, Type typeInAssembly)
        {
            RegisterEventHandlersInAssembly(scope, typeInAssembly.GetTypeInfo().Assembly);
        }

        public static void RegisterEventHandlersInAssembly(this IInjectionScope scope, Assembly assembly)
        {
            scope.Configure(config =>
            {
                config.Export(assembly.GetExportedTypes()).BasedOn(typeof(IEventHandler<>)).ByInterfaces().Lifestyle.SingletonPerScope();
            });
        }
    }
}
