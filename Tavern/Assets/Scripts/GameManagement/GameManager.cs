using Coherence.Toolkit;
using GameManagement.States;
using NPC;
using UnityEngine;

namespace GameManagement
{
    public enum GameStateType
    {
        Lobby,
        Playing
    }
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] private CoherenceSync sync;
        [SerializeField] private NpcSpawner npcSpawner;

        private GameStateMachine stateMachine = new GameStateMachine();
        
        public LobbyState LobbyState { get; private set; }
        public PlayingState PlayingState { get; private set; }

        public NpcSpawner NpcSpawner => npcSpawner;
        public CoherenceSync CoherenceSync => sync;

        [OnValueSynced(nameof(OnGameStateSynced))] 
        [Sync] public GameStateType currentGameState;

        private bool hasInitialized = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
        private void Start()
        {
            if (sync == null) { Debug.LogError("GameManager::Initialize(): sync is null."); return; }
            if (npcSpawner == null) { Debug.LogError("GameManager::Initialize(): npcSpawner is null."); return; }

            LobbyState = new LobbyState(this);
            PlayingState = new PlayingState(this);

            stateMachine.ChangeState(LobbyState);
            hasInitialized = true;
        }

        private void Update()
        {
            if (!hasInitialized) return;
            if (!sync.HasStateAuthority) return;

            stateMachine.OnUpdate();
        }

        public void CmdRequestStartGame()
        {
            Debug.Log($"CmdRequestStartGame called - HasStateAuthority: {sync.HasStateAuthority}");
            if (currentGameState == GameStateType.Playing) return;
            
            Debug.Log($"CmdGettingThisFar");
            stateMachine.ChangeState(PlayingState);
            currentGameState = GameStateType.Playing;
        }

        

        public void OnGameStateSynced(GameStateType previous, GameStateType current)
        {
            Debug.Log($"GameState changed to : {current}");
        }
    }
}