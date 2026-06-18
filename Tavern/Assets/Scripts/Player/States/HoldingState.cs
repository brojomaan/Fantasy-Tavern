using UnityEngine;

namespace Player.States
{
    public class HoldingState : PlayerState
    {
        public HoldingState(PlayerController controller) : base(controller) { }

        public override void OnEnter()
        {
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
            controller.InteractComponent.OnUpdate(false);
            controller.HoldComponent.OnUpdate();

            if (controller.Input.GetDropPressed())
            {
                controller.HoldComponent.Drop();
                controller.StateMachine.ChangeState(new FreeState(controller));
            }
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