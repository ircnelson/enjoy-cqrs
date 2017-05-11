using System;
using Cars.EventSource.Snapshots;
using Cars.UnitTests.Domain.Stubs;
using FluentAssertions;
using Xunit;

namespace Cars.UnitTests.Snapshoting
{
    public class When_restore_snapshot
    {
        public const string CategoryName = "Unit";
        public const string CategoryValue = "Snapshot";

        private readonly StubSnapshotAggregateSnapshot _snapshot;
        private readonly StubSnapshotAggregate _stubAggregate;

        public When_restore_snapshot()
        {
            var aggregateId = Guid.NewGuid();
            var version = 1;

            _snapshot = new StubSnapshotAggregateSnapshot
            {
                Name = "Coringa",
            };
            
            _stubAggregate = new StubSnapshotAggregate();
            ((ISnapshotAggregate)_stubAggregate).Restore(new SnapshotRestore(aggregateId, version, _snapshot, EventSource.Metadata.Empty));
        }

        [Trait(CategoryName, CategoryValue)]
        [Then]
        public void Should_set_aggregate_properties()
        {
            _stubAggregate.Name.Should().Be(_snapshot.Name);

            AssertionExtensions.Should((int) _stubAggregate.Version).Be(1);
        }
    }
}