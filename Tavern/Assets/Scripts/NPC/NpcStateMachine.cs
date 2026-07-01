namespace NPC
{
    public class NpcStateMachine
    {
        private NpcState currentState;

        public void ChangeState(NpcState newState)
        {
            currentState?.OnExit();
            currentState = newState;
            currentState?.OnEnter();
        }

        public void OnUpdate() => currentState?.OnUpdate();
        public void OnLateUpdate() => currentState?.OnLateUpdate();
        public NpcState GetCurrentState() => currentState;

    }
}