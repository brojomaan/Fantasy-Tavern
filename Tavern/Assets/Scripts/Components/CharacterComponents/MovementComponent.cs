using UnityEngine;

namespace Components.CharacterComponents
{
    public class MovementComponent : MonoBehaviour
    {
        [SerializeField] private float speedWalk = 5f;
        [SerializeField] private float speedRun = 10f;
        [SerializeField] private float speedCrouch = 2f;
        [SerializeField] private float jumpHeight = 1.5f;
        [SerializeField] private float gravity = -9.81f;

        private float speedCurrent;
        private float verticalVelocity;
        private CharacterController characterController;

        public void Initialize(CharacterController cc)
        {
            if (cc == null) Debug.LogError("MovementComponent::Initialize(): CharacterController is null.");
            characterController = cc;
            
        }

        public void OnUpdate(Vector2 moveDirection, bool sprintPressed, bool crouchPressed, bool jumpPressed)
        {
            HandleSpeed(sprintPressed, crouchPressed);
            HandleJump(jumpPressed);
            HandleMove(moveDirection);
        }

        private void HandleSpeed(bool sprintPressed, bool crouchPressed)
        {
            speedCurrent = speedWalk;
            if (sprintPressed) speedCurrent = speedRun;
            if (crouchPressed) speedCurrent = speedCrouch;
        }

        private void HandleJump(bool jumpPressed)
        {
            if (characterController.isGrounded) verticalVelocity = -2f;
            if (jumpPressed && characterController.isGrounded)
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            else
                verticalVelocity += gravity * Time.deltaTime;
        }

        private void HandleMove(Vector2 moveDirection)
        {
            Vector3 horizontal = transform.right * moveDirection.x + transform.forward * moveDirection.y;
            Vector3 movement = horizontal * speedCurrent * Time.deltaTime;
            movement.y = verticalVelocity * Time.deltaTime;
            characterController.Move(movement);
        }
    }
}