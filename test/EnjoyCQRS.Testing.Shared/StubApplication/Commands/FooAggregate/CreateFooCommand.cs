using System;
using Cars.Commands;

namespace Cars.Testing.Shared.StubApplication.Commands.FooAggregate
{
    public class CreateFooCommand : Command
    {
        public CreateFooCommand(Guid aggregateId) : base(aggregateId)
        {
        }
    }
}