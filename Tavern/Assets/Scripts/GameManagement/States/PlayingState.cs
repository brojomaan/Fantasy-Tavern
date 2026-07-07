using UnityEngine;

namespace GameManagement.States
{
    public class PlayingState : GameState
    {
        public PlayingState(GameManager manager) : base(manager) { }

        public override void OnEnter()
        {
            Debug.Log($"GameManager: Entered PlayingState");
            manager.NpcSpawner.SetActive(true);
        }

        public override void OnExit()
        {
            Debug.Log($"GameManager: Exiting PlayingState");
        }
    }
}