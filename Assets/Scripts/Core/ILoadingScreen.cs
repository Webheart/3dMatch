using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core
{
    public interface ILoadingScreen
    {
        UniTask FadeInAsync(CancellationToken token);
        UniTask FadeOutAsync(CancellationToken token);
    }
}