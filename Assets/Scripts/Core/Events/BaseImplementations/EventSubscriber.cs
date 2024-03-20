using System;

namespace Core.Events
{
    public class EventSubscriber<T> : IEventSubscriber<T> where T : IEvent
    {
        private event Action<T> @event;
        
        private EventPublisher<T> publisher;

        public EventSubscriber(EventPublisher<T> publisher)
        {
            this.publisher = publisher;
        }

        public void Subscribe(Action<T> action)
        {
            @event += action;
        }

        public void Unsubscribe(Action<T> action)
        {
            @event -= action;
        }

        public void Dispose()
        {
            publisher.RemoveSubscriber(this);
            @event = null;
        }

        public void Notify(T args)
        {
            @event?.Invoke(args);
        }
    }
}