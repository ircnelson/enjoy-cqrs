using System.Linq;
using Cars.Commands;
using Cars.Grace.UnitTests.Stubs;
using FluentAssertions;
using Grace.DependencyInjection;
using Xunit;

namespace Cars.Grace.UnitTests
{
    public class WhenRegisteringCommandHandlers
    {

        [Fact]
        public void Handlers_in_assembly_are_registered()
        {
            var container = new DependencyInjectionContainer();
            container.RegisterCommandHandlersInAssembly<TestCommandHandler>();

            var testCommandHandlers = container.LocateAll<ICommandHandler<TestCommand>>();
            testCommandHandlers.Count.Should().Be(1);
            AssertionExtensions.Should((object) testCommandHandlers.First()).BeOfType<TestCommandHandler>();

            var testCommandHandlers2 = container.LocateAll<ICommandHandler<TestCommand2>>();
            testCommandHandlers2.Count.Should().Be(1);
            AssertionExtensions.Should((object) testCommandHandlers2.First()).BeOfType<TestCommandHandler2>();
        }

    
    }

}