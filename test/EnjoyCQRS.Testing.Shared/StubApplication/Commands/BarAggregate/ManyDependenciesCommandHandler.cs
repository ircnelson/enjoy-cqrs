using System;
using System.Threading.Tasks;
using Cars.Commands;
using Cars.EventSource.Storage;
using Cars.Testing.Shared.StubApplication.Domain;
using Cars.Testing.Shared.StubApplication.Domain.BarAggregate;

namespace Cars.Testing.Shared.StubApplication.Commands.BarAggregate
{
    public class ManyDependenciesCommandHandler : ICommandHandler<ManyDependenciesCommand>
    {
        private readonly IRepository _repository;
        private readonly IBooleanService _booleanService;
        private readonly IStringService _stringService;

        public string Output { get; private set; }

        public ManyDependenciesCommandHandler(IRepository repository, IBooleanService booleanService, IStringService stringService)
        {
            _repository = repository;
            _booleanService = booleanService;
            _stringService = stringService;
        }

        public async Task ExecuteAsync(ManyDependenciesCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Text))
                throw new ArgumentNullException(nameof(command.Text));

            if (_booleanService.DoSomething())
            {
                Output = _stringService.PrintWithFormat(command.Text);
            }

            await _repository.AddAsync(Bar.Create(Guid.NewGuid())).ConfigureAwait(false);
        }
    }
}