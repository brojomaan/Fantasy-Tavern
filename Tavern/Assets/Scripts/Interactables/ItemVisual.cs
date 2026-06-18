using UnityEngine;

namespace Interactables
{
    public class ItemVisual : MonoBehaviour
    {
        [SerializeField] private MeshRenderer itemRenderer;
        [SerializeField] private Material material;

        public bool Initialize()
        {
            if (itemRenderer == null) { Debug.LogError("ItemController::Initialize(): itemRenderer = null"); return false; }

            return true;
        }

        public void OnHoverEnter()
        {
        
        }

        public void OnHoverExit()
        {
        
        }

        public void OnPickup()
        {
        
        }

        public void OnDrop()
        {
        
        }
    }
}
