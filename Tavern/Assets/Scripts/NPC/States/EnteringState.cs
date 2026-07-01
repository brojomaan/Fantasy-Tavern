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

            bool arrived = controller.Movement.MoveTowards(targetSeat.position);

            Vector2 moveInput = arrived ? Vector2.zero : new Vector2(0f, 1f);

            controller.Visual.OnUpdate(moveInput,
                controller.Movement.GetCurrentSpeed(),
                controller.CharacterController.isGrounded);

            if (arrived)
                controller.StateMachine.ChangeState(controller.SeatedState);
        }
    }
}