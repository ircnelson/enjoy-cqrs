using System.Threading.Tasks;
using Cars.Events;

namespace Cars.Grace.UnitTests.Stubs
{
    public class TestEventHandler2 : IEventHandler<TestEvent2>
    {
        public Task ExecuteAsync(TestEvent2 @event)
        {
            @event.WasHandled = true;
            return Task.CompletedTask;
        }
    }
}