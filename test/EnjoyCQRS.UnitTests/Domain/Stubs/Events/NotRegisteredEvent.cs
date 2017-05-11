using System;
using Cars.Events;

namespace Cars.UnitTests.Domain.Stubs.Events
{
    public class NotRegisteredEvent : DomainEvent
    {
        public NotRegisteredEvent(Guid aggregateId) : base(aggregateId)
        {
        }
    }
}