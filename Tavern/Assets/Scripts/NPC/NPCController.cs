using Coherence.Toolkit;
using Components.NPCComponents;
using Interfaces;
using NPC.States;
using UnityEngine;

namespace NPC
{
    public class NpcController : MonoBehaviour, IInteractable
    {
        [SerializeField] private NpcBrain brain;
        [SerializeField] private NpcMovement npcMovement;
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Transform testTransform;
        
        //Transforms
        [SerializeField] private Transform hoverSocket;

        //Other
        [SerializeField] private NpcVisual visual;
        [SerializeField] private NpcUI ui;
        [SerializeField] private CoherenceSync sync;
        
        //States
        public NpcStateMachine StateMachine { get; private set; }
        public EnteringState EnteringState { get; private set; }
        public SeatedState SeatedState { get; private set; }

        public NpcBrain Brain => brain;
        public NpcMovement Movement => npcMovement;
        public NpcVisual Visual => visual;
        public CharacterController CharacterController => characterController;


        
        //Logic
        private bool hasInitialized = false;
        private bool orderFulfilled = false;
        
        public Transform GetHoverSocket() => hoverSocket;

        public Transform GetGripSocket() => hoverSocket;


        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            if (npcMovement == null) Debug.LogError($"NpcController::Initialize(): NpcMovement is null");
            if (visual == null) Debug.LogError($"NpcController::Initialize(): visual = null");
            if (brain == null) Debug.LogError($"NpcController::Initialize(): brain = null");
            
            if (!npcMovement.Initialize(characterController))
                Debug.LogError($"NpcController::Initialize(): NpcMovement failed.");
            if (!visual.Initialize())
                Debug.LogError($"NpcController::Initialize(): visual failed");
            if (!brain.Initialize())
                Debug.LogError($"NpcController::Initialize(): brain failed");

            StateMachine = new NpcStateMachine();
            EnteringState = new EnteringState(this);

            hasInitialized = true;
        }

        private void Update()
        {
            if (!hasInitialized) return;
            if (orderFulfilled) return;

            bool arrived = npcMovement.MoveTowards(testTransform.position);

            Vector3 toTarget = (testTransform.position - transform.position).normalized;
            Vector2 moveInput = arrived ? Vector2.zero : new Vector2(0f, 1f);
            
            visual.OnUpdate(moveInput, 
                npcMovement.GetCurrentSpeed(),
                characterController.isGrounded);
            

        }

        
        public void OnHoverEnter()
        {
            
        }

        public void OnHoverExit()
        {
            
        }

        public bool CanInteractWith(IHoldable heldItem)
        {
            return false;
        }

        public void OnInteract()
        {
            
        }

        public void OnInteractRelease()
        {
            
        }
    }
}