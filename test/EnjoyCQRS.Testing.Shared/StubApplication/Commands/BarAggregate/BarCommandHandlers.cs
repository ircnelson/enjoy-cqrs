using System.Threading.Tasks;
using Cars.Commands;
using Cars.EventSource.Storage;
using Cars.Testing.Shared.StubApplication.Domain.BarAggregate;

namespace Cars.Testing.Shared.StubApplication.Commands.BarAggregate
{
    public class BarCommandHandlers : ICommandHandler<CreateBarCommand>, ICommandHandler<SpeakCommand>
    {
        private readonly IRepository _repository;

        public BarCommandHandlers(IRepository repository)
        {
            _repository = repository;
        }

        public async Task ExecuteAsync(CreateBarCommand command)
        {
            var bar = Bar.Create(command.AggregateId);

            await _repository.AddAsync(bar).ConfigureAwait(false);
        }

        public async Task ExecuteAsync(SpeakCommand command)
        {
            var bar = await _repository.GetByIdAsync<Bar>(command.AggregateId).ConfigureAwait(false);

            bar.Speak(command.Text);
        }
    }
}