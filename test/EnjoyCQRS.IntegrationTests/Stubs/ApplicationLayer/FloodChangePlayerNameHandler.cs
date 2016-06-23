using System.Threading.Tasks;
using EnjoyCQRS.Commands;
using EnjoyCQRS.EventSource.Storage;
using EnjoyCQRS.IntegrationTests.Stubs.DomainLayer;

namespace EnjoyCQRS.IntegrationTests.Stubs.ApplicationLayer
{
    public class FloodChangePlayerNameHandler : ICommandHandler<FloodChangePlayerName>
    {
        private readonly IRepository _repository;

        public FloodChangePlayerNameHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task ExecuteAsync(FloodChangePlayerName command)
        {
            var aggregate = await _repository.GetByIdAsync<FakeGame>(command.AggregateId);

            for (int i = 1; i < command.Times; i++)
            {
                aggregate.ChangePlayerName(command.Player, $"{command.Name} {i}");
            }

            await _repository.AddAsync(aggregate).ConfigureAwait(false);
        }
    }
}