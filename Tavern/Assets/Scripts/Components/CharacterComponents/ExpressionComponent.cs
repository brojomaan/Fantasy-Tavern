using UnityEngine;

namespace Components.CharacterComponents
{
    public class ExpressionComponent : MonoBehaviour
    {
        [SerializeField] private GameObject defaultMouth;
        [SerializeField] private GameObject angryMouth;
        [SerializeField] private GameObject sadMouth;
        [SerializeField] private GameObject happyMouth;
        [SerializeField] private GameObject[] talkingMouths;
        [SerializeField] private float cycleSpeed = 8f;

        private GameObject currentMouth;
        private float cycleTimer;

        public bool Initialize()
        {
            if (defaultMouth == null)
            {
                Debug.LogError($"ExpressionComponent::Initialize(): defaultMouth is null"); 
                return false;
            }

            if (talkingMouths == null || talkingMouths.Length == 0)
            {
                Debug.LogError("ExpressionComponent::Initialize(): talkingMouths is empty."); 
                return false;
            }
            
            SetMouth(defaultMouth);
            return true;
        }

        public void IsTalking(bool isTalking)
        {
            if (isTalking)
            {
                cycleTimer += Time.deltaTime;
                if (cycleTimer >= 1f / cycleSpeed)
                {
                    cycleTimer = 0f;
                    SetMouth(talkingMouths[Random.Range(0, talkingMouths.Length)]);
                }
            }
            else
            {
                cycleTimer = 0f;
                SetMouth(defaultMouth);
            }
        }

        private void SetMouth(GameObject mouth)
        {
            if (currentMouth == mouth) return;
            if (currentMouth != null) currentMouth.SetActive(false);
            currentMouth = mouth;
            currentMouth.SetActive(true);
            Debug.Log($"Active mouth is now: {currentMouth.name}");
        }
    }
}