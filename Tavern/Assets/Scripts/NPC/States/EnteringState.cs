using GameManagement;
using Interactables.WorldInteractable;
using UnityEngine;

namespace NPC.States
{
    public class EnteringState : NpcState
    {

        public EnteringState(NpcController npcController) : base(npcController) { }

        public override void OnEnter()
        {
            Debug.Log($"Npc Enter State: EnteringState");

            SeatController seat = SeatingManager.Instance.FindAvailableSeat();

            if (seat == null || !seat.TryClaim())
            {
                Debug.Log($"EnteringState: No seat available, leaving.");
                controller.StateMachine.ChangeState(controller.LeavingState);
                return;
            }
            
            controller.SetClaimedSeat(seat);
            
            controller.OrderComponent.HideBubble();
        }

        public override void OnExit()
        {
            Debug.Log($"Npc Exit State: Entering State");

        }

        public override void OnUpdate()
        {
            if (controller.ClaimedSeat == null) return;

            bool arrived = controller.Movement.MoveTowards(controller.ClaimedSeat.transform.position);
            
            Vector2 moveInput = arrived ? Vector2.zero : new Vector2(0f, 1f);
            controller.SetMoveInput(moveInput);



            controller.Visual.OnUpdate(moveInput,
                controller.Movement.GetCurrentSpeed(),
                controller.CharacterController.isGrounded);
            
            controller.Visual.FaceAnimationComponent.SetEmotion(1f);
            controller.Visual.FaceAnimationComponent.SetBlink();

            if (arrived)
                controller.StateMachine.ChangeState(controller.SeatedState);
        }
    }
}