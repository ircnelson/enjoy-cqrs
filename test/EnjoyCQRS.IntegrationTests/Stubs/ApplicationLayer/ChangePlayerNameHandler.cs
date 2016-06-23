using System.Threading.Tasks;
using EnjoyCQRS.Commands;
using EnjoyCQRS.EventSource.Storage;
using EnjoyCQRS.IntegrationTests.Stubs.DomainLayer;

namespace EnjoyCQRS.IntegrationTests.Stubs.ApplicationLayer
{
    public class ChangePlayerNameHandler : ICommandHandler<ChangePlayerName>
    {
        private readonly IRepository _repository;

        public ChangePlayerNameHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task ExecuteAsync(ChangePlayerName command)
        {
            var aggregate = await _repository.GetByIdAsync<FakeGame>(command.AggregateId);
            aggregate.ChangePlayerName(command.Player, command.Name);

            await _repository.AddAsync(aggregate).ConfigureAwait(false);
        }
    }
}