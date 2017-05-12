using System;
using Cars.Events;

namespace Cars.UnitTests.MessageBus.Stubs
{
    public class OrderedTestEvent : DomainEvent
    {
        public int Order { get; }

        public OrderedTestEvent(Guid aggregateId, int order) : base(aggregateId)
        {
            Order = order;
        }
    }
}