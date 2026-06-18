using Coherence;
using Coherence.Toolkit;
using Interfaces;
using UnityEngine;

namespace Interactables
{
    public class ItemController : MonoBehaviour, IHoldable, IInteractable
    {
        [SerializeField] private ItemData data;
        [SerializeField] private ItemVisual visual;

        [SerializeField] private PhysicsComponent physicsComponent;
        [SerializeField] private CoherenceSync sync;

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

        private bool hasInitialized = false;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
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
            if (sync.HasStateAuthority)
                CmdPickup(sync.CoherenceBridge.ClientID.ToString());
        }

        public void OnDrop()
        {
            if (sync.HasStateAuthority)
                CmdDrop();
        }

        [Command(defaultRouting = MessageTarget.AuthorityOnly)]
        public void CmdPickup(string playerId)
        {
            isHeld = true;
            holderId = playerId;
            physicsComponent.SetKinematic(true);
            physicsComponent.SetCollider(false);
            
        }

        [Command(defaultRouting = MessageTarget.AuthorityOnly)]
        public void CmdDrop()
        {
            isHeld = false;
            holderId = string.Empty;
            physicsComponent.SetKinematic(false);
            physicsComponent.SetCollider(true);

        }

        [Command(defaultRouting = MessageTarget.AuthorityOnly)]
        public void CmdPlace(Vector3 position)
        {
            isHeld = false;
            holderId = string.Empty;
            physicsComponent.SetKinematic(false);
            transform.position = position;
            physicsComponent.SetCollider(true);
        }

        public void OnIsHeldSynced(bool previous, bool current)
        {
            physicsComponent.SetKinematic(current);
            if (current)
                visual.OnPickup();
            else
                visual.OnDrop();
        }
    }
}
