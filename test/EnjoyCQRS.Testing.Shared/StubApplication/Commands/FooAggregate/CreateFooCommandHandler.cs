using System.Threading.Tasks;
using Cars.Commands;
using Cars.EventSource.Storage;
using Cars.Testing.Shared.StubApplication.Domain.FooAggregate;

namespace Cars.Testing.Shared.StubApplication.Commands.FooAggregate
{
    public class CreateFooCommandHandler : ICommandHandler<CreateFooCommand>
    {
        private readonly IRepository _repository;

        public CreateFooCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task ExecuteAsync(CreateFooCommand command)
        {
            var foo = new Foo(command.AggregateId);
            await _repository.AddAsync(foo);
        }
    }
}