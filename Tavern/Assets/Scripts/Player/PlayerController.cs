using Coherence;
using Coherence.Toolkit;
using Components.CharacterComponents;
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
        
        
        [SerializeField] private CoherenceSync sync;
        [SerializeField] private PlayerVisual visual;
        [SerializeField] private PlayerUI playerUI;
        [SerializeField] private CameraController cameraController;
        
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
        public MovementComponent MovementComponent => movementComponent;
        public LookComponent LookComponent => lookComponent;
        public CameraController CameraController => cameraController;

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

            if (sync.HasStateAuthority)
            {
                if (!CameraController.Initialize()) 
                    Debug.LogError("PlayerController::Initialize(): cameraController Initialization Failed.");
                if (!movementComponent.Initialize(characterController)) 
                    Debug.LogError("PlayerController::Initialize(): movementComponent Initialization Failed.");
                if (!lookComponent.Initialize(CameraController.GetCamControllerRoot())) 
                    Debug.LogError("PlayerController::Initialize(): lookComponent Initialization Failed.");
            }
            else
            {
                cameraController.gameObject.SetActive(false);
            }

            
            
            if (!visual.Initialize(sync.HasStateAuthority))
                Debug.LogError("PlayerController::Initialize(): visual Initialization Failed.");

            hasInitialized = true;
        }

        private void Update()
        {
            if (!hasInitialized) return;
            if (!sync.HasStateAuthority) return;
            
            input.OnUpdate();
            
            movementComponent.OnUpdate(
                input.GetMoveDirection(), 
                input.GetSprintPressed(),
                input.GetCrouchPressed(), 
                input.GetJumpPressed());
            
            lookComponent.OnUpdate(input.GetLookDirection());

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

            if (sync.HasStateAuthority)
            {
                headPitch = Mathf.Clamp(lookComponent.GetPitch(), -40f, 35f);
                cameraController.OnLateUpdate(visual.GetHeadBone(),
                    movementComponent.GetVelocity(),
                    input.GetMoveDirection().x,
                    movementComponent.GetVerticalVelocity(),
                    input.GetSprintPressed(),
                    characterController.isGrounded);
            }
            
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
