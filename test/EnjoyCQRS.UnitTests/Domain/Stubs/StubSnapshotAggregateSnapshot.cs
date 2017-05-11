using System.Collections.Generic;
using Cars.EventSource.Snapshots;

namespace Cars.UnitTests.Domain.Stubs
{
    public class StubSnapshotAggregateSnapshot : Snapshot
    {
        public string Name { get; set; }
        public List<SimpleEntity> SimpleEntities { get; set; } = new List<SimpleEntity>();
    }
}