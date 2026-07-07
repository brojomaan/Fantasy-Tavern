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
            
            GameManager.Instance.CmdRequestStartGame();
        }

        public override void OnInteractRelease()
        { 
            //Do nothing right now
        }
        
        public override void OnInteractUpdate(Vector2 lookDirection) { }
    }
}