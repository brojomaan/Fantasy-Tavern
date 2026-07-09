using Coherence;
using Coherence.Toolkit;
using Components.NPCComponents;
using Interactables.ItemInteractables.Mug;
using Interactables.WorldInteractable;
using Interfaces;
using Liquids;
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
        public CoherenceSync CoherenceSync => sync;
        
        [OnValueSynced(nameof(OnMoveDirectionSynced))]
        [Sync] public Vector2 syncedMoveDirection;
        
        [OnValueSynced(nameof(OnVelocitySynced))]
        [Sync] public float syncedVelocity;
        
        [OnValueSynced(nameof(OnIsGroundedSynced))]
        [Sync] public bool syncedIsGrounded;

        [OnValueSynced(nameof(OnPatienceSynced))] 
        [Sync] public float patience;

        [OnValueSynced(nameof(OnBubbleActiveSynced))] 
        [Sync] public bool syncedBubbleActive;

        [OnValueSynced(nameof(OnOrderTypeSynced))] 
        [Sync] public string syncedOrder;
        

        
        //States
        public NpcStateMachine StateMachine { get; private set; }
        public EnteringState EnteringState { get; private set; }
        public SeatedState SeatedState { get; private set; }
        public LeavingState LeavingState { get; private set; }
        public ServedState ServedState { get; private set; }
        
        public SeatController ClaimedSeat { get; private set; }

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


        
        public void Initialize(Transform exitTransform)
        {
            if (npcMovement == null) Debug.LogError($"NpcController::Initialize(): NpcMovement is null");
            if (visual == null) Debug.LogError($"NpcController::Initialize(): visual = null");
            if (brain == null) Debug.LogError($"NpcController::Initialize(): brain = null");
            if (needsComponent == null) Debug.LogError($"NpcController::Initialize(): needsComponent = null");
            if (orderComponent == null) Debug.LogError($"NpcController::Initialize(): orderComponent = null");

            DrinkRecipe recipe = DrinkRecipeBook.Instance.GetRandomRecipe();
            if (recipe == null) Debug.LogError($"NpcController::Initialize(): recipe = null");
            
            if (!npcMovement.Initialize(characterController))
                Debug.LogError($"NpcController::Initialize(): NpcMovement failed.");
            if (!visual.Initialize())
                Debug.LogError($"NpcController::Initialize(): visual failed");
            if (!brain.Initialize())
                Debug.LogError($"NpcController::Initialize(): brain failed");
            if (!needsComponent.Initialize())
                Debug.LogError($"NpcController::Initialize(): needsComponent failed");
            if (!orderComponent.Initialize(recipe))
                Debug.LogError($"NpcController::Initialize(): orderComponent failed");
            
            testExitTransform = exitTransform;
            
            StateMachine = new NpcStateMachine();
            EnteringState = new EnteringState(this);
            SeatedState = new SeatedState(this);
            LeavingState = new LeavingState(this);
            ServedState = new ServedState(this);
            
            StateMachine.ChangeState(EnteringState);
            
            if (sync.HasStateAuthority)
            {
                syncedBubbleActive = false;
                syncedOrder = orderComponent.GetOrderName();
            }
    
            hasInitialized = true;
        }

        private void Update()
        {
            if (!hasInitialized) return;
            if (!sync.HasStateAuthority) return;

            StateMachine.OnUpdate();
            orderComponent.OnUpdate();
            
            syncedMoveDirection = CurrentMoveInput;
            syncedVelocity = npcMovement.GetCurrentSpeed();
            syncedIsGrounded = true;
            patience = needsComponent.GetPatienceNormalized();
            syncedBubbleActive = orderComponent.IsBubbleVisible();
            syncedOrder = orderComponent.GetOrderName();
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
        
        public bool CanInteractWith(IHoldable heldItem) => heldItem as MugController;

        public void OnMoveDirectionSynced(Vector2 previous, Vector2 current)
        {
            visual.AnimationComponent.SetWalking(current);
            Debug.Log($"MoveDirection Synced: {current}");
        }

        public void OnVelocitySynced(float previous, float current)
        {
            visual.AnimationComponent.SetSpeed(current);
        }

        public void OnIsGroundedSynced(bool previous, bool current)
        {
            visual.AnimationComponent.SetGrounded(current);
            Debug.Log($"IsGroundedSynced: {current}");
        }

        public void SetMoveInput(Vector2 input)
        {
            CurrentMoveInput = input;
        }

        [Command(defaultRouting = MessageTarget.AuthorityOnly)]
        public void CmdDeliverOrder(string liquidContents, float fillLevel, 
            float targetFill, float acceptableRange)
        {
            Debug.Log("CmdDeliverOrder received on authority");
            LiquidMixer deliveredMixer = new LiquidMixer();
            deliveredMixer.Deserialize(liquidContents, LiquidRegistry.Instance);

            if (!orderComponent.TryFulfillOrder(deliveredMixer, fillLevel, targetFill, acceptableRange))
            {
                Debug.Log($"OrderRejected: {liquidContents}");
                return;
            }

            StateMachine.ChangeState(ServedState);
        }

        public void OnPatienceSynced(float previous, float current)
        {
            Visual.FaceAnimationComponent.SetEmotion(current);
            Visual.FaceAnimationComponent.SetBlink();
        }

        public void OnBubbleActiveSynced(bool previous, bool current)
        {
            Debug.Log($"bubbleActiveSynced: {current}");
            if (current) orderComponent.ShowBubble();
            else orderComponent.HideBubble();
        }

        public void OnOrderTypeSynced(string previous, string current)
        {
            orderComponent.SetOrder(current);
        }

        public void SetClaimedSeat(SeatController seat)
        {
            ClaimedSeat = seat;
            Debug.Log($"ClaimedSeat = {seat}");
        }
    }
}