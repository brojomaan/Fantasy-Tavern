using UnityEngine;

namespace Interfaces
{
    public interface IInteractable
    {
        Transform GetHoverSocket();
        Transform GetGripSocket();
        void OnHoverEnter();
        void OnHoverExit();
        bool CanInteractWith(IHoldable heldItem);
        void OnInteract();
        void OnInteractRelease();

    }

    public interface IHoldable
    {
        string ItemId { get; }
        Transform GetGripSocket();
        Vector3 CarryPositionOffset { get; }
        Vector3 CarryRotationOffset { get; }

        void OnPickup();
        void OnDrop();
    }
}