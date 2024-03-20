using System;
using Core.Events;

namespace Gameplay.Events.Aggregators
{
    public class GameplayEventsAggregator : IDisposable
    {
        public readonly IEventSubscriber<TableClearedEvent> TableClearedSubscriber;
        public readonly IEventSubscriber<OutOfTimeEvent> OutOfTimeSubscriber;
        public readonly IEventSubscriber<PoolOverflowEvent> PoolOverflowSubscriber;
        
        public readonly IEventPublisher<GameplayFinishEvent> FinishEventPublisher;
        public readonly IEventPublisher<StartGameplayEvent> StartGameplayPublisher;
        public readonly IEventPublisher<ExitGameplayEvent> ExitGameplayPublisher;

        public GameplayEventsAggregator(
            IEventSubscriber<TableClearedEvent> tableClearedSubscriber,
            IEventSubscriber<OutOfTimeEvent> outOfTimeSubscriber,
            IEventSubscriber<PoolOverflowEvent> poolOverflowSubscriber,
            IEventPublisher<GameplayFinishEvent> finishEventPublisher, 
            IEventPublisher<StartGameplayEvent> startGameplayPublisher, 
            IEventPublisher<ExitGameplayEvent> exitGameplayPublisher)
        {
            TableClearedSubscriber = tableClearedSubscriber;
            OutOfTimeSubscriber = outOfTimeSubscriber;
            PoolOverflowSubscriber = poolOverflowSubscriber;
            FinishEventPublisher = finishEventPublisher;
            StartGameplayPublisher = startGameplayPublisher;
            ExitGameplayPublisher = exitGameplayPublisher;
        }

        public void Dispose()
        {
            TableClearedSubscriber.Dispose();
            OutOfTimeSubscriber.Dispose();
            PoolOverflowSubscriber.Dispose();
        }
    }
}