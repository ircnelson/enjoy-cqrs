using System;

namespace Cars.Testing.Shared.EventStore
{
    [Flags]
    public enum EventStoreMethods
    {
        Ctor = 0,
        Dispose = 1,
        BeginTransaction = 2,
        Rollback = 4,
        CommitAsync = 8,
        SaveAsync = 16,
        SaveSnapshotAsync = 32,
        GetAllEventsAsync = 64,
        GetLatestSnapshotByIdAsync = 128,
        GetEventsForwardAsync = 256,
        SaveAggregateProjection = 512
    }
}