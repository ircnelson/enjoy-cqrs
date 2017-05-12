using System.Threading.Tasks;
using Cars.Events;
using Cars.MessageBus;
using Grace.DependencyInjection;

namespace Cars.Grace
{
    public class GraceEventRouter : IEventRouter
    {
        private readonly IExportLocatorScope _scope;

        public GraceEventRouter(IExportLocatorScope scope)
        {
            _scope = scope;
        }

        public async Task RouteAsync<TEvent>(TEvent @event) where TEvent : IDomainEvent
        {
            var handlers = _scope.LocateAll<IEventHandler<TEvent>>();

            foreach (var handler in handlers)
            {
                await handler.ExecuteAsync(@event).ConfigureAwait(false);
            }
        }
    }
}
