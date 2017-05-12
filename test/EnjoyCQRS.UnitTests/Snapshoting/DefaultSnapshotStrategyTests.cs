using Cars.EventSource;
using Cars.EventSource.Snapshots;
using Cars.UnitTests.Domain.Stubs;
using FluentAssertions;
using Moq;
using Xunit;

namespace Cars.UnitTests.Snapshoting
{
    public class DefaultSnapshotStrategyTests
    {
        private const string CategoryName = "Unit";
        private const string CategoryValue = "DefaultSnapshotStrategy";

        [Trait(CategoryName, CategoryValue)]
        [Fact]
        public void When_aggregate_type_have_support_snapshoting()
        {
            var snapshotAggregateType = typeof(StubSnapshotAggregate);
            
            var defaultSnapshotStrategy = new DefaultSnapshotStrategy();;
            var hasSupport = defaultSnapshotStrategy.CheckSnapshotSupport(snapshotAggregateType);

            AssertionExtensions.Should((bool) hasSupport).BeTrue();
        }

        [Trait(CategoryName, CategoryValue)]
        [Fact]
        public void When_aggregate_type_doesnt_have_support_snapshoting()
        {
            var snapshotAggregateType = typeof(StubAggregate);

            var defaultSnapshotStrategy = new DefaultSnapshotStrategy();
            var hasSupport = defaultSnapshotStrategy.CheckSnapshotSupport(snapshotAggregateType);

            AssertionExtensions.Should((bool) hasSupport).BeFalse();
        }

        [Trait(CategoryName, CategoryValue)]
        [Fact]
        public void Should_make_snapshot()
        {
            var defaultSnapshotStrategy = new DefaultSnapshotStrategy();

            var snapshotAggregate = Mock.Of<ISnapshotAggregate>();
            
            var makeSnapshot = defaultSnapshotStrategy.ShouldMakeSnapshot(snapshotAggregate);

            AssertionExtensions.Should((bool) makeSnapshot).BeTrue();
        }

        [Trait(CategoryName, CategoryValue)]
        [Fact]
        public void Should_not_make_snapshot()
        {
            var defaultSnapshotStrategy = new DefaultSnapshotStrategy();

            var aggregate = Mock.Of<IAggregate>();
          
            var makeSnapshot = defaultSnapshotStrategy.ShouldMakeSnapshot(aggregate);

            AssertionExtensions.Should((bool) makeSnapshot).BeFalse();
        }
    }
}