using UnityEngine;

namespace Player.States
{
    public class FreeState : PlayerState
    {
        public FreeState(PlayerController controller) : base(controller) { }

        public override void OnEnter()
        {
            Debug.Log($"Entered FreeState");
            controller.MovementComponent.SetEnabled(true);
            controller.LookComponent.SetEnabled(true);
        }

        public override void OnUpdate()
        {
            controller.MovementComponent.OnUpdate(
                controller.Input.GetMoveDirection(),
                controller.Input.GetSprintPressed(),
                controller.Input.GetCrouchPressed(),
                controller.Input.GetJumpPressed());

            controller.LookComponent.OnUpdate(controller.Input.GetLookDirection());
            controller.InteractComponent.OnUpdate(controller.Input.GetPickupPressed());

            if (controller.Input.GetPickupPressed() && controller.InteractComponent.HasHover())
                controller.StateMachine.ChangeState(controller.HoldingState);

        }

        public override void OnLateUpdate()
        {
            controller.headPitch = Mathf.Clamp(controller.LookComponent.GetPitch(), -40f, 35f);
            controller.CameraController.OnLateUpdate(
                controller.Visual.GetHeadBone(),
                controller.MovementComponent.GetVelocity(),
                controller.Input.GetMoveDirection().x,
                controller.MovementComponent.GetVerticalVelocity(),
                controller.Input.GetSprintPressed(),
                controller.CharacterController.isGrounded);
        }
    }
}