using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Components.CharacterComponents
{
    public class IKComponent : MonoBehaviour
    {
        [SerializeField] private Transform ikTarget;
        [SerializeField] private Transform ikHint;
        [SerializeField] private Rig rig;

        private Transform targetSocket;
        private float rigWeightTarget;
        [SerializeField] private float rigWeightSpeed = 8f;

        public bool Initialize()
        {
            if (ikTarget == null) { Debug.LogError("IKComponent::Initialize(): ikTarget is null."); return false; }
            if (ikHint == null) { Debug.LogError("IKComponent::Initialize(): ikHint is null."); return false; }
            if (rig == null) { Debug.LogError("IKComponent::Initialize(): rig is null."); return false; }

            rig.weight = 0f;
            return true;
        }

        public void SetIKTarget(Transform socket)
        {
            if (socket == null)
            {
                ClearIKTarget();
                return;
            }
            targetSocket = socket;
            rigWeightTarget = 1f;
        }

        public void ClearIKTarget()
        {
            targetSocket = null;
            rigWeightTarget = 0f;
        }

        public void SetTargetPosition(Vector3 position)
        {
            ikTarget.position = position;
        }

        public void SetTargetRotation(Quaternion rotation)
        {
            ikTarget.rotation = rotation;
        }

        public void SetWeight(float weight)
        {
            rigWeightTarget = weight;
        }
        
        public void OnUpdate(bool isAuthority)
        {
            rig.weight = Mathf.Lerp(rig.weight, rigWeightTarget, Time.deltaTime * rigWeightSpeed);

            if (isAuthority && targetSocket != null)
            {
                ikTarget.position = targetSocket.position;
                ikTarget.rotation = targetSocket.rotation;
            }
        }

        public float GetWeight() => rig.weight;

        public Vector3 GetIKTargetPosition() => ikTarget.position;
        public Quaternion GetIKTargetRotation() => ikTarget.rotation;
    }
}