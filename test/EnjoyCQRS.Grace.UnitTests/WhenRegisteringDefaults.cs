using Cars.EventSource;
using Cars.EventSource.Snapshots;
using Cars.EventSource.Storage;
using Cars.Logger;
using Cars.MessageBus;
using Cars.MessageBus.InProcess;
using FluentAssertions;
using Grace.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Cars.Grace.UnitTests
{
    public class WhenRegisteringDefaultImplementations
    {
        [Fact]
        public void Default_implementations_are_present()
        {
            var container = new DependencyInjectionContainer();
            container.RegisterDefaults();

            AssertionExtensions.Should((object) container.Locate<IEventSerializer>()).BeOfType<EventSerializer>();
            AssertionExtensions.Should((object) container.Locate<ISnapshotSerializer>()).BeOfType<SnapshotSerializer>();
            AssertionExtensions.Should((object) container.Locate<IEventSerializer>()).BeOfType<EventSerializer>();
            container.Locate<ILoggerFactory>().Should().BeOfType<NoopLoggerFactory>();
            AssertionExtensions.Should((object) container.Locate<ISnapshotStrategy>()).BeOfType<IntervalSnapshotStrategy>();

            AssertionExtensions.Should((object) container.Locate<IUnitOfWork>()).BeOfType<UnitOfWork>();
            AssertionExtensions.Should((object) container.Locate<ISession>()).BeOfType<Session>();
            AssertionExtensions.Should((object) container.Locate<IRepository>()).BeOfType<Repository>();
            AssertionExtensions.Should((object) container.Locate<IEventPublisher>()).BeOfType<EventPublisher>();

            AssertionExtensions.Should((object) container.Locate<ICommandDispatcher>()).BeOfType<GraceCommandDispatcher>();
            AssertionExtensions.Should((object) container.Locate<IEventRouter>()).BeOfType<GraceEventRouter>();

        }
    }
}
