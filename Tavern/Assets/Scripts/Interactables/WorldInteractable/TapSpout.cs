using Interfaces;
using UnityEngine;

namespace Interactables.WorldInteractable
{
    public class TapSpout : MonoBehaviour
    {
        private BeerTapController tap;
        private IFillable currentFillable;

        private void Start()
        {
            tap = GetComponentInParent<BeerTapController>();
        }

        private void OnTriggerStay(Collider other)
        {
            IFillable fillable = other.GetComponent<IFillable>();
            if (fillable != null)
                currentFillable = fillable;
        }

        private void OnTriggerExit(Collider other)
        {
            IFillable fillable = other.GetComponent<IFillable>();
            if (fillable == currentFillable)
                currentFillable = null;
        }

        public void OnUpdate(float handleAngle, float maxAngle)
        {
            if (currentFillable == null) return;
            
            float fillRate = handleAngle / maxAngle;
            currentFillable.Fill(fillRate);
        }
    }
}
