namespace Core.Events
{
    public interface IEvent
    {
    }
    
    public class EmptyEvent<T> : IEvent where T : EmptyEvent<T>, new()
    {
        public static readonly T Empty = new T();
    }
}