using UnityEngine;

namespace NPC.States
{
    public class ServedState : NpcState
    {
        public ServedState(NpcController npcController) : base(npcController) { }

        public override void OnEnter()
        {
            Debug.Log($"Entered ServedState");
            controller.SetMoveInput(Vector2.zero);
            controller.StateMachine.ChangeState(controller.LeavingState);
        }
    }
}