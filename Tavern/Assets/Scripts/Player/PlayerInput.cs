using UnityEngine;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        private InputActions actions;

        private Vector2 moveDirection;
        private Vector2 lookDirection;
        private bool interactPressed;
        private bool pickupPressed;
        private bool dropPressed; // This is also throwing
        private bool crouchPressed;
        private bool jumpPressed;
        private bool sprintPressed;

        private void Awake()
        {
            actions = new InputActions();
        }

        private void OnEnable()
        {
            actions.Enable();
        }

        private void OnDisable()
        {
            actions.Disable();
        }
        
        public void OnUpdate()
        {
            moveDirection = actions.Player.Move.ReadValue<Vector2>();
            lookDirection = actions.Player.Look.ReadValue<Vector2>();
            interactPressed = actions.Player.Interact.WasPressedThisFrame();
            pickupPressed = actions.Player.Pickup.WasPressedThisFrame();
            dropPressed = actions.Player.Drop.IsPressed();
            crouchPressed = actions.Player.Crouch.IsPressed();
            jumpPressed = actions.Player.Jump.IsPressed();
            sprintPressed = actions.Player.Sprint.IsPressed();
        }

        public Vector2 GetMoveDirection() => moveDirection;
        public Vector2 GetLookDirection() => lookDirection;
        public bool GetInteractPressed() => interactPressed;
        public bool GetPickupPressed() => pickupPressed;
        public bool GetDropPressed() => dropPressed;
        public bool GetCrouchPressed() => crouchPressed;
        public bool GetJumpPressed() => jumpPressed;
        public bool GetSprintPressed() => sprintPressed;
        

    }
}
