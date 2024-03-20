using System;
using UnityEngine;

namespace Gameplay.Controllers
{
    public class InputController : MonoBehaviour
    {
        public event Action<GameObject> OnObjectMouseOver;
        public event Action<GameObject> OnObjectClicked;
        
        [Header("Raycast Settings")]
        [SerializeField] LayerMask raycastLayer;
        [SerializeField] float raycastDistance = 20f;

        private GameObject selectedObject, raycastedObject;

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                var raycast = Physics.Raycast(ray, out var hit, raycastDistance, raycastLayer);
                
                raycastedObject = raycast ? hit.collider.gameObject : null;

                if (raycastedObject != selectedObject)
                {
                    selectedObject = raycastedObject;
                    OnObjectMouseOver?.Invoke(selectedObject);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                OnObjectClicked?.Invoke(selectedObject);
            }
        }
    }
}