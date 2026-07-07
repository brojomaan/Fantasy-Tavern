namespace GameManagement
{
    public abstract class GameState
    {
        protected GameManager manager;

        public GameState(GameManager manager)
        {
            this.manager = manager;
        }

        public virtual void OnEnter() { }
        public virtual void OnExit() { }
        public virtual void OnUpdate() { }
        
    }
}