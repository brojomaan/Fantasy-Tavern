using UnityEngine;

namespace Components.CharacterComponents
{
    public class MovementComponent : MonoBehaviour
    {
        [SerializeField] private float speedWalk = 5f;
        [SerializeField] private float speedRun = 10f;
        [SerializeField] private float speedCrouch = 3f;
        [SerializeField] private float jumpHeight = 1.5f;
        [SerializeField] private float strafeMultiplier = 0.8f;
        [SerializeField] private float backwardsMultiplier = 0.6f;
        [SerializeField] private float crouchMultiplier = 0.4f;
        [SerializeField] private float gravity = -9.81f;
        [SerializeField] private float standingHeight = 1.6f;
        [SerializeField] private float crouchHeight = 1.1f;
        [SerializeField] private float crouchCameraY = 0.95f;
        [SerializeField] private float crouchTransitionSpeed = 10f;

        private float speedCurrent;
        private float verticalVelocity;
        private float targetHeight;
        private float targetCameraY;
        private CharacterController characterController;
        private bool enabledMove = true;

        public bool Initialize(CharacterController cc)
        {
            if (cc == null)
            {
                Debug.LogError("MovementComponent::Initialize(): CharacterController is null.");
                return false;
            }
            characterController = cc;
            targetHeight = characterController.height;

            return true;
        }

        public void OnUpdate(Vector2 moveDirection, bool sprintPressed, bool crouchPressed, bool jumpPressed)
        {
            if (!enabledMove) return;
            
            HandleSpeed(sprintPressed, crouchPressed, moveDirection);
            HandleCrouch(crouchPressed);
            HandleJump(jumpPressed);
            HandleMove(moveDirection);
        }

        private void HandleCrouch(bool crouchPressed)
        {
            targetHeight = crouchPressed ? crouchHeight : standingHeight;
            targetCameraY = crouchPressed ? crouchCameraY : crouchTransitionSpeed;

            float newHeight = Mathf.Lerp(characterController.height, targetHeight,
                Time.deltaTime * crouchTransitionSpeed);
            characterController.height = newHeight;
            characterController.center = new Vector3(0f, newHeight / 2f, 0f);
        }

        private void HandleSpeed(bool sprintPressed, bool crouchPressed, Vector2 moveDirection)
        {
            speedCurrent = speedWalk;
            
            if (sprintPressed) speedCurrent = speedRun;
            if (crouchPressed) speedCurrent = speedCrouch;
            
            if (moveDirection.y < 0) speedCurrent *= backwardsMultiplier;
            else if (moveDirection.y == 0) speedCurrent *= strafeMultiplier;
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

        public float GetSpeed() => speedCurrent;
        public float GetVelocity() => characterController.velocity.magnitude;
        public float GetVerticalVelocity() => verticalVelocity;
        public bool SetEnabled(bool value) => enabledMove = value;
    }
}