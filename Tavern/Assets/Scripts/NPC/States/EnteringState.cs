using UnityEngine;

namespace NPC.States
{
    public class EnteringState : NpcState
    {
        private Transform targetSeat;
        public EnteringState(NpcController npcController) : base(npcController) { }

        public override void OnEnter()
        {
            Debug.Log($"Npc Enter State: EnteringState");
            targetSeat = controller.Brain.FindAvailableSeat();

            if (targetSeat == null)
                Debug.LogError($"Entering State:: On Enter(): No Available Seat  Found");
        }

        public override void OnExit()
        {
            Debug.Log($"Npc Exit State: Entering State");
        }

        public override void OnUpdate()
        {
            if (targetSeat == null) return;

            bool arrived = controller.Movement.MoveTowards(controller.testSeatTransform.position);
            Debug.Log($"Arrived: {arrived}");
            
            Vector2 moveInput = arrived ? Vector2.zero : new Vector2(0f, 1f);
            controller.SetMoveInput(moveInput);

            controller.Visual.OnUpdate(moveInput,
                controller.Movement.GetCurrentSpeed(),
                controller.CharacterController.isGrounded);
            
            controller.Visual.FaceAnimationComponent.SetEmotion(0f);
            controller.Visual.FaceAnimationComponent.SetBlink();

            if (arrived)
                controller.StateMachine.ChangeState(controller.SeatedState);
        }
    }
}