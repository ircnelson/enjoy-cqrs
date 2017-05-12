using System.Linq;
using Cars.Testing.Shared.MessageBus;
using Cars.UnitTests.Domain.Stubs;
using Cars.UnitTests.Domain.Stubs.Events;
using FluentAssertions;
using Xunit;

namespace Cars.UnitTests.Domain
{
    public class Registered_event_in_aggregate : AggregateTestFixture<StubAggregate>
    {
        public const string CategoryName = "Unit";
        public const string CategoryValue = "Aggregate";

        protected override void When()
        {
            AggregateRoot = StubAggregate.Create("Heinsenberg");
            AggregateRoot.ChangeName("Walter White");
        }

        [Trait(CategoryName, CategoryValue)]
        [Then]
        public void Then_some_event_should_be_published()
        {
            AssertionExtensions.Should((object) Enumerable.Last(PublishedEvents)).BeAssignableTo<NameChangedEvent>();
        }

        [Trait(CategoryName, CategoryValue)]
        [Then]
        public void Then_verify_name_property()
        {
            AssertionExtensions.As<NameChangedEvent>(Enumerable.Last(PublishedEvents)).Name.Should().Be(AggregateRoot.Name);
        }
    }
}