using System;
using Core.Events;
using Gameplay.Controllers;
using Gameplay.Events;
using UnityEngine;
using VContainer.Unity;

namespace Gameplay.Managers
{
    public class SelectionManager : IInitializable, IDisposable
    {
        private readonly InputController inputController;
        private readonly IEventPublisher<ObjectSelectedEvent> objectSelectedEventPublisher;
        private readonly IEventSubscriber<GameplayFinishEvent> finishEventSubscriber;

        private InteractableObject selected;

        public SelectionManager(
            InputController inputController, 
            IEventPublisher<ObjectSelectedEvent> objectSelectedEventPublisher, 
            IEventSubscriber<GameplayFinishEvent> finishEventSubscriber)
        {
            this.inputController = inputController;
            this.objectSelectedEventPublisher = objectSelectedEventPublisher;
            this.finishEventSubscriber = finishEventSubscriber;
        }

        public void Initialize()
        {
            finishEventSubscriber.Subscribe(OnFinishGameHandler);

            inputController.OnObjectClicked += OnObjectClickedHandler;
            inputController.OnObjectMouseOver += OnObjectMouseOverHandler;
        }
        
        public void Dispose()
        {
            finishEventSubscriber.Dispose();

            inputController.OnObjectClicked -= OnObjectClickedHandler;
            inputController.OnObjectMouseOver -= OnObjectMouseOverHandler;
        }

        private void OnObjectMouseOverHandler(GameObject mouseOverObject)
        {
            if (selected) selected.SetSelection(false);
            if (!mouseOverObject) return;
            if (mouseOverObject.TryGetComponent(out InteractableObject selectable))
            {
                selectable.SetSelection(true);
                selected = selectable;
            }
        }

        private void OnObjectClickedHandler(GameObject clickedObject)
        {
            if (!clickedObject) return;
            if (clickedObject.TryGetComponent(out InteractableObject selectable))
            {
                selectable.SetSelection(false);
                selected = null;
                objectSelectedEventPublisher.Publish(new ObjectSelectedEvent(selectable));
            }
        }

        private void OnFinishGameHandler(GameplayFinishEvent args)
        {
            inputController.enabled = false;
        }
    }
}