using UnityEngine;

namespace Interfaces
{
    public interface IInteractable
    {
        Transform GetHoverSocket();
        Transform GetHeadSocket();
        void OnHoverEnter();
        void OnHoverExit();
        bool CanInteractWith(IHoldable heldItem);

    }

    public interface IHoldable
    {
        string ItemId { get; }
        Transform GetCarrySocket();
        
        Vector3 CarryPositionOffset { get; }
        Vector3 CarryRotationOffset { get; }

        void OnPickup();
        void OnDrop();
    }
}