using System;
using Cars.Events;

namespace Cars.Testing.Shared.StubApplication.EventHandlers
{
    public class ManyDependenciesEvent : IDomainEvent
    {
        public string Text { get; }

        public ManyDependenciesEvent(string text)
        {
            Text = text;
        }

        public Guid AggregateId { get; } = Guid.NewGuid();
    }
}