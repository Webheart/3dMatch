using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.States
{
    public interface IStateAsync
    {
        UniTask Enter(CancellationToken token);
        UniTask Exit(CancellationToken token);
    }
}