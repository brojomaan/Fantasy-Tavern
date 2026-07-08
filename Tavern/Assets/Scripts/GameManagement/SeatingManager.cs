using Interactables.WorldInteractable;
using UnityEngine;

namespace GameManagement
{
    public class SeatingManager : MonoBehaviour
    {
        public static SeatingManager Instance { get; private set; }

        [SerializeField] private SeatController[] seats;

        private void Awake()
        {
            if (Instance != null) 
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public SeatController FindAvailableSeat()
        {
            foreach (SeatController seat in seats)
            {
                if (!seat.IsOccupied) return seat;
            }

            return null;
        }

        public bool HasAvailableSeat() => FindAvailableSeat() != null;
    }
}