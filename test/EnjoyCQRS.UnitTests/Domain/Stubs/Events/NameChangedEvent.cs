using System;
using Cars.Events;

namespace Cars.UnitTests.Domain.Stubs.Events
{
    public class NameChangedEvent : DomainEvent
    {
        public string Name { get; }

        public NameChangedEvent(Guid aggregateId, string name) : base(aggregateId)
        {
            Name = name;
        }
    }
}