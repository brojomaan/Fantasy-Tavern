using UnityEngine;

namespace Interactables
{
    public class ItemVisual : MonoBehaviour
    {
        [SerializeField] private MeshRenderer itemRenderer;
        [SerializeField] private Material material;

        public  virtual bool Initialize()
        {
            if (itemRenderer == null) { Debug.LogError("ItemController::Initialize(): itemRenderer = null"); return false; }

            return true;
        }

        public virtual void OnHoverEnter()
        {
        
        }

        public virtual void OnHoverExit()
        {
        
        }

        public virtual void OnPickup()
        {
        
        }

        public virtual void OnDrop()
        {
        
        }
    }
}
