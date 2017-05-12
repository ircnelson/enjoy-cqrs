using System;
using Cars.Events;

namespace Cars.UnitTests.Domain.Stubs.Events
{
    public class StubAggregateRelatedEvent : DomainEvent
    {
        public Guid StubAggregateId { get; }

        public StubAggregateRelatedEvent(Guid aggregateId, Guid stubAggregateId) : base(aggregateId)
        {
            StubAggregateId = stubAggregateId;
        }
    }
}