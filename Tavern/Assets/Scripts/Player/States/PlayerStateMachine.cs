namespace Player.States
{
    public class PlayerStateMachine
    {
        private PlayerState currentState;

        public void ChangeState(PlayerState newState)
        {
            currentState?.OnExit();
            currentState = newState;
            currentState?.OnEnter();
        }

        public void OnUpdate() => currentState?.OnUpdate();
        public void OnLateUpdate() => currentState?.OnLateUpdate();
        public PlayerState GetCurrentState() => currentState;
        
    }
}