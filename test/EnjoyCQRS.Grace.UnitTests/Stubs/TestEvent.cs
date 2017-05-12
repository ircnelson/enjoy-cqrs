using System;
using Cars.Events;

namespace Cars.Grace.UnitTests.Stubs
{
    public class TestEvent : DomainEvent
    {
        public TestEvent(Guid aggregateId, string someProperty) : base(aggregateId)
        {
            SomeProperty = someProperty;
        }

        public string SomeProperty { get; }

        public bool WasHandled { get; set; }
    }
}