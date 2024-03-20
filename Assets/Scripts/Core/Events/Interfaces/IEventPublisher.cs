namespace Core.Events
{
    public interface IEventPublisher<T> where T : IEvent
    {
        void Publish(T args);
    }
}