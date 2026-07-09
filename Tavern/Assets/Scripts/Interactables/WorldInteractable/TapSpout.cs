using Interfaces;
using Liquids;
using UnityEngine;

namespace Interactables.WorldInteractable
{
    public class TapSpout : MonoBehaviour
    {
        private IFillable currentFillable;
        
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

        public void OnUpdate(float handleAngle, float maxAngle, LiquidData liquidId)
        {
            if (currentFillable == null) return;
            
            float fillRate = handleAngle / maxAngle;
            currentFillable.Fill(fillRate, liquidId);
        }
    }
}
