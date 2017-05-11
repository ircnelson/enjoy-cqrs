using System;
using Cars.Core;

namespace Cars.EventSource.Projections
{
    public class ProjectionSerializer : IProjectionSerializer
    {
        private readonly ITextSerializer _textSerializer;

        public ProjectionSerializer(ITextSerializer textSerializer)
        {
            _textSerializer = textSerializer;
        }

        public ISerializedProjection Serialize(Guid id, object projection)
        {
            var serialized = _textSerializer.Serialize(projection);

            return new SerializedProjection(id, projection.GetType().Name, serialized);
        }
    }
}
