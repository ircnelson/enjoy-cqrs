﻿using System;
using System.Threading.Tasks;
using EnjoyCQRS.Commands;
using EnjoyCQRS.EventSource.Storage;
using EnjoyCQRS.UnitTests.Shared.StubApplication.Domain;
using EnjoyCQRS.UnitTests.Shared.StubApplication.Domain.BarAggregate;

namespace EnjoyCQRS.UnitTests.Shared.StubApplication.Commands.BarAggregate
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