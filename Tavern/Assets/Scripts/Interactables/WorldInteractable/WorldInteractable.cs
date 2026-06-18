using Interfaces;
using UnityEngine;

namespace Interactables.WorldInteractable
{
    public abstract class WorldInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] protected Transform hoverSocket;

        public Transform GetHoverSocket() => hoverSocket;
        public Transform GetGripSocket() => hoverSocket;


        public virtual bool CanInteractWith(IHoldable heldItem) => true;
        
        public virtual void OnHoverEnter() { }
        public virtual void OnHoverExit() { }
        
        public abstract void OnInteract();

        public virtual void OnInteractUpdate(Vector2 lookDirection) { }

        public abstract void OnInteractRelease();

    }
}