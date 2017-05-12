using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cars.Events;

namespace Cars.UnitTests.MessageBus.Stubs
{
    public class SecondTestEventHandler : IEventHandler<TestEvent>
    {
        public List<Guid> Ids { get; } = new List<Guid>();

        public Task ExecuteAsync(TestEvent @event)
        {
            Ids.Add(@event.AggregateId);

            return Task.CompletedTask;
        }
    }
}