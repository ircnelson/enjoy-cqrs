using System;
using Cars.Attributes;
using Cars.Events;

namespace Cars.UnitTests.Domain.Stubs.Events
{
    [EventName("StubCreated")]
    public class StubAggregateCreatedEvent : DomainEvent
    {
        public string Name { get; }

        public StubAggregateCreatedEvent(Guid aggregateId, string name) : base(aggregateId)
        {
            Name = name;
        }
    }
}