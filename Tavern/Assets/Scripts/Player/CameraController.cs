using Unity.Cinemachine;
using UnityEngine;

namespace Player
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform controllerRoot;
        [SerializeField] private Transform cameraRoot;
        [SerializeField] private CinemachineCamera cineCam;

        [SerializeField] private float bobFrequency = 2.5f;
        [SerializeField] private float bobAmplitude = 0.05f;

        [SerializeField] private float leanAngle = 3f;
        [SerializeField] private float leanSpeed = 8f;

        [SerializeField] private float dipAmount = 0.1f;
        [SerializeField] private float dipSpeed = 10f;

        [SerializeField] private float traumaDecaySpeed = 1.2f;
        [SerializeField] private float maxTraumaPitch = 5f;
        [SerializeField] private float maxTraumaYaw = 5f;
        [SerializeField] private float maxTraumaRoll = 5f;

        [SerializeField] private float baseFOV = 60f;
        [SerializeField] private float sprintFOVIncrease = 5f;
        [SerializeField] private float landingFOVDip = 5f;
        [SerializeField] private float fovSpeed = 8f;
        
        [SerializeField] private float interactionTiltAngle = 2f;
        [SerializeField] private float interactionTiltSpeed = 6f;
        [SerializeField] private float interactionTiltEaseOut = 3f;
        [SerializeField] private float maxInteractionTilt = 5f;

        private float bobTimer;
        private float currentLean;
        private float currentDip;
        private float previousVerticalVelocity;
        private float currentInteractionTilt;

        private float trauma;
        private float traumaSeed;

        private float targetFOV;
        private float currentFOV;
        private float fovLandingDip;
        
        private bool isInteracting;
        public bool Initialize()
        {
            if (controllerRoot == null) return false;
            if (cameraRoot == null) return false;
            if (cineCam == null) return false;

            return true;
        }
        
        public Transform GetCamControllerRoot() => controllerRoot;
        public Transform GetCameraRoot() => cameraRoot;

        public void OnLateUpdate(Transform headBone, float speed, 
            float strafeInput, float verticalVelocity, bool isSprinting, bool isGrounded, float mouseDeltaY)
        {
            TrackHeadBone(headBone);
            HandleBob(speed, isGrounded);
            HandleLean(strafeInput);
            HandleDip(verticalVelocity);
            HandleTrauma();
            HandleFOV(isSprinting, verticalVelocity);
            HandleInteractionTilt(mouseDeltaY);
            ApplyOffsets();
        }

        public void AddTrauma(float traumaAmount)
        {
            trauma = Mathf.Clamp01(trauma + traumaAmount);
        }
    
        private void TrackHeadBone(Transform headBone)
        {
            float newPositionY = headBone.position.y + 0.15f;
            Vector3 newPosition = new Vector3(headBone.position.x, newPositionY, headBone.position.z);
            controllerRoot.position = Vector3.Lerp(
                controllerRoot.position,
                newPosition,
                1f - Mathf.Pow(0.01f, Time.deltaTime));
        }

        private void HandleBob(float speed, bool isGrounded)
        {
            if (speed > 0.1f && isGrounded)
            {
                bobTimer += Time.deltaTime * bobFrequency * speed;
                bobTimer %= Mathf.PI * 2f;
            }
            else
            {
                bobTimer = Mathf.Lerp(bobTimer, 0f, Time.deltaTime * 5f);
            }
        }
        
        private void HandleLean(float strafeInput)
        {
            float targetLean = -strafeInput * leanAngle;
            currentLean = Mathf.Lerp(currentLean, targetLean, Time.deltaTime * leanSpeed);
        }
        
        private void HandleTrauma()
        {
            trauma = Mathf.Clamp01(trauma - traumaDecaySpeed * Time.deltaTime);
        }
        
        private void HandleFOV(bool isSprinting, float verticalVelocity)
        {
            targetFOV = baseFOV;
            if (isSprinting) targetFOV += sprintFOVIncrease;
            targetFOV += fovLandingDip;

            currentFOV = Mathf.Lerp(currentFOV, targetFOV, Time.deltaTime * fovSpeed);
            cineCam.Lens.FieldOfView = currentFOV;
        }

        private void HandleDip(float verticalVelocity)
        {
            float velocityDelta = verticalVelocity - previousVerticalVelocity;

            if (velocityDelta < -5f)
                currentDip = -dipAmount;

            currentDip = Mathf.Lerp(currentDip, 0f, Time.deltaTime * dipSpeed);
            previousVerticalVelocity = verticalVelocity;
        }

        private void HandleInteractionTilt(float mouseDeltaY)
        {
            if (isInteracting)
            {
                currentInteractionTilt = Mathf.Clamp(
                    currentInteractionTilt + -mouseDeltaY * interactionTiltAngle * Time.deltaTime,
                    -maxInteractionTilt,
                    maxInteractionTilt);
            }
            else
            {
                currentInteractionTilt = Mathf.Lerp(currentInteractionTilt, 0f, Time.deltaTime * interactionTiltEaseOut);
            }
        }

        public void SetInteracting(bool interacting)
        {
            isInteracting = interacting;
        }

        private void ApplyOffsets()
        {
            float shake = trauma * trauma;
            float seed = Time.time * traumaSeed;

            float traumaPitch = maxTraumaPitch * shake * (Mathf.PerlinNoise(seed, 0f) * 2f - 1f);
            float traumaYaw = maxTraumaYaw * shake * (Mathf.PerlinNoise(0f, seed) * 2f - 1f);
            float traumaRoll = maxTraumaRoll * shake * (Mathf.PerlinNoise(seed, seed) * 2f - 1f);

            float bobOffsetY = Mathf.Sin(bobTimer) * bobAmplitude;
            cameraRoot.localPosition = new Vector3(0f, bobOffsetY + currentDip, 0f);
            cameraRoot.localRotation = Quaternion.Euler(
                traumaPitch + currentInteractionTilt,
                traumaYaw,
                currentLean + traumaRoll);
        }
    }
}