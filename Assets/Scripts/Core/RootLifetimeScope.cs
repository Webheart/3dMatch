using System.Linq;
using Core.Events;
using Core.States;
using Gameplay;
using Gameplay.Configs;
using VContainer;
using VContainer.Unity;

namespace Core
{
    public class RootLifetimeScope : LifetimeScope
    {
        public GameplaySettings GameplaySettings;
        public LoadingScreen LoadingScreen;
        
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterGameplayData(builder);
            RegisterGameStates(builder);
            RegisterEvents(builder);
            RegisterUI(builder);
        }
        
        private void RegisterGameplayData(IContainerBuilder builder)
        {
            builder.RegisterInstance(GameplaySettings);

            var gameplayData = new GameplayData();
            gameplayData.Timer = GameplaySettings.DefaultGameplayData.Timer;
            gameplayData.CollectibleObjects = GameplaySettings.DefaultGameplayData.CollectibleObjects.ToList();
            builder.RegisterInstance(gameplayData);
        }
        
        private void RegisterGameStates(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GameStateManager>();
            
            builder.Register<GameplayState>(Lifetime.Singleton);
            builder.Register<LobbyState>(Lifetime.Singleton);
        }
        
        private void RegisterEvents(IContainerBuilder builder)
        {
            builder.RegisterEvent<StartGameplayEvent>(false);
            builder.RegisterEvent<ExitGameplayEvent>(false);
        }
        
        private void RegisterUI(IContainerBuilder builder)
        {
            builder.RegisterComponentInNewPrefab(LoadingScreen, Lifetime.Singleton).DontDestroyOnLoad().As<ILoadingScreen>();
        }
    }
}