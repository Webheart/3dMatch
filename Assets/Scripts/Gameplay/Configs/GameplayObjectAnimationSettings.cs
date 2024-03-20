using UnityEngine;

namespace Gameplay.Configs
{
    [CreateAssetMenu(menuName = "Create GameplayObjectAnimationSettings", fileName = "GameplayObjectAnimationSettings", order = 0)]
    public class GameplayObjectAnimationSettings : ScriptableObject
    {
        [Header("Selection Animation Settings")]
        [Min(0)] public float ScaleDuration = 0.3f;
        [Min(0)] public float ScaleModifier = 1.2f;
        public AnimationCurve ScaleCurve;

        [Header("Move Settings")]
        [Min(0)] public float MovingSpring = 10f;
        [Min(0)] public float MovingDamping = 30f;

        [Header("Merge Animation Settings")]
        [Min(0)] public float MergingTime = 0.3f;
        public AnimationCurve MergingScaleCurve;
        public AnimationCurve MergingMoveCurve;
    }
}