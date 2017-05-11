using System;
using Cars.Commands;

namespace Cars.Grace.UnitTests.Stubs
{
    public class TestCommand : Command
    {
        public TestCommand(Guid aggregateId, string someProperty) : base(aggregateId)
        {
            SomeProperty = someProperty;
        }

        public string SomeProperty { get; }

        public bool WasHandled { get; set; }
    }
}