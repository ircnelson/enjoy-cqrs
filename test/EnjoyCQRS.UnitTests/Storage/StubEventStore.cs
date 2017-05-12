using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cars.EventSource;
using Cars.EventSource.Snapshots;
using Cars.EventSource.Storage;

namespace Cars.UnitTests.Storage
{
    public class StubEventStore : InMemoryEventStore
    {
        public bool SaveSnapshotMethodCalled { get; private set; }
        public bool GetSnapshotMethodCalled { get; private set; }
        
        public override Task SaveAsync(IEnumerable<ISerializedEvent> collection)
        {
            SaveSnapshotMethodCalled = true;

            return base.SaveAsync(collection);
        }

        public override Task<ICommitedSnapshot> GetLatestSnapshotByIdAsync(Guid aggregateId)
        {
            GetSnapshotMethodCalled = true;

            return base.GetLatestSnapshotByIdAsync(aggregateId);
        }
    }
}