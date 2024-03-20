using Core.Events;

namespace Gameplay.Events
{
    public struct ObjectSelectedEvent : IEvent
    {
        public readonly InteractableObject Object;

        public ObjectSelectedEvent(InteractableObject arg)
        {
            Object = arg;
        }
    }
}