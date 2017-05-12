using System;
using System.Threading.Tasks;
using Cars.Commands;
using Cars.MessageBus;

namespace Cars.IntegrationTests
{
    public class CustomCommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public CustomCommandDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            var handlerType = typeof(ICommandHandler<>).MakeGenericType(((Object) command).GetType());

            var handler = (dynamic) _serviceProvider.GetService(handlerType);

            await handler.ExecuteAsync(command);
        }
    }
}