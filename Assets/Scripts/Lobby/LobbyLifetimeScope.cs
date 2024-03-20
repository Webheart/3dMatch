using VContainer;
using VContainer.Unity;

namespace Lobby
{
    public class LobbyLifetimeScope : LifetimeScope
    {
        public LobbyView LobbyView;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<LobbyManager>().WithParameter(LobbyView);
        }
    }
}