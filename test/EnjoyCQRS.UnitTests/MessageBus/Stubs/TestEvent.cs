using System;
using Cars.Events;

namespace Cars.UnitTests.MessageBus.Stubs
{
    public class TestEvent : DomainEvent
    {
        public TestEvent(Guid aggregateId) : base(aggregateId)
        {
        }
    }
}