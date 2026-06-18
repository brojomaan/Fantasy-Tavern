using Interfaces;
using Player;
using UnityEngine;

namespace Components.CharacterComponents
{
    public class InteractComponent : MonoBehaviour
    {
        [SerializeField] private float interactRange = 2f;
        [SerializeField] private LayerMask interactLayer;

        private Camera cam;
        private IInteractable currentHover;
        private PlayerVisual visual;
        private HoldComponent holdComponent;

        //TODO uncouple player visual from component at some point
        
        public bool Initialize(PlayerVisual playerVisual, HoldComponent holdComp)
        {
            cam = Camera.main;
            visual = playerVisual;
            holdComponent = holdComp;
            
            return true;
        }

        public void OnUpdate(bool pickupPressed)
        {
            HandleHover();
            if (pickupPressed) HandlePickup();
        }

        private void HandleHover()
        {
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactLayer))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();

                if (interactable != null && interactable.CanInteractWith(holdComponent.GetHeldItem()))
                {
                    if (interactable != currentHover)
                    {
                        currentHover?.OnHoverExit();
                        currentHover = interactable;
                        currentHover?.OnHoverEnter();
                    }

                    visual.SetIKTarget(holdComponent.IsHolding() 
                        ? holdComponent.GetCarrySocket() 
                        : interactable.GetHoverSocket());
                    return;
                }
            }

            if (currentHover != null)
            {
                currentHover.OnHoverExit();
                currentHover = null;
            }

            visual.SetIKTarget(holdComponent.IsHolding() 
                ? holdComponent.GetCarrySocket() 
                : null);
        }

        private void HandlePickup()
        {
            if (currentHover == null) return;

            GameObject obj = (currentHover as MonoBehaviour)?.gameObject;
            IHoldable holdable = obj?.GetComponent<IHoldable>();

            if (holdable != null)
            {
                holdComponent.PickUp(obj, holdable);
                visual.SetIKTarget(holdComponent.GetCarrySocket());
            }
        }

        public bool HasHover() => currentHover != null;
    }
}