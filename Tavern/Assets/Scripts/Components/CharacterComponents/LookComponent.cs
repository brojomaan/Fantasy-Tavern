using UnityEngine;

namespace Components.CharacterComponents
{
    public class LookComponent : MonoBehaviour
    {
        [SerializeField] private float sensitivity = 0.1f;
        [SerializeField] private float pitchMin = -80f;
        [SerializeField] private float pitchMax = 80f;

        private Transform cameraRoot;
        private float currentPitch;
        private bool enableLook = true;

        public bool Initialize(Transform camRoot)
        {
            if (camRoot == null)
            {
                Debug.LogError("PlayerController::Initialize(): camRoot is null.");
                return false;
            }
            cameraRoot = camRoot;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            return true;
        }

        public void OnUpdate(Vector2 lookDirection)
        {
            if (!enableLook) return;
            HandleYaw(lookDirection.x);
            HandlePitch(lookDirection.y);
        }

        private void HandleYaw(float yaw)
        {
            transform.Rotate(Vector3.up, yaw * sensitivity);
        }

        private void HandlePitch(float pitch)
        {
            currentPitch -= pitch * sensitivity;
            currentPitch = Mathf.Clamp(currentPitch, pitchMin, pitchMax);
            cameraRoot.localRotation = Quaternion.Euler(currentPitch, 0f, 0f);
        }

        public float GetPitch() => currentPitch;
        public bool SetEnabled(bool value) => enableLook = value;
    }
}