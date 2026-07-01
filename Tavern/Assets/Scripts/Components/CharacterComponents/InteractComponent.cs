using Interactables.WorldInteractable;
using Interfaces;
using NPC;
using Player;
using Unity.Cinemachine;
using UnityEngine;

namespace Components.CharacterComponents
{
    public class InteractComponent : MonoBehaviour
    {
        [SerializeField] private float interactRange = 2f;
        [SerializeField] private float placeRange = 2f;
        [SerializeField] private LayerMask interactLayer;
        [SerializeField] private LayerMask placeableLayer;

        private Camera cam;
        private IInteractable currentHover;
        private PlayerVisual visual;
        private HoldComponent holdComponent;
        
        public bool IsPreviewingPlacement { get; private set; }

        //TODO uncouple player visual from component at some point
        
        public bool Initialize(PlayerVisual playerVisual, HoldComponent holdComp)
        {
            if (playerVisual == null) { Debug.LogError("InteractComponent::Initialize(): playerVisual is null."); return false; }
            if (holdComp == null) { Debug.LogError("InteractComponent::Initialize(): holdComp is null."); return false; }

            cam = Camera.main;
            visual = playerVisual;
            holdComponent = holdComp;
            
            return true;
        }

        public void OnUpdate()
        {
            if (holdComponent.IsHolding())
                HandleHoldingRaycast();
            else
                HandleHover();
        }

        public void TryPickup()
        {
            HandlePickup();
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
                        if (IsHoverValid()) currentHover.OnHoverExit();
                        currentHover = interactable;
                        currentHover.OnHoverEnter();
                    }

                    visual.SetIKTarget(interactable.GetHoverSocket());
                    return;
                }
            }

            if (IsHoverValid())
            {
                currentHover.OnHoverExit();
                currentHover = null;
            }

            visual.SetIKTarget(null);
        }
        
        private void HandleHoldingRaycast()
        {
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);

             // Check for NPC or interactable while holding
            if (Physics.Raycast(ray, out RaycastHit interactHit, interactRange, interactLayer))
            {
                IInteractable interactable = interactHit.collider.GetComponentInParent<IInteractable>();
                if (interactable != null && interactable.CanInteractWith(holdComponent.GetHeldItem()))
                {
                    if (interactable != currentHover)
                    {
                        if (IsHoverValid()) currentHover.OnHoverExit();
                        currentHover = interactable;
                        currentHover.OnHoverEnter();
                    }
                }
                else if (IsHoverValid())
                {
                    currentHover.OnHoverExit();
                    currentHover = null;
                }
            }
            else if (IsHoverValid())
            {
                currentHover.OnHoverExit();
                currentHover = null;
            }

            // Separately check for placement
            if (Physics.Raycast(ray, out RaycastHit placeHit, placeRange, placeableLayer))
            {
                IsPreviewingPlacement = true;

                Vector3 targetPosition = placeHit.point + 
                                         holdComponent.GetCarrySocket().TransformDirection(holdComponent.GetHeldItem().CarryPositionOffset);
                Quaternion targetRotation = holdComponent.GetCarrySocket().rotation * 
                                            Quaternion.Euler(holdComponent.GetHeldItem().CarryRotationOffset);

                holdComponent.SetTargetPosition(targetPosition);
                holdComponent.SetTargetRotation(targetRotation);
                visual.SetIKTarget(holdComponent.GetGripSocket());
            }
            else
            {
                IsPreviewingPlacement = false;

                Vector3 targetPosition = holdComponent.GetCarrySocket().position + 
                                         holdComponent.GetCarrySocket().TransformDirection(holdComponent.GetHeldItem().CarryPositionOffset);
                Quaternion targetRotation = holdComponent.GetCarrySocket().rotation * 
                                            Quaternion.Euler(holdComponent.GetHeldItem().CarryRotationOffset);

                holdComponent.SetTargetPosition(targetPosition);
                holdComponent.SetTargetRotation(targetRotation);
                visual.SetIKTarget(currentHover != null ? null : holdComponent.GetGripSocket()); 
            } 
        }

        private void HandlePickup()
        {
            if (!IsHoverValid()) return;

            GameObject obj = (currentHover as MonoBehaviour)?.gameObject;
            IHoldable holdable = obj?.GetComponent<IHoldable>();

            if (holdable != null)
            {
                holdComponent.PickUp(obj, holdable);
                if (IsHoverValid())currentHover.OnHoverExit();
                currentHover = null;
                visual.SetIKTarget(holdComponent.GetGripSocket());
            }
        }

        public WorldInteractable GetCurrentWorldInteractable()
        {
            return (currentHover as MonoBehaviour)?.GetComponent<WorldInteractable>();
        }

        public NpcController GetCurrentNpcController()
        {
            return (currentHover as MonoBehaviour)?.GetComponent<NpcController>();
        }

        private bool IsHoverValid()
        {
            if (currentHover == null) return false;
            MonoBehaviour mb = currentHover as MonoBehaviour;
            return mb != null && mb.gameObject != null;
        }

        public bool HasHover() => currentHover != null;
    }
}