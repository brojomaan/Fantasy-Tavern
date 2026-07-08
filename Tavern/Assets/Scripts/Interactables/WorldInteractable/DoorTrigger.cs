using Coherence;
using GameManagement;
using UnityEngine;

namespace Interactables.WorldInteractable
{
    public class DoorTrigger : WorldInteractable
    {

        private bool hasTriggered = false;
        public override void OnInteract()
        {
            if (hasTriggered) return;
            Debug.Log($"Door Triggered");
            hasTriggered = true;
            
            //This doesnt work
            //GameManager.Instance.CmdRequestStartGame();
            
            //This does but i dont understand why
            GameManager.Instance.CoherenceSync.SendCommand<GameManager>(
                nameof(GameManager.CmdRequestStartGame),
                MessageTarget.StateAuthorityOnly);
        }

        public override void OnInteractRelease()
        { 
            //Do nothing right now
        }
        
        public override void OnInteractUpdate(Vector2 lookDirection) { }
    }
}