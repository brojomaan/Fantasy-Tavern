using UnityEngine;

namespace Components.NPCComponents
{
    public class NpcMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private float rotationSpeed = 8f;
        [SerializeField] private float arrivalDistance = 0.2f;

        private CharacterController characterController;
        
        public bool Initialize(CharacterController cc)
        {
            if (cc == null) { Debug.LogError("NpcMovement::Initialize(): CharacterController is null."); return false; }
            characterController = cc;
            return true;
        }

        public bool MoveTowards(Vector3 targetPosition)
        {
            Vector3 toTarget = targetPosition - transform.position;
            toTarget.y = 0f;

            if (toTarget.magnitude <= arrivalDistance)
                return true; // arrived
            
            Vector3 direction = toTarget.normalized;
            //rotate towards
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            Vector3 movement = direction * moveSpeed * Time.deltaTime;
            characterController.Move(movement);

            return false; // still Moving
        }
        
        public float GetCurrentSpeed() => characterController.velocity.magnitude;
    }
}