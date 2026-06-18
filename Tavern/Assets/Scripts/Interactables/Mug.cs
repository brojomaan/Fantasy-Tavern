

using Interfaces;
using UnityEngine;

namespace Interactables
{
    public class Mug : MonoBehaviour, IInteractable, IHoldable
    {

        [SerializeField] private Transform hoverSocket;
        [SerializeField] private Transform gripSocket;

        [SerializeField] private BoxCollider boxCollider;
        [SerializeField] private Vector3 carryPositionOffset;
        [SerializeField] private Vector3 carryRotationOffset;


        public Transform GetHoverSocket() => hoverSocket;
        public Transform GetHeadSocket() => gripSocket;
        public Transform GetCarrySocket() => gripSocket;
        public Vector3 CarryPositionOffset => carryPositionOffset;
        public Vector3 CarryRotationOffset => carryRotationOffset;

        public void OnPickup()
        {
            if (TryGetComponent<Rigidbody>(out var rb))
                rb.isKinematic = true;

            boxCollider.enabled = false;
        }

        public void OnDrop()
        {
            throw new System.NotImplementedException();
        }

        public bool CanInteractWith(IHoldable heldItem) => heldItem == null;

        public void OnHoverEnter()
        {
            //Hightlight later
        }

        public void OnHoverExit()
        {
            //Remove Highlight
        }

        public string ItemId => "mug";
        
    }
}