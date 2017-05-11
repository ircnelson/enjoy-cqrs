using System.Reflection;
using Cars.Attributes;
using Cars.Events;

namespace Cars.Extensions
{
    public static class DomainEventExtensions
    {
        public static bool TryGetEventNameAttribute(this IDomainEvent @event, out string eventName)
        {

            EventNameAttribute attribute;

#if REFLECTIONBRIDGE && (!(NET40 || NET35 || NET20))
            attribute =  @event.GetType().GetCustomAttribute<EventNameAttribute>();
#else
            attribute = @event.GetType().GetTypeInfo().GetCustomAttribute<EventNameAttribute>();
#endif
            eventName = attribute?.EventName;
            
            return !string.IsNullOrWhiteSpace(eventName);
        }
    }
}