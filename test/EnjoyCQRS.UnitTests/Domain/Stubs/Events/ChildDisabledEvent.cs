using System;
using EnjoyCQRS.Events;

namespace EnjoyCQRS.UnitTests.Domain.Stubs.Events
{
    public class ChildDisabledEvent : DomainEvent
    {
        public ChildDisabledEvent(Guid entityId) : base(entityId)
        {
        }
    }
}