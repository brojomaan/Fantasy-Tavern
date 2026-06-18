using Components.CharacterComponents;
using UnityEngine;

namespace Player
{
    public class PlayerVisual : MonoBehaviour
    {
        [SerializeField] private AnimationComponent animComponent;
        [SerializeField] private IKComponent ikComponent;
        [SerializeField] private Transform headBone;
        [SerializeField] private Transform headAddons;
        [SerializeField] private SkinnedMeshRenderer thirdPerson;
        [SerializeField] private SkinnedMeshRenderer firstPerson;

        private float headPitch;

        public AnimationComponent AnimationComponent => animComponent;

        public bool Initialize(bool isPlayer) //turn this into state authority
        {
            if (animComponent == null) {Debug.Log($"Animation component is null"); return false; }
            if (ikComponent == null) {Debug.Log($"IKComponent is null"); return false; }
            if (headBone == null) {Debug.LogError($"Head Bone is null"); return false; }
            if (headAddons == null) {Debug.LogError($"Head Addons is null"); return false; }
            if (thirdPerson == null) {Debug.LogError($"Third Person is null"); return false; }
            if (firstPerson == null) {Debug.LogError($"First Person is null"); return false; }
            
            if (!animComponent.Initialize())  
            {
                Debug.LogError($"Animation component Initialize() failed.");
                return false;
            }

            if (!ikComponent.Initialize())
            {
                Debug.LogError($"Animation component Initialize() failed.");
                return false;
            }

            if (isPlayer)
            {
                thirdPerson.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                headAddons.gameObject.SetActive(false);
            }
            else
            {
                firstPerson.gameObject.SetActive(false);
                headAddons.gameObject.SetActive(true);
            }

            return true;
        }

        public void SetHeadPitch(float pitch)
        {
            headPitch = pitch;
        }
        public void OnUpdate(Vector2 input, float speed, bool crouching, float verticalVelocity, bool isGrounded, bool isAuthority)
        {
            animComponent.SetWalking(input);
            animComponent.SetCrouching(crouching);
            animComponent.SetSpeed(speed);
            animComponent.SetVerticalVelocity(verticalVelocity);
            animComponent.SetGrounded(isGrounded);
            
            ikComponent.OnUpdate(isAuthority);
        }

        public void SetIKTarget(Transform target)
        {
            ikComponent.SetIKTarget(target);
        }

        public void SetIKTargetPosition(Vector3 position)
        {
            ikComponent.SetTargetPosition(position);
        }

        public void SetIKTargetRotation(Quaternion rotation)
        {
            ikComponent.SetTargetRotation(rotation);
        }

        public void ClearIKTarget()
        {
            ikComponent.ClearIKTarget();
        }
        public void OnLateUpdate()
        {
            headBone.localRotation = Quaternion.Euler(headPitch, 0f, 0f);
        }
        
        public Transform GetHeadBone() => headBone;

        public void SetIKWeight(float weight)
        {
            ikComponent.SetWeight(weight);
        }

        public void UpdateIK(bool isAuthority)
        {
            ikComponent.OnUpdate(isAuthority);
        }

        public float GetIKWeight() => ikComponent.GetWeight();
        public Vector3 GetIKTargetPosition() => ikComponent.GetIKTargetPosition();
        public Quaternion GetIKTargetRotation() => ikComponent.GetIKTargetRotation();
    }
}
