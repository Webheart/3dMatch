using System;
using Core.Events;
using Gameplay.Configs;
using Gameplay.Controllers;
using Gameplay.Events;
using VContainer.Unity;

namespace Gameplay.Managers
{
    public class TimerManager : IInitializable, IStartable, IDisposable
    {
        private readonly TimerController timerController;
        private readonly GameplayData gameplayData;
        private readonly IEventPublisher<OutOfTimeEvent> outOfTimePublisher;
        private readonly IEventSubscriber<GameplayFinishEvent> gameplayFinishSubscriber;

        public TimerManager(TimerController timerController, GameplayData gameplayData, 
            IEventPublisher<OutOfTimeEvent> outOfTimePublisher, 
            IEventSubscriber<GameplayFinishEvent> gameplayFinishSubscriber)
        {
            this.timerController = timerController;
            this.gameplayData = gameplayData;
            this.outOfTimePublisher = outOfTimePublisher;
            this.gameplayFinishSubscriber = gameplayFinishSubscriber;
        }

        public void Initialize()
        {
            timerController.OnOutOfTime += OnTimerEndHandler;
            gameplayFinishSubscriber.Subscribe(OnGameplayFinishHandler);
        }


        public void Start()
        {
            timerController.SetTimer(gameplayData.Timer);
            timerController.StartTimer();
        }

        public void Dispose()
        {
            timerController.OnOutOfTime -= OnTimerEndHandler;
            gameplayFinishSubscriber.Dispose();
        }

        private void OnTimerEndHandler()
        {
            outOfTimePublisher.Publish(OutOfTimeEvent.Empty);
        }
        
        private void OnGameplayFinishHandler(GameplayFinishEvent arg)
        {
            timerController.StopTimer();
        }
    }
}