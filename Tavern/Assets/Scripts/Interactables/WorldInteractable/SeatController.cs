using UnityEngine;

namespace Interactables.WorldInteractable
{
    public class SeatController : MonoBehaviour
    {
        public bool IsOccupied { get; private set; }

        public bool TryClaim()
        {
            if (IsOccupied) return false;
            IsOccupied = true;
            Debug.Log($"{gameObject.name}: IsOccupied {IsOccupied}");
            return true;
        }

        public void Release()
        {
            Debug.Log($"{gameObject.name}: IsOccupied {IsOccupied}");
            IsOccupied = false;
        }
    }
}