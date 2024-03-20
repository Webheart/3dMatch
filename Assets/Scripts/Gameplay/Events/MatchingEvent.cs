using Core.Events;

namespace Gameplay.Events
{
    public struct MatchingEvent : IEvent
    {
        public readonly InteractableObject[] MatchedObjects;

        public MatchingEvent(InteractableObject[] matchedObjects)
        {
            MatchedObjects = matchedObjects;
        }
    }
}