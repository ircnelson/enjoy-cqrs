using System;
using System.Threading.Tasks;
using Cars.Events;
using Cars.MessageBus;
using Microsoft.Extensions.DependencyInjection;

namespace Cars.IntegrationTests
{
    public class CustomEventRouter : IEventRouter
    {
        private readonly IServiceProvider _serviceProvider;

        public CustomEventRouter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task RouteAsync<TEvent>(TEvent @event) where TEvent : IDomainEvent
        {
            var handlerType = typeof(IEventHandler<>).MakeGenericType(((Object) @event).GetType());

            var handlers = _serviceProvider.GetServices(handlerType);

            foreach (var handler in handlers)
            {
                await ((dynamic)handler).ExecuteAsync(@event);
            }
        }
    }
}