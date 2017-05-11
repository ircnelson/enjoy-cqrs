using System;
using Cars.Events;

namespace Cars.Testing.Shared.StubApplication.Domain.FooAggregate
{
    public class DidSomething : DomainEvent
    {
        public DidSomething(Guid aggregateId) : base(aggregateId)
        {
        }
    }
}