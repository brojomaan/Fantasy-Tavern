namespace Player.States
{
    public abstract class PlayerState
    {
        protected PlayerController controller;

        public PlayerState(PlayerController playerController)
        {
            controller = playerController;
        }

        public virtual void OnEnter() { }
        public virtual void OnExit() { }
        public virtual void OnUpdate() { }
        public virtual void OnLateUpdate() { }
    }
}
