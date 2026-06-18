using Interfaces;
using UnityEngine;

namespace Components.CharacterComponents
{
    public class HoldComponent : MonoBehaviour
    {
        [SerializeField] private Transform carrySocket;
        [SerializeField] private float carryLerpSpeed = 8f;

        private IHoldable heldItem;
        private GameObject heldObject;

        public bool Initialize()
        {
            if (carrySocket == null) { Debug.LogError($"HoldComponent::Initialize(): Carry Socket is null"); return false; }

            return true;
        }

        public void OnUpdate()
        {
            if (heldObject != null)
            {
                Vector3 targetPosition = carrySocket.position + 
                                         carrySocket.TransformDirection(heldItem.CarryPositionOffset);
        
                Quaternion targetRotation = carrySocket.rotation * 
                                            Quaternion.Euler(heldItem.CarryRotationOffset);

                heldObject.transform.position = Vector3.Lerp(
                    heldObject.transform.position, 
                    targetPosition, 
                    Time.deltaTime * carryLerpSpeed);

                heldObject.transform.rotation = Quaternion.Lerp(
                    heldObject.transform.rotation, 
                    targetRotation, 
                    Time.deltaTime * carryLerpSpeed);
            }
        }

        public void PickUp(GameObject objectToPickUp, IHoldable holdable)
        {
            heldObject = objectToPickUp;
            heldItem = holdable;
            heldItem.OnPickup();
        }

        public void Drop()
        {
            heldItem.OnDrop();
            heldObject = null;
            heldItem = null;
        }

        public bool IsHolding() => heldItem != null;
        public IHoldable GetHeldItem() => heldItem;
        public Transform GetCarrySocket() => carrySocket;
    }
}