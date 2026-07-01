using UnityEngine;

namespace NPC.States
{
    public class SeatedState : NpcState
    {
        public SeatedState(NpcController npcController) : base(npcController) { }

        public override void OnEnter()
        {
            Debug.Log($"Npc Enter State: SeatedState");
        }
    }
}