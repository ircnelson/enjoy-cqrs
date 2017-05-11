using System;
using Cars.Testing.Shared.MessageBus;
using Cars.Testing.Shared.StubApplication.Commands.BarAggregate;
using Cars.Testing.Shared.StubApplication.Domain;
using Cars.Testing.Shared.StubApplication.Domain.BarAggregate;
using FluentAssertions;
using Moq;
using Xunit;

namespace Cars.UnitTests.Handlers
{
    public class PrintSomethingTests : CommandTestFixture<ManyDependenciesCommand, ManyDependenciesCommandHandler, Bar>
    {
        protected override void SetupDependencies()
        {
            OnDependency<IBooleanService>().Setup(e => e.DoSomething()).Returns(true);
            OnDependency<IStringService>()
                .Setup(e => e.PrintWithFormat(It.IsAny<string>()))
                .Returns<string>(str => $"** {str} **");
        }

        protected override ManyDependenciesCommand When()
        {
            return new ManyDependenciesCommand("Hello World");
        }

        [Fact]
        public void Should_output_formatted_text()
        {
            AssertionExtensions.Should((string) CommandHandler.Output).Be("** Hello World **");
        }
    }

    public class CaughtExceptionCommandHandlerTests : CommandTestFixture<ManyDependenciesCommand, ManyDependenciesCommandHandler, Bar>
    {
        protected override void SetupDependencies()
        {
            OnDependency<IBooleanService>().Setup(e => e.DoSomething()).Returns(true);
            OnDependency<IStringService>().Setup(e => e.PrintWithFormat(It.IsAny<string>()));
        }

        protected override ManyDependenciesCommand When()
        {
            return new ManyDependenciesCommand(string.Empty);
        }

        [Fact]
        public void Should_throw_ArgumentNullException()
        {
            AssertionExtensions.Should((object) CaughtException).BeOfType<ArgumentNullException>();
        }
    }
}