using UnityEngine;

namespace GameManagement.States
{
    public class LobbyState : GameState
    {
        public LobbyState(GameManager manager) : base(manager) { }
        
        public override void OnEnter()
        {
            Debug.Log($"GameManager: Entered LobbyState");
            manager.NpcSpawner.SetActive(false);
        }

        public override void OnExit()
        {
            Debug.Log($"GameManager: Exiting LobbyState");
        }
    }
}