using Interfaces;
using UnityEngine;

namespace Interactables.ItemInteractables
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Tavern/ItemData")]
    public class ItemData : ScriptableObject
    {
        [SerializeField] private string itemId;
        [SerializeField] private Vector3 carryPositionOffset;
        [SerializeField] private Vector3 carryRotationOffset;


        public string ItemId => itemId;
        public Vector3 CarryPositionOffset => carryPositionOffset;
        public Vector3 CarryRotationOffset => carryRotationOffset;

        public bool CanInteractWith(IHoldable heldItem)
        {
            if (heldItem == null) return true;
            return false;
        }
    }
}
