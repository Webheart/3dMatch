using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.Configs;
using UnityEngine;

namespace Gameplay
{
    public class InteractableObject : MonoBehaviour
    {
        public string UID => uid;
        public bool IsMoving => currentState == State.Moving;
        public bool IsMerging => currentState == State.Merging;

        [Header("Identifiers")]
        [SerializeField] string uid;

        [Header("Components")]
        [SerializeField] new Collider collider;
        [SerializeField] new Rigidbody rigidbody;
        [SerializeField] Outline outline;

        [Header("Animation Config")]
        [SerializeField] GameplayObjectAnimationSettings animationSettings;

        private State currentState;
        private Vector3 originScale, selectedScale;
        private Vector3 targetPosition, velocity;
        private bool needMove, needScale, selected;
        private float scaleProgress;
        
        private void Start()
        {
            originScale = transform.localScale;
            selectedScale = animationSettings.ScaleModifier * originScale;
        }

        public void SetSelection(bool value)
        {
            selected = value;
            outline.enabled = value;

            if (animationSettings.ScaleDuration == 0)
            {
                transform.localScale = value ? selectedScale : originScale;
                needScale = false;
            }
            else needScale = true;
        }

        public void SetKinematic(bool value)
        {
            collider.enabled = !value;
            rigidbody.isKinematic = value;
        }

        public void MoveTo(Vector3 position)
        {
            if (currentState != State.Idle && currentState != State.Moving) return;
            var distanceSqr = (position - targetPosition).sqrMagnitude;
            if (distanceSqr < 0.001f) return;
            currentState = State.Moving;
            needMove = true;
            targetPosition = position;
            velocity = Vector3.zero;
        }

        public void Merge(Vector3 mergePosition)
        {
            needMove = false;
            if (animationSettings.MergingTime <= 0)
            {
                currentState = State.Merged;
            }
            else
            {
                currentState = State.Merging;
                MergeAsync(mergePosition, destroyCancellationToken).Forget();
            }
        }

        public void ResetRotation()
        {
            transform.rotation = Quaternion.identity;
        }

        private void Update()
        {
            if (needMove) UpdateMovement();
            if (needScale) UpdateScale();
        }

        private void UpdateMovement()
        {
            Vector3 displacement = targetPosition - transform.position;

            var targetReached = displacement.sqrMagnitude < 0.001f;
            if (targetReached && currentState == State.Moving) currentState = State.Idle;

            if (velocity.sqrMagnitude < 0.001f && targetReached)
            {
                needMove = false;
                transform.position = targetPosition;
                return;
            }

            Vector3 acceleration = animationSettings.MovingSpring * displacement - animationSettings.MovingDamping * velocity;
            velocity += acceleration * Time.deltaTime;
            transform.position += velocity;
        }

        private void UpdateScale()
        {
            scaleProgress += (selected ? 1 : -1) * Time.deltaTime / animationSettings.ScaleDuration;
            transform.localScale = Vector3.Lerp(originScale, selectedScale, animationSettings.ScaleCurve.Evaluate(scaleProgress));
            if (selected && scaleProgress > 1 || !selected && scaleProgress < 0)
            {
                needScale = false;
            }
        }

        private async UniTaskVoid MergeAsync(Vector3 mergePosition, CancellationToken token)
        {
            var time = 0f;
            var startScale = transform.localScale;
            var startPosition = transform.position;
            while (time < 1)
            {
                time += Time.deltaTime / animationSettings.MergingTime;
                var scaleModifier = animationSettings.MergingScaleCurve.Evaluate(time);
                transform.localScale = startScale * scaleModifier;

                var position = Vector3.LerpUnclamped(startPosition, mergePosition, animationSettings.MergingMoveCurve.Evaluate(time));
                transform.position = position;
                await UniTask.Yield(token);
            }

            currentState = State.Merged;
        }

        private enum State
        {
            Idle,
            Moving,
            Merging,
            Merged
        }
    }
}