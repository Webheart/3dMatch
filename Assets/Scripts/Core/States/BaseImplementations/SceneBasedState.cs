using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Core.States
{
    public abstract class SceneBasedState : IStateAsync
    {
        protected virtual string SceneName { get; } = string.Empty;

        public async UniTask Enter(CancellationToken token)
        {
            await SceneManager.LoadSceneAsync(SceneName).WithCancellation(token);
        }

        public UniTask Exit(CancellationToken token)
        {
            return UniTask.CompletedTask;
        }
    }
}