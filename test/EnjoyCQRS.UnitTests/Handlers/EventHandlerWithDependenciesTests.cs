using System;
using Cars.Testing.Shared.MessageBus;
using Cars.Testing.Shared.StubApplication.Domain;
using Cars.Testing.Shared.StubApplication.EventHandlers;
using FluentAssertions;
using Moq;
using Xunit;

namespace Cars.UnitTests.Handlers
{
    public class PrintedSomethingTests : EventTestFixture<ManyDependenciesEvent, ManyDependenciesEventHandler>
    {
        protected override void SetupDependencies()
        {
            OnDependency<IBooleanService>().Setup(e => e.DoSomething()).Returns(true);
            OnDependency<IStringService>().Setup(e => e.PrintWithFormat(It.IsAny<string>())).Returns<string>(str => $"** {str} **");
        }

        protected override ManyDependenciesEvent When()
        {
            return new ManyDependenciesEvent("Hello World");
        }

        [Fact]
        public void Should_output_formatted_text()
        {
            AssertionExtensions.Should((string) EventHandler.Output).Be("** Hello World **");
        }
    }

    public class CaughtExceptionEventHandlerTests : EventTestFixture<ManyDependenciesEvent, ManyDependenciesEventHandler>
    {
        protected override void SetupDependencies()
        {
            OnDependency<IBooleanService>().Setup(e => e.DoSomething()).Returns(true);
            OnDependency<IStringService>().Setup(e => e.PrintWithFormat(It.IsAny<string>()));
        }

        protected override ManyDependenciesEvent When()
        {
            return new ManyDependenciesEvent(string.Empty);
        }

        [Fact]
        public void Should_throw_ArgumentNullException()
        {
            AssertionExtensions.Should((object) CaughtException).BeOfType<ArgumentNullException>();
        }
    }
}