using System;
using Core.Events;
using Gameplay.Events;
using Gameplay.Events.Aggregators;
using Gameplay.Views;
using VContainer.Unity;

namespace Gameplay.Managers
{
    public class GameplayManager : IInitializable, IDisposable
    {
        private readonly GameplayEventsAggregator eventsAggregator;
        private readonly GameResultWindowView gameResultWindowView;

        public GameplayManager(GameplayEventsAggregator eventsAggregator, GameResultWindowView gameResultWindowView)
        {
            this.eventsAggregator = eventsAggregator;
            this.gameResultWindowView = gameResultWindowView;
        }

        public void Initialize()
        {
            eventsAggregator.OutOfTimeSubscriber.Subscribe(OnOutOfTimeHandler);
            eventsAggregator.PoolOverflowSubscriber.Subscribe(OnPoolOverflowHandler);
            eventsAggregator.TableClearedSubscriber.Subscribe(OnTableClearedHandler);

            gameResultWindowView.OnRestartButtonClicked += OnRestartButtonHandler;
            gameResultWindowView.OnExitButtonClicked += OnExitButtonHandler;
            gameResultWindowView.gameObject.SetActive(false);
        }


        public void Dispose()
        {
            eventsAggregator.Dispose();
            gameResultWindowView.OnRestartButtonClicked -= OnRestartButtonHandler;
            gameResultWindowView.OnExitButtonClicked -= OnExitButtonHandler;
        }

        private void OnTableClearedHandler(TableClearedEvent args)
        {
            FinishGame(FinishReason.TableCleared);
        }

        private void OnPoolOverflowHandler(PoolOverflowEvent args)
        {
            FinishGame(FinishReason.PoolOverflow);
        }

        private void OnOutOfTimeHandler(OutOfTimeEvent args)
        {
            FinishGame(FinishReason.OutOfTime);
        }

        private void FinishGame(FinishReason reason)
        {
            eventsAggregator.FinishEventPublisher.Publish(new GameplayFinishEvent(reason));
            gameResultWindowView.ShowResult(reason);
            gameResultWindowView.gameObject.SetActive(true);
        }

        private void OnRestartButtonHandler()
        {
            eventsAggregator.StartGameplayPublisher.Publish(StartGameplayEvent.Empty);
        }

        private void OnExitButtonHandler()
        {
            eventsAggregator.ExitGameplayPublisher.Publish(ExitGameplayEvent.Empty);
        }
    }
}