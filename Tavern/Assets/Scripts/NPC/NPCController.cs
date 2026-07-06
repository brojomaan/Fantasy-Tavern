using Coherence.Toolkit;
using Components.NPCComponents;
using Interactables.ItemInteractables.Mug;
using Interfaces;
using NPC.States;
using UnityEngine;

namespace NPC
{
    public class NpcController : MonoBehaviour, IInteractable
    {
        //Other
        [SerializeField] private NpcVisual visual;
        [SerializeField] private NpcUI ui;
        [SerializeField] private CoherenceSync sync;
        
        //Components
        [SerializeField] private NpcBrain brain;
        [SerializeField] private NpcMovement npcMovement;
        [SerializeField] private CharacterController characterController;
        [SerializeField] private NpcNeedsComponent needsComponent;
        [SerializeField] private NpcOrderComponent orderComponent;
        
        //Transforms
        [SerializeField] private Transform hoverSocket;
        public Transform testSeatTransform;
        public Transform testExitTransform;
        
        [OnValueSynced(nameof(OnMoveDirectionSynced))]
        [Sync] public Vector2 syncedMoveDirection;
        
        [OnValueSynced(nameof(OnVelocitySynced))]
        [Sync] public float syncedVelocity;
        
        [OnValueSynced(nameof(OnIsGroundedSynced))]
        [Sync] public bool syncedIsGrounded;

        
        //States
        public NpcStateMachine StateMachine { get; private set; }
        public EnteringState EnteringState { get; private set; }
        public SeatedState SeatedState { get; private set; }
        public LeavingState LeavingState { get; private set; }
        public ServedState ServedState { get; private set; }

        public NpcBrain Brain => brain;
        public NpcMovement Movement => npcMovement;
        public NpcVisual Visual => visual;
        public CharacterController CharacterController => characterController;
        public NpcNeedsComponent NeedsComponent => needsComponent;
        public  NpcOrderComponent OrderComponent => orderComponent;
        
        //Logic
        public Vector2 CurrentMoveInput { get; private set; }
        private bool hasInitialized = false;
        
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
            if (needsComponent == null) Debug.LogError($"NpcController::Initialize(): needsComponent = null");
            if (orderComponent == null) Debug.LogError($"NpcController::Initialize(): orderComponent = null");
            
            
            if (!npcMovement.Initialize(characterController))
                Debug.LogError($"NpcController::Initialize(): NpcMovement failed.");
            if (!visual.Initialize())
                Debug.LogError($"NpcController::Initialize(): visual failed");
            if (!brain.Initialize())
                Debug.LogError($"NpcController::Initialize(): brain failed");
            if (!needsComponent.Initialize())
                Debug.LogError($"NpcController::Initialize(): needsComponent failed");
            if (!orderComponent.Initialize())
                Debug.LogError($"NpcController::Initialize(): orderComponent failed");
            

            StateMachine = new NpcStateMachine();
            EnteringState = new EnteringState(this);
            SeatedState = new SeatedState(this);
            LeavingState = new LeavingState(this);
            ServedState = new ServedState(this);
            
            
            StateMachine.ChangeState(EnteringState);
    
            hasInitialized = true;
        }

        private void Update()
        {
            if (!hasInitialized) return;
            if (!sync.HasStateAuthority) return;

            StateMachine.OnUpdate();

            syncedMoveDirection = CurrentMoveInput;
            syncedVelocity = npcMovement.GetCurrentSpeed();
            syncedIsGrounded = characterController.isGrounded;


        }
        
        public void OnHoverEnter()
        {
            Debug.Log($"HoverEntered");
            orderComponent.ShowBubble();
        }

        public void OnHoverExit()
        {
            
        }
        public void OnInteract()
        {
            
            
        }

        public void OnInteractRelease()
        {
            
        }

        public bool TryDeliverOrder(MugController mug)
        {
            if (mug == null)
            {
                Debug.Log($"No Mug");
                return false;
            }
            if (!orderComponent.TryFulfillOrder(
                    mug.GetLiquidMixer(),
                    mug.FillLevel,
                    mug.TargetFillLevel,
                    mug.AcceptableRange)) return false;
            
            
            StateMachine.ChangeState(ServedState);
            return true;
        }
        
        public bool CanInteractWith(IHoldable heldItem) => heldItem != null;

        public void OnMoveDirectionSynced(Vector2 previous, Vector2 current)
        {
            visual.AnimationComponent.SetWalking(current);
        }

        public void OnVelocitySynced(float previous, float current)
        {
            visual.AnimationComponent.SetSpeed(current);
        }

        public void OnIsGroundedSynced(bool previous, bool current)
        {
            visual.AnimationComponent.SetGrounded(current);
        }

        public void SetMoveInput(Vector2 input)
        {
            CurrentMoveInput = input;
        }
    }
}