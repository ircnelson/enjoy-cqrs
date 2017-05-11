using System.Threading.Tasks;
using Cars.Events;

namespace Cars.Grace.UnitTests.Stubs
{
    public class TestEventHandler : IEventHandler<TestEvent>
    {
        public Task ExecuteAsync(TestEvent @event)
        {
            @event.WasHandled = true;
            return Task.CompletedTask;
        }
    }
}