using System;
using Cars.Commands;

namespace Cars.Testing.Shared.StubApplication.Commands.BarAggregate
{
    public class CreateBarCommand : Command
    {
        public CreateBarCommand(Guid aggregateId) : base(aggregateId)
        {
        }
    }
}