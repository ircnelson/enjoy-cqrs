using System;
using Cars.EventSource.Exceptions;
using Cars.Testing.Shared.MessageBus;
using Cars.UnitTests.Domain.Stubs;
using Cars.UnitTests.Domain.Stubs.Events;
using FluentAssertions;
using Xunit;

namespace Cars.UnitTests.Domain
{
    public class Not_registered_event_in_aggregate : AggregateTestFixture<StubAggregate>
    {
        public const string CategoryName = "Unit";
        public const string CategoryValue = "Aggregate";

        protected override void When()
        {
            AggregateRoot.DoSomethingWithoutEventSubscription();
        }

        [Trait(CategoryName, CategoryValue)]
        [Then]
        public void Then_throws_an_exception()
        {
            AssertionExtensions.Should((object) CaughtException).BeAssignableTo<HandleNotFound>();
        }

        [Trait(CategoryName, CategoryValue)]
        [Then]
        public void Then_the_event_type_should_be_SomeEvent()
        {
            AssertionExtensions.Should((Type) AssertionExtensions.As<HandleNotFound>(CaughtException).EventType).BeAssignableTo<NotRegisteredEvent>();
        }
    }
}