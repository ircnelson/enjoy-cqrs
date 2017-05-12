using System;
using Cars.Events;

namespace Cars.Grace.UnitTests.Stubs
{
    public class TestEvent2 : DomainEvent
    {
        public TestEvent2(Guid aggregateId, string someProperty) : base(aggregateId)
        {
            SomeProperty = someProperty;
        }

        public string SomeProperty { get; }

        public bool WasHandled { get; set; }
    }
}