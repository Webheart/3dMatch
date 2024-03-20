using System;
using System.Collections.Generic;

namespace Core.Events
{
    public class EventPublisher<T> : IEventPublisher<T>, IDisposable where T : IEvent
    {
        private List<EventSubscriber<T>> subscribers = new List<EventSubscriber<T>>();

        public void Publish(T args)
        {
            foreach (var eventSubscriber in subscribers)
            {
                eventSubscriber.Notify(args);
            }
        }

        public IEventSubscriber<T> CreateSubscriber()
        {
            var subscriber = new EventSubscriber<T>(this);
            subscribers.Add(subscriber);
            return subscriber;
        }
        
        public void RemoveSubscriber(EventSubscriber<T> subscriber)
        {
            subscribers.Remove(subscriber);
        }

        public void Dispose()
        {
            subscribers.Clear();
        }
    }
}