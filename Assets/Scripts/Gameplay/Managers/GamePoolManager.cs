using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.Configs;
using Gameplay.Events;
using Gameplay.Events.Aggregators;
using Gameplay.Views;
using UnityEngine;
using VContainer.Unity;

namespace Gameplay.Managers
{
    public class GamePoolManager : IInitializable, IDisposable
    {
        private readonly PoolView view;
        private readonly GamePoolEventsAggregator eventsAggregator;
        private readonly GameplaySettings gameplaySettings;

        private List<InteractableObject> pool;

        private int maxPoolSize;
        private int mergingsCount;

        private CancellationTokenSource disposeTokenSource;

        public GamePoolManager(PoolView view, GamePoolEventsAggregator eventsAggregator, GameplaySettings gameplaySettings)
        {
            this.view = view;
            this.eventsAggregator = eventsAggregator;
            this.gameplaySettings = gameplaySettings;
        }

        public void Initialize()
        {
            disposeTokenSource = new CancellationTokenSource();
            maxPoolSize = view.Slots.Length;
            pool = new(maxPoolSize);
            eventsAggregator.ObjectSelectedSubscriber.Subscribe(OnObjectSelectedHandler);
        }

        public void Dispose()
        {
            eventsAggregator.Dispose();
            disposeTokenSource.Cancel();
            disposeTokenSource.Dispose();
        }

        private void OnObjectSelectedHandler(ObjectSelectedEvent args)
        {
            if (pool.Count >= maxPoolSize) return;
            AddObjectToPool(args.Object);
            MoveObjects();

            var uid = args.Object.UID;
            if (CheckMatch(uid)) ProcessMatch(uid);
            else if (pool.Count == maxPoolSize && mergingsCount == 0) eventsAggregator.PoolOverflowPublisher.Publish(PoolOverflowEvent.Empty);
        }

        private void AddObjectToPool(InteractableObject interactable)
        {
            eventsAggregator.ObjectAddedToPoolPublisher.Publish(new ObjectAddedToPoolEvent(interactable.gameObject));

            interactable.SetKinematic(true);
            interactable.ResetRotation();

            if (pool.Count > 1)
            {
                var uid = pool[^1].UID;
                if (!interactable.UID.Equals(uid))
                {
                    for (var i = pool.Count - 2; i >= 0; i--)
                    {
                        if (pool[i].UID != interactable.UID) continue;
                        pool.Insert(i + 1, interactable);
                        return;
                    }
                }
            }

            pool.Add(interactable);
        }

        private Vector3 GetSlotPosition(int index) => view.Slots[index].position + view.Offset;

        private void MoveObjects()
        {
            for (var i = 0; i < pool.Count; i++)
            {
                pool[i].MoveTo(GetSlotPosition(i));
            }
        }

        private bool CheckMatch(string uid)
        {
            var count = 0;
            for (var i = 0; i < pool.Count; i++)
            {
                if (pool[i].UID.Equals(uid)) count++;
            }

            return count >= gameplaySettings.MatchSize;
        }

        private void ProcessMatch(string uid)
        {
            var matched = new InteractableObject[gameplaySettings.MatchSize];
            var count = 0;
            for (var i = 0; i < pool.Count; i++)
            {
                if (!pool[i].UID.Equals(uid)) continue;
                matched[count] = pool[i];
                count++;
                if (count == gameplaySettings.MatchSize) break;
            }

            if (count < gameplaySettings.MatchSize) return;
            
            ProcessMergingAsync(matched, disposeTokenSource.Token).Forget();
        }

        private async UniTaskVoid ProcessMergingAsync(InteractableObject[] matchedObjects, CancellationToken token)
        {
            mergingsCount++;
            while (!AreObjectsIdle()) await UniTask.Yield(token);

            var leftPosition = matchedObjects[0].transform.position;
            var rightPosition = matchedObjects[^1].transform.position;
            var middle = (leftPosition + rightPosition) / 2;

            for (var index = 0; index < matchedObjects.Length; index++)
            {
                matchedObjects[index].Merge(middle);
            }

            while (!AreObjectsIdle()) await UniTask.Yield(token);
            
            mergingsCount--;

            for (var index = 0; index < matchedObjects.Length; index++)
            {
                pool.Remove(matchedObjects[index]);
            }

            var matchingEvent = new MatchingEvent(matchedObjects);
            eventsAggregator.MatchingPublisher.Publish(matchingEvent);
            
            MoveObjects();
            return;

            bool AreObjectsIdle()
            {
                foreach (var obj in matchedObjects)
                    if (obj.IsMoving || obj.IsMerging)
                        return false;

                return true;
            }
        }
    }
}