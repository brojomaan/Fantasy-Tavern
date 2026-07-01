namespace NPC
{
    public class NpcState
    {
        protected NpcController controller;

        public NpcState(NpcController npcController)
        {
            controller = npcController;
        }

        public virtual void OnEnter() { }
        public virtual void OnExit() { }
        public virtual void OnUpdate() { }
        public virtual void OnLateUpdate() { }
    }
}