using Core.Events;

namespace Gameplay.Events
{
    public struct GameplayFinishEvent : IEvent
    {
        public FinishReason FinishReason;

        public GameplayFinishEvent(FinishReason finishReason)
        {
            FinishReason = finishReason;
        }
    }
}