using System.Threading.Tasks;
using Cars.Commands;
using Cars.EventSource.Storage;
using Cars.Testing.Shared.StubApplication.Domain.FooAggregate;

namespace Cars.Testing.Shared.StubApplication.Commands.FooAggregate
{
    public class DoSomethingCommandHandler : ICommandHandler<DoSomethingCommand>
    {
        private readonly IRepository _repository;

        public DoSomethingCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task ExecuteAsync(DoSomethingCommand command)
        {
            var foo = await _repository.GetByIdAsync<Foo>(command.AggregateId);
            foo.DoSomething();
        }
    }
}