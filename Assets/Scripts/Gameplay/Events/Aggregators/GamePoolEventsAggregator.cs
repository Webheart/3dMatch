using System;
using Core.Events;

namespace Gameplay.Events.Aggregators
{
    public class GamePoolEventsAggregator : IDisposable
    {
        public readonly IEventSubscriber<ObjectSelectedEvent> ObjectSelectedSubscriber;
        public readonly IEventPublisher<ObjectAddedToPoolEvent> ObjectAddedToPoolPublisher;
        public readonly IEventPublisher<PoolOverflowEvent> PoolOverflowPublisher;
        public readonly IEventPublisher<MatchingEvent> MatchingPublisher;

        public GamePoolEventsAggregator(
            IEventSubscriber<ObjectSelectedEvent> objectSelectedSubscriber, 
            IEventPublisher<ObjectAddedToPoolEvent> objectAddedToPoolPublisher, 
            IEventPublisher<PoolOverflowEvent> poolOverflowPublisher, 
            IEventPublisher<MatchingEvent> matchingPublisher)
        {
            ObjectSelectedSubscriber = objectSelectedSubscriber;
            ObjectAddedToPoolPublisher = objectAddedToPoolPublisher;
            PoolOverflowPublisher = poolOverflowPublisher;
            MatchingPublisher = matchingPublisher;
        }

        public void Dispose()
        {
            ObjectSelectedSubscriber?.Dispose();
        }
    }
}