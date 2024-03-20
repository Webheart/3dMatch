using Core.Events;
using VContainer;

namespace Core
{
    public static class VContainerExtensions
    {
        public static void RegisterEvent<T>(this IContainerBuilder builder, bool scoped = true) where T : IEvent
        {
            builder.Register<EventPublisher<T>>(scoped ? Lifetime.Scoped : Lifetime.Singleton).As<IEventPublisher<T>>();

            builder.Register<IEventSubscriber<T>>(container =>
            {
                var publisher = (EventPublisher<T>)container.Resolve<IEventPublisher<T>>();
                return publisher.CreateSubscriber();
            }, Lifetime.Transient);
        }
    }
}