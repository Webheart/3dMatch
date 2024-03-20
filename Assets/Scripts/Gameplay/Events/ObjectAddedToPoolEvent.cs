using Core.Events;
using UnityEngine;

namespace Gameplay.Events
{
    public struct ObjectAddedToPoolEvent : IEvent
    {
        public readonly GameObject Object;

        public ObjectAddedToPoolEvent(GameObject arg)
        {
            Object = arg;
        }
    }
}