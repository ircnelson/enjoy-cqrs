using System;
using Cars.Events;

namespace Cars.UnitTests.Domain.Stubs.Events
{
    public class ChildDisabledEvent : DomainEvent
    {
        public Guid EntityId { get; }

        public ChildDisabledEvent(Guid aggregateId, Guid entityId) : base(aggregateId)
        {
            EntityId = entityId;
        }
    }
}