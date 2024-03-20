using System;
using System.Threading;
using Core.Events;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Core.States
{
    public class GameStateManager : IInitializable, IDisposable
    {
        private readonly IObjectResolver resolver;
        private readonly IEventSubscriber<StartGameplayEvent> gameplayStateEventSubscriber;
        private readonly IEventSubscriber<ExitGameplayEvent> lobbyStateEventSubscriber;
        private readonly ILoadingScreen loadingScreen;

        private IStateAsync current;

        private CancellationTokenSource disposeTokenSource;

        public GameStateManager(
            IObjectResolver resolver,
            IEventSubscriber<StartGameplayEvent> gameplayStateEventSubscriber,
            IEventSubscriber<ExitGameplayEvent> lobbyStateEventSubscriber,
            ILoadingScreen loadingScreen)
        {
            this.resolver = resolver;
            this.gameplayStateEventSubscriber = gameplayStateEventSubscriber;
            this.lobbyStateEventSubscriber = lobbyStateEventSubscriber;
            this.loadingScreen = loadingScreen;
        }

        public void Initialize()
        {
            Application.targetFrameRate = 60;
            
            gameplayStateEventSubscriber.Subscribe(LoadGameplayEventHandler);
            lobbyStateEventSubscriber.Subscribe(LoadLobbyEventHandler);
            disposeTokenSource = new CancellationTokenSource();
        }

        public void Dispose()
        {
            gameplayStateEventSubscriber.Dispose();
            lobbyStateEventSubscriber.Dispose();
            disposeTokenSource.Cancel();
            disposeTokenSource.Dispose();
        }

        private void LoadLobbyEventHandler(ExitGameplayEvent args)
        {
            ChangeState<LobbyState>(disposeTokenSource.Token).Forget();
        }

        private void LoadGameplayEventHandler(StartGameplayEvent args)
        {
            ChangeState<GameplayState>(disposeTokenSource.Token).Forget();
        }

        private async UniTask ChangeState<T>(CancellationToken token) where T : IStateAsync
        {
            var state = resolver.Resolve<T>();
            await ChangeState(state, token);
        }

        private async UniTask ChangeState(IStateAsync state, CancellationToken token)
        {
            await loadingScreen.FadeInAsync(token);
            if (current != null) await current.Exit(token);
            current = state;
            await current.Enter(token);
            await loadingScreen.FadeOutAsync(token);
        }
    }
}