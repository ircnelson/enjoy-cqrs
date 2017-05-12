using System;
using Cars.Commands;

namespace Cars.Testing.Shared.StubApplication.Commands.FooAggregate
{
    public class DoSomethingCommand : Command
    {
        public DoSomethingCommand(Guid aggregateId) : base(aggregateId)
        {
        }
    }
}