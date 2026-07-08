using UnityEngine;

namespace Components.CharacterComponents
{
    public class FaceAnimationComponent : MonoBehaviour
    {
        private static readonly int Patience = Animator.StringToHash("Patience");
        private static readonly int IsBlinking = Animator.StringToHash("isBlinking");

        [SerializeField] private Animator eyebrowAnimator;
        [SerializeField] private Animator blinkAnimator;
        [SerializeField] private Animator mouthAnimator;
        
        public bool Initialize()
        {
            if (eyebrowAnimator == null)
            {
                Debug.LogError($"eyebrowAnimator is null");
                return false;
            }

            if (blinkAnimator == null)
            {
                Debug.LogError($"blinkAnimator is null");
                return false;
            }
        
            if (mouthAnimator == null)
            {
                Debug.LogError($"mouthAnimator is null");
                return false;
            }

            return true;
        }
        
        public void SetEmotion(float patience)
        {
            eyebrowAnimator.SetFloat(Patience, patience);
            mouthAnimator.SetFloat(Patience, patience);
        }

        public void SetBlink()
        {
            float random = UnityEngine.Random.value;

            bool isBlinking = random > 0.996f;
            blinkAnimator.SetBool(IsBlinking, isBlinking);
        }
    }
}