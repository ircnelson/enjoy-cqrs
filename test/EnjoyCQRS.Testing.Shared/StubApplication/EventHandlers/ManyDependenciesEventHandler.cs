using System;
using System.Threading.Tasks;
using Cars.Events;
using Cars.EventSource.Storage;
using Cars.Testing.Shared.StubApplication.Domain;
using Cars.Testing.Shared.StubApplication.Domain.BarAggregate;

namespace Cars.Testing.Shared.StubApplication.EventHandlers
{
    public class ManyDependenciesEventHandler : IEventHandler<ManyDependenciesEvent>
    {
        private readonly IRepository _repository;
        private readonly IBooleanService _booleanService;
        private readonly IStringService _stringService;

        public string Output { get; private set; }

        public ManyDependenciesEventHandler(IRepository repository, IBooleanService booleanService, IStringService stringService)
        {
            _repository = repository;
            _booleanService = booleanService;
            _stringService = stringService;
        }

        public async Task ExecuteAsync(ManyDependenciesEvent @event)
        {
            if (string.IsNullOrWhiteSpace(@event.Text))
                throw new ArgumentNullException(nameof(@event.Text));

            if (_booleanService.DoSomething())
            {
                Output = _stringService.PrintWithFormat(@event.Text);
            }

            await _repository.AddAsync(Bar.Create(Guid.NewGuid())).ConfigureAwait(false);
        }
    }
}