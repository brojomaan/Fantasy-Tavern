namespace GameManagement
{
    public class GameStateMachine
    {
        private GameState currentState;

        public void ChangeState(GameState newState)
        {
            currentState?.OnExit();
            currentState = newState;
            currentState?.OnEnter();
        }

        public void OnUpdate() => currentState?.OnUpdate();
        public GameState GetCurrentState() => currentState;
    }
}