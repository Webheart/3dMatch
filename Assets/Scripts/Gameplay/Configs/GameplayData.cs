using System;
using System.Collections.Generic;

namespace Gameplay.Configs
{
    [Serializable]
    public class GameplayData
    {
        public List<InteractableObject> CollectibleObjects;
        public float Timer;
    }
}