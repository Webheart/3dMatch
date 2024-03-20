using System;

namespace Core.Events
{
    public interface IEventSubscriber<T> : IDisposable where T : IEvent
    {
        void Subscribe(Action<T> action);
        void Unsubscribe(Action<T> action);
    }
}