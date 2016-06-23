﻿using System.Linq;
using EnjoyCQRS.EventSource;
using EnjoyCQRS.MetadataProviders;
using EnjoyCQRS.UnitTests.Domain.Stubs;
using FluentAssertions;
using Xunit;

namespace EnjoyCQRS.UnitTests.Metadata
{
    public class MetadataProviderTests
    {
        [Fact]
        public void Event_MetadataProvider()
        {
            var stubAggregate = StubAggregate.Create("Test");
            
            var metadataProvider = new EventTypeMetadataProvider();

            var metadata = stubAggregate.UncommitedEvents.SelectMany(e => metadataProvider.Provide(stubAggregate, e, EventSource.Metadata.Empty));
            
            metadata.Count().Should().Be(2);
        }
        
        [Fact]
        public void Aggregate_MetadataProvider()
        {
            var stubAggregate = StubAggregate.Create("Test");

            var metadataProvider = new AggregateTypeMetadataProvider();

            var metadata = stubAggregate.UncommitedEvents.SelectMany(e => metadataProvider.Provide(stubAggregate, e, EventSource.Metadata.Empty));

            metadata.Count().Should().Be(3);
        }

        [Fact]
        public void Should_take_event_name_based_on_attribute()
        {
            var stubAggregate = StubAggregate.Create("Test");
            var metadataProvider = new EventTypeMetadataProvider();
            var metadatas = stubAggregate.UncommitedEvents.SelectMany(e => metadataProvider.Provide(stubAggregate, e, EventSource.Metadata.Empty));

            var metadata = new EventSource.Metadata(metadatas);

            metadata.GetValue(MetadataKeys.EventName).Should().Be("StubCreated");
        }
    }
}