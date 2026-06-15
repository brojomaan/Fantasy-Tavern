using UnityEngine;

namespace Components.CharacterComponents
{
    public class AnimationComponent : MonoBehaviour
    {
        private static readonly int IsMoving = Animator.StringToHash("isMoving");
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int VelocityX = Animator.StringToHash("velocityX");
        private static readonly int VelocityZ = Animator.StringToHash("velocityZ");
        private static readonly int IsCrouching = Animator.StringToHash("isCrouching");
        private static readonly int JumpPressed = Animator.StringToHash("jumpPressed");
        private static readonly int IsGrounded = Animator.StringToHash("isGrounded");

        [SerializeField] private float maxVelocity = 7f;
        [SerializeField] private Animator animator;
        
        private float minAnimSpeed = 1f;
        private float maxAnimSpeed = 2f;
        
        public bool Initialize()
        {
            if (animator == null)
            {
                Debug.LogError($"AnimationComponent : Animator is null.");
                return false;
            }
            
            
            return true;
        }

        public void SetGrounded(bool isGrounded)
        {
            animator.SetBool(IsGrounded, isGrounded);
        }
        
        public void SetVerticalVelocity(float verticalVelocity)
        {
            animator.SetFloat("verticalVelocity", verticalVelocity);
        }
        
        public void SetWalking(Vector2 input)
        {
            bool moving = input.x != 0 || input.y != 0;
            animator.SetBool(IsMoving, moving);
            
            animator.SetFloat(VelocityX, input.x);
            animator.SetFloat(VelocityZ, input.y);
            
        }

        public void SetSpeed(float velocity)
        {
            float normalised = Mathf.InverseLerp(0f, maxVelocity, velocity);
            animator.speed = Mathf.Lerp(minAnimSpeed, maxAnimSpeed, normalised);
        }

        public void SetCrouching(bool crouching)
        {
            animator.SetBool(IsCrouching, crouching);
        }
    }
}