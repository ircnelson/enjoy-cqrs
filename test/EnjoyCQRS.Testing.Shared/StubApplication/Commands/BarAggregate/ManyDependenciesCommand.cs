using System;
using Cars.Commands;

namespace Cars.Testing.Shared.StubApplication.Commands.BarAggregate
{
    public class ManyDependenciesCommand : ICommand
    {
        public string Text { get; }

        public ManyDependenciesCommand(string text)
        {
            Text = text;
        }

        public Guid AggregateId { get; } = Guid.NewGuid();
    }
}