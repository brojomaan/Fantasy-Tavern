using Coherence;
using Interactables.ItemInteractables.Mug;
using NPC;
using UnityEditor;
using UnityEngine;

namespace Player.States
{
    public class HoldingState : PlayerState
    {
        public HoldingState(PlayerController controller) : base(controller) { }

        public override void OnEnter()
        {
            Debug.Log($"Entered HoldingState");
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
            controller.InteractComponent.OnUpdate();
            controller.HoldComponent.OnUpdate();

            if (controller.Input.GetDropPressed())
            {
                if (controller.InteractComponent.IsPreviewingPlacement)
                {
                    controller.HoldComponent.Place();
                }
                else
                {
                    controller.HoldComponent.Drop();
                }
                
                controller.StateMachine.ChangeState(controller.FreeState);
            }

            if (controller.Input.GetInteractPressed() && controller.HoldComponent.IsHolding())
            {
                NpcController npc = controller.InteractComponent.GetCurrentNpcController();
                if (npc == null)
                    Debug.LogError($"Cant find Npc");
                MugController mug = controller.HoldComponent.GetHeldItem() as MugController;

                if (npc != null && mug != null)
                {
                    npc.CoherenceSync.SendCommand<NpcController>(
                        nameof(NpcController.CmdDeliverOrder),
                        MessageTarget.StateAuthorityOnly,
                        mug.GetLiquidMixer().Serialize(),
                        mug.FillLevel,
                        mug.TargetFillLevel,
                        mug.AcceptableRange);

                    controller.HoldComponent.Drop();
                    GameObject.Destroy((mug).gameObject);

                    controller.StateMachine.ChangeState(controller.FreeState);

                }
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
                controller.CharacterController.isGrounded,
                controller.Input.GetLookDirection().y);
        }
    }
}