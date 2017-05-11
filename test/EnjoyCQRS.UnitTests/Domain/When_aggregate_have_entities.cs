using System;
using System.Collections.Generic;
using System.Linq;
using Cars.Events;
using Cars.Testing.Shared.MessageBus;
using Cars.UnitTests.Domain.Stubs;
using Cars.UnitTests.Domain.Stubs.Events;
using FluentAssertions;
using Xunit;

namespace Cars.UnitTests.Domain
{
    public class When_aggregate_have_entities : AggregateTestFixture<StubSnapshotAggregate>
    {
        private const string CategoryName = "Unit";
        private const string CategoryValue = "Snapshot";

        private string newChildName = "New child";

        protected override IEnumerable<IDomainEvent> Given()
        {
            var aggregateId = Guid.NewGuid();
            
            yield return new StubAggregateCreatedEvent(aggregateId, "Mother");
            yield return new ChildCreatedEvent(aggregateId, Guid.NewGuid(), "Child 1");
            yield return new ChildCreatedEvent(aggregateId, Guid.NewGuid(), "Child 2");
        }

        protected override void When()
        {
            AggregateRoot.AddEntity(newChildName);
        }

        [Trait(CategoryName, CategoryValue)]
        [Then]
        public void Aggregate_should_have_3_items()
        {
            AggregateRoot.Entities.Should().HaveCount(3);
        }

        [Trait(CategoryName, CategoryValue)]
        [Then]
        public void Should_be_published_an_event_that_entity_was_created()
        {
            AssertionExtensions.Should((object) Enumerable.Last(PublishedEvents)).BeOfType<ChildCreatedEvent>();
        }

        [Trait(CategoryName, CategoryValue)]
        [Then]
        public void Should_verify_last_event_properties()
        {
            var childCreatedEvent = AssertionExtensions.As<ChildCreatedEvent>(Enumerable.Last(PublishedEvents));

            childCreatedEvent.Name.Should().Be(newChildName);
        }
    }
}