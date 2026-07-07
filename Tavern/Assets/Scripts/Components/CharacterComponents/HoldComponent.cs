using Interfaces;
using Unity.Mathematics;
using UnityEngine;

namespace Components.CharacterComponents
{
    public class HoldComponent : MonoBehaviour
    {
        [SerializeField] private Transform carrySocket;
        [SerializeField] private float carryLerpSpeed = 8f;

        private IHoldable heldItem;
        private GameObject heldObject;
        private Vector3 targetPosition;
        private Quaternion targetRotation;
        
        public bool IsHolding() => heldItem != null;
        public IHoldable GetHeldItem() => heldItem;
        public Transform GetCarrySocket() => carrySocket;

        private Transform cRoot;

        public bool Initialize()
        {
            if (carrySocket == null) { Debug.LogError($"HoldComponent::Initialize(): Carry Socket is null"); return false; }
            
            return true;
        }

        public void OnUpdate()
        {
            if (heldObject == null) return;

            heldObject.transform.position = Vector3.Lerp(
                heldObject.transform.position,
                targetPosition,
                Time.deltaTime * carryLerpSpeed);

            heldObject.transform.rotation = Quaternion.Lerp(
                heldObject.transform.rotation,
                targetRotation,
                Time.deltaTime * carryLerpSpeed);
        }

        public void SetTargetPosition(Vector3 position)
        {
            targetPosition = position;
        }

        public void SetTargetRotation(Quaternion rotation)
        {
            targetRotation = rotation;
        }

        public Transform GetGripSocket() => heldObject != null ? heldItem.GetGripSocket() : null;

        public void PickUp(GameObject objectToPickUp, IHoldable holdable)
        {
            if (heldObject != null) return;
            
            heldObject = objectToPickUp;
            heldItem = holdable;
            targetPosition = carrySocket.position;
            targetRotation = carrySocket.rotation;
            heldItem.OnPickup();
        }

        public void Place()
        {
            if (heldObject == null) return;
            
            heldItem.OnDrop();
            heldObject = null;
            heldItem = null;
        }

        public void Drop()
        {
            if (heldObject == null) return;
            
            heldItem.OnDrop();
            heldObject = null;
            heldItem = null;
        }
        
    }
}