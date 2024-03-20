using System;
using Core.Events;
using Gameplay.Configs;
using VContainer.Unity;

namespace Lobby
{
    public class LobbyManager : IInitializable, IDisposable
    {
        private readonly LobbyView lobbyView;
        private readonly GameplayData gameplayData;
        private readonly GameplaySettings gameplaySettings;
        private readonly IEventPublisher<StartGameplayEvent> loadGameplayEventPublisher;

        private int objectsCount;

        public LobbyManager(LobbyView lobbyView, GameplayData gameplayData, GameplaySettings gameplaySettings,
            IEventPublisher<StartGameplayEvent> loadGameplayEventPublisher)
        {
            this.lobbyView = lobbyView;
            this.gameplayData = gameplayData;
            this.gameplaySettings = gameplaySettings;
            this.loadGameplayEventPublisher = loadGameplayEventPublisher;
        }

        public void Initialize()
        {
            lobbyView.TimerSlider.onValueChanged.AddListener(OnTimerChangedHandler);
            lobbyView.CountSlider.onValueChanged.AddListener(OnCountChangedHandler);
            lobbyView.PlayButton.onClick.AddListener(OnPlayHandler);

            var collection = gameplaySettings.DefaultGameplayData.CollectibleObjects;
            lobbyView.SetCount(collection.Count * gameplaySettings.MatchSize);
            lobbyView.SetTimer(gameplayData.Timer);
            lobbyView.CountSlider.maxValue = collection.Count;
            lobbyView.CountSlider.value = gameplayData.CollectibleObjects.Count;
        }

        public void Dispose()
        {
            lobbyView.TimerSlider.onValueChanged.RemoveListener(OnTimerChangedHandler);
            lobbyView.CountSlider.onValueChanged.RemoveListener(OnCountChangedHandler);
        }

        private void OnPlayHandler()
        {
            var defaultCollection = gameplaySettings.DefaultGameplayData.CollectibleObjects;
            gameplayData.CollectibleObjects = defaultCollection.GetRange(0, (int)objectsCount);
            loadGameplayEventPublisher.Publish(StartGameplayEvent.Empty);
        }

        private void OnCountChangedHandler(float value)
        {
            objectsCount = (int)value;
            lobbyView.SetCount(objectsCount * gameplaySettings.MatchSize);
        }

        private void OnTimerChangedHandler(float time)
        {
            gameplayData.Timer = time;
            lobbyView.SetTimer(time);
        }
    }
}