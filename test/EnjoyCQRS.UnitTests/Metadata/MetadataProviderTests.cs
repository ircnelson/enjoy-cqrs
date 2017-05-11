using System.Collections.Generic;
using System.Linq;
using Cars.EventSource;
using Cars.MetadataProviders;
using Cars.UnitTests.Domain.Stubs;
using FluentAssertions;
using Xunit;

namespace Cars.UnitTests.Metadata
{
    public class MetadataProviderTests
    {
        public const string CategoryName = "Unit";
        public const string CategoryValue = "Metadata";

        [Trait(CategoryName, CategoryValue)]
        [Fact]
        public void Event_MetadataProvider()
        {
            var stubAggregate = StubAggregate.Create("Test");

            var metadataProvider = new EventTypeMetadataProvider();

            var metadata = Enumerable.SelectMany(stubAggregate.UncommitedEvents, e => metadataProvider.Provide(stubAggregate, e.OriginalEvent, EventSource.Metadata.Empty));

            metadata.Count<KeyValuePair<string, object>>().Should().Be(2);
        }

        [Trait(CategoryName, CategoryValue)]
        [Fact]
        public void Aggregate_MetadataProvider()
        {
            var stubAggregate = StubAggregate.Create("Test");

            var metadataProvider = new AggregateTypeMetadataProvider();

            var metadata = Enumerable.SelectMany(stubAggregate.UncommitedEvents, e => metadataProvider.Provide(stubAggregate, e.OriginalEvent, EventSource.Metadata.Empty));

            metadata.Count<KeyValuePair<string, object>>().Should().Be(3);
        }

        [Trait(CategoryName, CategoryValue)]
        [Fact]
        public void CorrelationId_MetadataProvider()
        {
            var stubAggregate = StubAggregate.Create("Test");
            stubAggregate.ChangeName("Test 1");
            stubAggregate.ChangeName("Test 2");

            var metadataProvider = new CorrelationIdMetadataProvider();

            var metadatas = Enumerable.SelectMany(stubAggregate.UncommitedEvents, e => metadataProvider.Provide(stubAggregate, e.OriginalEvent, EventSource.Metadata.Empty));

            metadatas.Select<KeyValuePair<string, object>, object>(e => e.Value).Distinct().Count().Should().Be(1);
        }

        [Trait(CategoryName, CategoryValue)]
        [Fact]
        public void Should_take_event_name_based_on_attribute()
        {
            var stubAggregate = StubAggregate.Create("Test");
            var metadataProvider = new EventTypeMetadataProvider();
            var metadatas = Enumerable.SelectMany(stubAggregate.UncommitedEvents, e => metadataProvider.Provide(stubAggregate, e.OriginalEvent, EventSource.Metadata.Empty));

            var metadata = new EventSource.Metadata(metadatas);

            metadata.GetValue(MetadataKeys.EventName).Should().Be("StubCreated");
        }
    }
}