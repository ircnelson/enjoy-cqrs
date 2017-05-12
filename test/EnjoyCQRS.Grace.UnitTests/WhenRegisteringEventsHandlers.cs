using System.Linq;
using Cars.Events;
using Cars.Grace.UnitTests.Stubs;
using FluentAssertions;
using Grace.DependencyInjection;
using Xunit;

namespace Cars.Grace.UnitTests
{
    public class WhenRegisteringEventHandlers
    {

        [Fact]
        public void Handlers_in_assembly_are_registered()
        {
            var container = new DependencyInjectionContainer();
            container.RegisterEventHandlersInAssembly<TestEventHandler>();

            var testEventHandlers = container.LocateAll<IEventHandler<TestEvent>>();
            testEventHandlers.Count.Should().Be(1);
            AssertionExtensions.Should((object) testEventHandlers.First()).BeOfType<TestEventHandler>();

            var testEventHandlers2 = container.LocateAll<IEventHandler<TestEvent2>>();
            testEventHandlers2.Count.Should().Be(2);
            testEventHandlers2.Count(x => x.GetType() == typeof(TestEventHandler2)).Should().Be(1);
            testEventHandlers2.Count(x => x.GetType() == typeof(TestEventHandler2A)).Should().Be(1);
        }

    
    }

}