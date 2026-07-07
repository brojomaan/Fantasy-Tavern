using UnityEngine;

namespace NPC.States
{
    public class LeavingState : NpcState
    {
        public LeavingState(NpcController npcController) : base(npcController) { }

        public override void OnEnter()
        {
            Debug.Log($"Npc Enter State: LeavingState");
        }

        public override void OnUpdate()
        {
            bool arrived = controller.Movement.MoveTowards(controller.testExitTransform.position);

            Vector2 moveInput = arrived ? Vector2.zero : new Vector2(0f, 1f);
            controller.SetMoveInput(moveInput);
            
            controller.Visual.OnUpdate(moveInput,
                controller.Movement.GetCurrentSpeed(),
                true);
            
            controller.Visual.FaceAnimationComponent.SetEmotion(controller.NeedsComponent.GetPatienceNormalized());
            controller.Visual.FaceAnimationComponent.SetBlink();
            
            if (arrived)
                Object.Destroy(controller.gameObject);
        }

        public override void OnExit()
        {
            Debug.Log($"Npc Exit State: LeavingState");
        }
    }
}