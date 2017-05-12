using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cars.Events;
using Cars.EventSource;
using Cars.EventSource.Projections;
using Cars.EventSource.Storage;
using Cars.Logger;
using Cars.MessageBus;
using Cars.Testing.Shared;
using Cars.UnitTests.Domain.Stubs;
using FluentAssertions;
using Moq;
using Xunit;

namespace Cars.UnitTests.Storage
{
    public class EventStoreTests
    {
        private const string CategoryName = "Unit";
        private const string CategoryValue = "Event store";

        private readonly InMemoryEventStore _inMemoryDomainEventStore;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository _repository;
        private readonly Mock<IEventPublisher> _mockEventPublisher;

        public EventStoreTests()
        {
            var eventSerializer = new EventSerializer(new JsonTextSerializer());
            var snapshotSerializer = new SnapshotSerializer(new JsonTextSerializer());
            var projectionSerializer = new ProjectionSerializer(new JsonTextSerializer());

            _inMemoryDomainEventStore = new InMemoryEventStore();
            

            _mockEventPublisher = new Mock<IEventPublisher>();
            _mockEventPublisher.Setup(e => e.PublishAsync(It.IsAny<IEnumerable<IDomainEvent>>())).Returns(Task.CompletedTask);
            
            var loggerFactory = new NoopLoggerFactory();
            var session = new Session(loggerFactory, _inMemoryDomainEventStore, _mockEventPublisher.Object, eventSerializer, snapshotSerializer, projectionSerializer);
            _repository = new Repository(loggerFactory, session);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(e => e.CommitAsync())
                .Callback(async () =>
                {
                    await session.SaveChangesAsync().ConfigureAwait(false);
                })
                .Returns(Task.CompletedTask);

            _unitOfWork = unitOfWorkMock.Object;
        }

        [Trait(CategoryName, CategoryValue)]
        [Then]
        public async Task When_calling_Save_it_will_add_the_domain_events_to_the_domain_event_storage()
        {
            var testAggregate = StubAggregate.Create("Walter White");
            testAggregate.ChangeName("Heinsenberg");

            await _repository.AddAsync(testAggregate).ConfigureAwait(false);

            await _unitOfWork.CommitAsync().ConfigureAwait(false);

            var events = await _inMemoryDomainEventStore.GetAllEventsAsync(testAggregate.Id);
            
            Enumerable.Count(events).Should().Be(2);
        }

        [Trait(CategoryName, CategoryValue)]
        [Then]
        public async Task When_Save_Then_the_uncommited_events_should_be_published()
        {
            var testAggregate = StubAggregate.Create("Walter White");
            testAggregate.ChangeName("Heinsenberg");

            await _repository.AddAsync(testAggregate).ConfigureAwait(false);
            await _unitOfWork.CommitAsync().ConfigureAwait(false);

            _mockEventPublisher.Verify(e => e.PublishAsync(It.IsAny<IEnumerable<IDomainEvent>>()));
        }

        [Trait(CategoryName, CategoryValue)]
        [Then]
        public async Task When_load_aggregate_should_be_correct_version()
        {
            var testAggregate = StubAggregate.Create("Walter White");
            testAggregate.ChangeName("Heinsenberg");

            await _repository.AddAsync(testAggregate).ConfigureAwait(false);
            await _unitOfWork.CommitAsync().ConfigureAwait(false);

            var testAggregate2 = await _repository.GetByIdAsync<StubAggregate>(testAggregate.Id).ConfigureAwait(false);
            
            AssertionExtensions.Should((int) testAggregate.Version).Be(testAggregate2.Version);
        }
    }
}