using System.Collections.Generic;
using Cars.EventSource;
using Cars.EventSource.Projections;

namespace Cars.Testing.Shared.StubApplication.Domain.BarAggregate.Projections
{
    public class BarProjectionProvider : IProjectionProvider
    {
        public object CreateProjection(IAggregate aggregate)
        {
            var target = (Bar)aggregate;

            return new BarProjection
            {
                Id = target.Id,
                LastText = target.LastText,
                UpdatedAt = target.UpdatedAt,
                Messages = new List<string>(target.Messages)
            };
        }
    }

    public class BarWithoutMessagesProjectionProvider : IProjectionProvider
    {
        public object CreateProjection(IAggregate aggregate)
        {
            var target = (Bar)aggregate;

            return new BarWithoutMessagesProjection
            {
                Id = target.Id,
                LastText = target.LastText,
                UpdatedAt = target.UpdatedAt
            };
        }
    }

    public class BarOnlyIdProjectionProvider : ProjectionProvider<Bar, BarWithIdOnlyProjection>
    {
        public override BarWithIdOnlyProjection CreateProjection(Bar aggregate)
        {
            return new BarWithIdOnlyProjection
            {
                Id = aggregate.Id
            };
        }
    }
}
