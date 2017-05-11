using System;
using Cars.Commands;

namespace Cars.Grace.UnitTests.Stubs
{
    public class TestCommand2 : Command
    {
        public TestCommand2(Guid aggregateId, string someProperty) : base(aggregateId)
        {
            SomeProperty = someProperty;
        }

        public string SomeProperty { get; }

        public bool WasHandled { get; set; }
    }
}