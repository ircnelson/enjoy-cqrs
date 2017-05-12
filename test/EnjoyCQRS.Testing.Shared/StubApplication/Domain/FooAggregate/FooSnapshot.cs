using Cars.EventSource.Snapshots;

namespace Cars.Testing.Shared.StubApplication.Domain.FooAggregate
{
    public class FooSnapshot : Snapshot
    {
        public int DidSomethingCounter { get; set; }
    }
}