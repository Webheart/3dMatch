using UnityEngine;

namespace Gameplay.Configs
{
    [CreateAssetMenu(menuName = "Create GameSettings", fileName = "GameSettings", order = 0)]
    public class GameplaySettings : ScriptableObject
    {
        [Header("Default Gameplay Settings")] 
        public GameplayData DefaultGameplayData;
        
        [Header("Pool Settings")] 
        public int MatchSize = 3;
    }
}