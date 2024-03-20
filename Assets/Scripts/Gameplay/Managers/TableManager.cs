using System;
using System.Collections.Generic;
using Core.Events;
using Gameplay.Configs;
using Gameplay.Events;
using Gameplay.Views;
using UnityEngine;
using VContainer.Unity;
using Random = Unity.Mathematics.Random;

namespace Gameplay.Managers
{
    public class TableManager : IInitializable, IStartable, IDisposable
    {
        private readonly IEventPublisher<TableClearedEvent> tableClearedPublisher;
        private readonly IEventSubscriber<ObjectAddedToPoolEvent> addedToPoolSubscriber;
        private readonly IEventSubscriber<MatchingEvent> matchingSubscriber;
        private readonly TableView view;
        private readonly GameplayData gameplayData;
        private readonly GameplaySettings gameplaySettings;

        private List<GameObject> collectibles;

        public TableManager(
            IEventPublisher<TableClearedEvent> tableClearedPublisher, 
            IEventSubscriber<ObjectAddedToPoolEvent> addedToPoolSubscriber, 
            IEventSubscriber<MatchingEvent> matchingSubscriber, 
            TableView view, GameplayData gameplayData, GameplaySettings gameplaySettings)
        {
            this.tableClearedPublisher = tableClearedPublisher;
            this.addedToPoolSubscriber = addedToPoolSubscriber;
            this.matchingSubscriber = matchingSubscriber;
            this.view = view;
            this.gameplayData = gameplayData;
            this.gameplaySettings = gameplaySettings;
        }

        public void Initialize()
        {
            addedToPoolSubscriber.Subscribe(OnObjectAddedToPoolHandler);
            matchingSubscriber.Subscribe(OnMatchingHandler);
        }


        public void Start()
        {
            SpawnObjects(gameplayData.CollectibleObjects, gameplaySettings.MatchSize);
        }


        public void Dispose()
        {
            addedToPoolSubscriber.Dispose();
            matchingSubscriber.Dispose();
        }

        private void OnObjectAddedToPoolHandler(ObjectAddedToPoolEvent args)
        {
            collectibles.Remove(args.Object);
            if (collectibles.Count == 0) tableClearedPublisher.Publish(TableClearedEvent.Empty);
        }
        
        private void OnMatchingHandler(MatchingEvent arg)
        {
            for (var i = 0; i < arg.MatchedObjects.Length; i++)
            {
                var interactableObject = arg.MatchedObjects[i];
                GameObject.Destroy(interactableObject.gameObject);
            }
        }

        private void SpawnObjects(ICollection<InteractableObject> prefabs, int matchSize, uint seed = 0)
        {
            collectibles = new List<GameObject>(prefabs.Count * matchSize);

            var random = new Random(seed == 0 ? (uint)Time.frameCount : seed);
            var spawnZoneSize = view.SpawnZoneSize;
            var spawnZoneOffset = view.SpawnZoneOffset + view.transform.position;
            
            var minPos = new Vector3(-spawnZoneSize.x / 2 + spawnZoneOffset.x, 0, -spawnZoneSize.y / 2 + spawnZoneOffset.y);
            var maxPos = new Vector3(spawnZoneSize.x / 2 + spawnZoneOffset.x, 0, spawnZoneSize.y / 2 + spawnZoneOffset.y);
            
            foreach (var collectible in prefabs)
            {
                for (int i = 0; i < matchSize; i++)
                {
                    var instance = GameObject.Instantiate(collectible, view.Container);
                    instance.transform.position = random.NextFloat3(minPos, maxPos);
                    collectibles.Add(instance.gameObject);
                }
            }
        }

    }
}