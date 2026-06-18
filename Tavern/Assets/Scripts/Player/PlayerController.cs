using Coherence;
using Coherence.Toolkit;
using Components.CharacterComponents;
using Player.States;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        //Data
        
        
        //Components
        [SerializeField] private PlayerInput input;
        [SerializeField] private CharacterController characterController;
        [SerializeField] private MovementComponent movementComponent;
        [SerializeField] private LookComponent lookComponent;
        [SerializeField] private InteractComponent interactComponent;
        [SerializeField] private HoldComponent holdComponent;
        
        //
        [SerializeField] private CoherenceSync sync;
        [SerializeField] private PlayerVisual visual;
        [SerializeField] private PlayerUI playerUI;
        [SerializeField] private CameraController cameraController;
        
        //States
        private PlayerStateMachine stateMachine = new PlayerStateMachine();
        public FreeState FreeState { get; private set; }
        public HoldingState HoldingState { get; private set; }
        
        
        //Synced Variables
        [OnValueSynced(nameof(OnVelocitySynced))]
        [Sync] public float velocity;

        [OnValueSynced(nameof(OnMoveDirectionSynced))] 
        [Sync]
        public Vector2 moveDirection;

        [OnValueSynced(nameof(OnHeadPitchSynced))] 
        [Sync] public float headPitch;

        [OnValueSynced(nameof(OnIsGroundedSynced))] 
        [Sync] public bool isGrounded;
        
        [OnValueSynced(nameof(OnVerticalVelocitySynced))]
        [Sync] public float verticalVelocity;

        public void OnHeadPitchSynced(float previous, float current)
        {
            visual.SetHeadPitch(current);
        }
        
        public PlayerInput Input => input;
        public CharacterController CharacterController => characterController;
        public MovementComponent MovementComponent => movementComponent;
        public LookComponent LookComponent => lookComponent;
        public CameraController CameraController => cameraController;
        public HoldComponent HoldComponent => holdComponent;
        public InteractComponent InteractComponent => interactComponent;
        public PlayerVisual Visual => visual;
        public PlayerStateMachine StateMachine => stateMachine;
        

        private bool hasInitialized = false;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (input == null) Debug.LogError("PlayerController::Initialize(): input is null.");
            if (playerUI == null) Debug.LogError("PlayerController::Initialize(): playerUI is null.");
            if (MovementComponent == null) Debug.LogError("PlayerController::Initialize(): movementComponent is null.");
            if (LookComponent == null) Debug.LogError("PlayerController::Initialize(): lookComponent is null.");
            if (CameraController == null) Debug.LogError("PlayerController::Initialize(): cameraController is null.");
            if (sync == null) Debug.LogError("PlayerController::Initialize(): cameraController is null.");
            if (visual == null) Debug.LogError("PlayerController::Initialize(): visual = null.");
            if (interactComponent == null) Debug.LogError("PlayerController::Initialize(): interactComponent is null.");
            if (holdComponent == null) Debug.LogError("PlayerController::Initialize(): holdComponent is null.");
            
            if (sync.HasStateAuthority)
            {
                if (!CameraController.Initialize()) 
                    Debug.LogError("PlayerController::Initialize(): cameraController Initialization Failed.");
                if (!movementComponent.Initialize(characterController)) 
                    Debug.LogError("PlayerController::Initialize(): movementComponent Initialization Failed.");
                if (!lookComponent.Initialize(CameraController.GetCamControllerRoot())) 
                    Debug.LogError("PlayerController::Initialize(): lookComponent Initialization Failed.");
                if (!holdComponent.Initialize())
                    Debug.LogError("PlayerController::Initialize(): holdComponent Initialization Failed.");
                if (!interactComponent.Initialize(visual, holdComponent))
                    Debug.LogError("PlayerController::Initialize(): interactComponent Initialization Failed.");
            }
            else
            {
                cameraController.gameObject.SetActive(false);
            }

            
            
            if (!visual.Initialize(sync.HasStateAuthority))
                Debug.LogError("PlayerController::Initialize(): visual Initialization Failed.");

            FreeState = new FreeState(this);
            HoldingState = new HoldingState(this);
            
            stateMachine.ChangeState(FreeState);

            hasInitialized = true;
        }

        private void Update()
        {
            if (!hasInitialized) return;
            if (!sync.HasStateAuthority) return;
            
            input.OnUpdate();
            stateMachine.OnUpdate();
            
            moveDirection = input.GetMoveDirection();
            velocity = movementComponent.GetVelocity();
            isGrounded = characterController.isGrounded;
            verticalVelocity = movementComponent.GetVerticalVelocity();
            
            visual.OnUpdate(input.GetMoveDirection(), 
                velocity, 
                input.GetCrouchPressed(),
                movementComponent.GetVerticalVelocity(),
                characterController.isGrounded);  
        }

        private void LateUpdate()
        {
            if (!hasInitialized) return;
            if (!sync.HasStateAuthority) return;
            
            stateMachine.OnLateUpdate();
            visual.OnLateUpdate();
        }

        public void OnVelocitySynced(float previous, float current)
        {
            visual.AnimationComponent.SetSpeed(current);
        }

        public void OnMoveDirectionSynced(Vector2 previous, Vector2 current)
        {
            visual.AnimationComponent.SetWalking(current);
        }

        public void OnIsGroundedSynced(bool previous, bool current)
        {
            visual.AnimationComponent.SetGrounded(current);
        }

        public void OnVerticalVelocitySynced(float previous, float current)
        {
            visual.AnimationComponent.SetVerticalVelocity(current);
        }
        
    }
}
