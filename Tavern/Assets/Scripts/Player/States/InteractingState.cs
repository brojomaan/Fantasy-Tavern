using Interactables.WorldInteractable;

namespace Player.States
{
    public class InteractingState : PlayerState
    {
        private WorldInteractable interactable;
        
        public InteractingState(PlayerController playerController) : base(playerController) { }

        public void SetInteractable(WorldInteractable target)
        {
            interactable = target;
        }

        public override void OnEnter()
        {
            controller.MovementComponent.SetEnabled(false);
            controller.LookComponent.SetEnabled(false);
            controller.CameraController.SetInteracting(true);
            interactable?.OnInteract();
        }

        public override void OnExit()
        {
            interactable?.OnInteractRelease();
            interactable = null;
            controller.MovementComponent.SetEnabled(true);
            controller.LookComponent.SetEnabled(true);
            controller.CameraController.SetInteracting(false);
        }

        public override void OnUpdate()
        {
            interactable?.OnInteractUpdate(controller.Input.GetLookDirection());
            
            if (!controller.Input.GetInteractHeld())
                controller.StateMachine.ChangeState(controller.FreeState);
        }

        public override void OnLateUpdate()
        {
            controller.CameraController.OnLateUpdate(
                controller.Visual.GetHeadBone(),
                0f,
                0f,
                0f,
                false,
                true,
                controller.Input.GetLookDirection().y);
        }
    }
}