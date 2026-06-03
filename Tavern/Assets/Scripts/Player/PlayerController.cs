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
        
        //Visual
        [SerializeField] private PlayerVisual visual;
        
        
        //UI
        [SerializeField] private PlayerUI playerUI;

        //THINGS THAT NEED DOING PROPERLY
        [SerializeField] private Transform cameraTransform;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (input == null) Debug.LogError("PlayerController::Initialize(): input is null.");
            if (visual == null) Debug.LogError("PlayerController::Initialize(): visual is null.");
            if (playerUI == null) Debug.LogError("PlayerController::Initialize(): playerUI is null.");
            if (movementComponent == null) Debug.LogError("PlayerController::Initialize(): movementComponent is null.");
            if (lookComponent == null) Debug.LogError("PlayerController::Initialize(): lookComponent is null.");
            
            movementComponent.Initialize(characterController);
            lookComponent.Initialize(cameraTransform);
        }

        private void Update()
        {
            input.OnUpdate();
            
            movementComponent.OnUpdate(input.GetMoveDirection(), input.GetSprintPressed(),
                input.GetCrouchPressed(), input.GetJumpPressed());
            
            lookComponent.OnUpdate(input.GetLookDirection());
            
        }
    }
}
