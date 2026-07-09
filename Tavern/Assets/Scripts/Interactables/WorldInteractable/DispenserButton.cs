using Coherence;
using Coherence.Toolkit;
using Interfaces;
using Liquids;
using UnityEngine;

namespace Interactables.WorldInteractable
{
    public class DispenserButton : WorldInteractable
    {
        [SerializeField] private CoherenceSync sync;
        [SerializeField] private LiquidData liquidData;
        [SerializeField] private TapSpout spout;
        [SerializeField] private float fillRate = 0.5f;

        [Sync] public bool syncedIsFlowing;
        
        
        public override bool CanInteractWith(IHoldable heldItem) => heldItem == null;
        public override void OnInteract()
        {
            if (!sync.HasStateAuthority)
            {
                sync.RequestAuthority(AuthorityType.Full);
            }
        }

        public override void OnInteractUpdate(Vector2 lookDirection)
        {
            if (!sync.HasStateAuthority) return;
            
            syncedIsFlowing = true;
            spout.OnUpdate(fillRate, 1f, liquidData);
            
        }

        public override void OnInteractRelease()
        {
            syncedIsFlowing = false;
        }
    }
}
