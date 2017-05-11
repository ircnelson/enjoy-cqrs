using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Cars.EventSource.Projections;
using Cars.Testing.Shared.StubApplication.Domain.BarAggregate;
using Cars.Testing.Shared.StubApplication.Domain.BarAggregate.Projections;
using FluentAssertions;
using Xunit;

namespace Cars.UnitTests.Projections
{
    public class ProjectionAttributeTests
    {
        [Fact]
        public async Task Should_scan_all_attributes_for_aggregate()
        {
            var bar = Bar.Create(Guid.NewGuid());

            var scanner = new ProjectionProviderAttributeScanner();

            var result = await scanner.ScanAsync(bar.GetType()).ConfigureAwait(false);

            Enumerable.Count(result.Providers).Should().Be(3);
        }

        [Fact]
        public void Should_throw_exception_when_target_is_not_aggregate()
        {
            var scanner = new ProjectionProviderAttributeScanner();

            Func<Task> func = async () => await scanner.ScanAsync(typeof(FakeNonAggregate)).ConfigureAwait(false);

            func.ShouldThrowExactly<TargetException>();
        }

        [Fact]
        public async Task Should_provide_a_projection_instance_from_aggregate()
        {
            var bar = Bar.Create(Guid.NewGuid());

            var scanner = new ProjectionProviderAttributeScanner();

            var result = await scanner.ScanAsync(bar.GetType()).ConfigureAwait(false);

            var provider = Enumerable.First(result.Providers, e => e.GetType().Name == nameof(BarProjectionProvider));

            var projection = (BarProjection) provider.CreateProjection(bar);

            AssertionExtensions.Should((Guid) projection.Id).Be(bar.Id);
            AssertionExtensions.Should((string) projection.LastText).Be(bar.LastText);
            AssertionExtensions.Should((int) projection.Messages.Count).Be(bar.Messages.Count);
            AssertionExtensions.Should((DateTime) projection.UpdatedAt).Be(bar.UpdatedAt);
        }
    }
    
    internal class FakeNonAggregate
    {

    }
}
