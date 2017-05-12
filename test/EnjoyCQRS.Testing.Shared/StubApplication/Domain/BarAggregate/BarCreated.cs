using System;
using Cars.Events;

namespace Cars.Testing.Shared.StubApplication.Domain.BarAggregate
{
    public class BarCreated : DomainEvent
    {
        public BarCreated(Guid aggregateId) : base(aggregateId)
        {
        }
    }
}