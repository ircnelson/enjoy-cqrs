using System.Threading.Tasks;
using Cars.Commands;
using Cars.MessageBus;
using Grace.DependencyInjection;

namespace Cars.Grace
{
    public class GraceCommandDispatcher : CommandDispatcher
    {
        private readonly IExportLocatorScope _scope;

        public GraceCommandDispatcher(IExportLocatorScope scope)
        {
            _scope = scope;
        }

        protected override async Task RouteAsync<TCommand>(TCommand command)
        {
            var handler = _scope.Locate<ICommandHandler<TCommand>>();
            await handler.ExecuteAsync(command);
        }
    }
}
