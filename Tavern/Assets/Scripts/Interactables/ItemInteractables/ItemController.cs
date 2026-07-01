using Coherence;
using Coherence.Toolkit;
using Interactables.ItemInteractables;
using Interfaces;
using UnityEngine;

namespace Interactables
{
    public class ItemController : MonoBehaviour, IHoldable, IInteractable
    {
        [SerializeField] private ItemData data;
        [SerializeField] private ItemVisual visual;

        [SerializeField] private PhysicsComponent physicsComponent;
        [SerializeField] protected CoherenceSync sync;

        [SerializeField] private Transform hoverSocket;
        [SerializeField] private Transform gripSocket;

        [OnValueSynced(nameof(OnIsHeldSynced))]
        [Sync] public bool isHeld;
        [Sync] public string holderId;
        
        
        public string ItemId => data.ItemId;
        public Vector3 CarryPositionOffset => data.CarryPositionOffset;
        public Vector3 CarryRotationOffset => data.CarryRotationOffset;
        public Transform GetCarrySocket() => gripSocket;
        public Transform GetGripSocket() => gripSocket;
        public Transform GetHoverSocket() => hoverSocket;

        public bool CanInteractWith(IHoldable heldItem) => data.CanInteractWith(heldItem);
        public void OnInteract() { }
        public void OnInteractRelease() { }

        public void OnHoverEnter() => visual.OnHoverEnter();
        public void OnHoverExit() => visual.OnHoverExit();

        protected bool hasInitialized = false;

        private void Start()
        {
            Initialize();
        }

        public virtual void Initialize()
        {
            if (data == null) {Debug.LogError("ItemController::Initialize(): data is null");}
            if (visual == null) {Debug.LogError("ItemController::Initialize(): visual = null");}
            if (physicsComponent == null) {Debug.LogError("ItemController::Initialize(): physicsComponent = null");}
            if (sync == null) {Debug.LogError("ItemController::Initialize(): sync = null");}

            if (!visual.Initialize()) { Debug.LogError("ItemController::Initialize(): visual failed."); }
            if (!physicsComponent.Initialize()) {  Debug.LogError("ItemController::Initialize(): physics failed."); }
            
            hasInitialized = true;
        }

        public void OnPickup()
        {
            if (!sync.HasStateAuthority)
                sync.RequestAuthority(AuthorityType.Full);

            isHeld = true;
            physicsComponent.SetKinematic(true);
            physicsComponent.SetCollider(false);
            visual.OnPickup();
        }

        public void OnDrop()
        {
            isHeld = false;
            physicsComponent.SetKinematic(false);
            physicsComponent.SetCollider(true);
            visual.OnDrop();
        }
        
        public void OnPlace(Vector3 position)
        {
            isHeld = false;
            physicsComponent.SetKinematic(false);
            physicsComponent.SetCollider(true);
            transform.position = position;
            visual.OnDrop();
        }

        public void OnIsHeldSynced(bool previous, bool current)
        {
            if (current)
                visual.OnPickup();
            else
                visual.OnDrop();
        }
    }
}
