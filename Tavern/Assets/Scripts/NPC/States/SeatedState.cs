using UnityEngine;

namespace NPC.States
{
    public class SeatedState : NpcState
    {
        public SeatedState(NpcController npcController) : base(npcController) { }

        public override void OnEnter()
        {
            Debug.Log($"Npc Enter State: SeatedState");
            controller.SetMoveInput(Vector2.zero);
            controller.NeedsComponent.StartDecay();
            controller.Visual.OnUpdate(Vector2.zero, 0f, true);
            controller.OrderComponent.ShowBubble();
        }

        public override void OnUpdate()
        {
            controller.NeedsComponent.OnUpdate();
            


            if (!controller.NeedsComponent.HasPatience())
            {
                controller.StateMachine.ChangeState(controller.LeavingState);
            }
        }

        public override void OnExit()
        {
            Debug.Log($"Npc Exit State: SeatedState");
            controller.NeedsComponent.StopDecay();
        }
    }
}