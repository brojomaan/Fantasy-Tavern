using UnityEngine;

namespace Components.NPCComponents
{
    public class NpcNeedsComponent : MonoBehaviour
    {
        [SerializeField] private float maxPatience = 30f;
        [SerializeField] private float patienceDecayRate = 1f;

        private float patienceCurrent;
        private bool isDecaying = false;

        public float GetPatience() => patienceCurrent;
        public float GetPatienceNormalized() => patienceCurrent / maxPatience;
        public bool HasPatience() => patienceCurrent > 0f;

        public bool Initialize()
        {
            patienceCurrent = maxPatience;
            return true;
        }

        public void StartDecay()
        {
            isDecaying = true;
        }

        public void StopDecay()
        {
            isDecaying = false;
        }

        public void OnUpdate()
        {
            if (!isDecaying) return;

            patienceCurrent = Mathf.Max(0f, patienceCurrent - patienceDecayRate * Time.deltaTime);
        }
    }
}