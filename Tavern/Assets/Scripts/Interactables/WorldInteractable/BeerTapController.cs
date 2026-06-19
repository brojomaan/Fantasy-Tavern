using System.Collections;
using Coherence;
using Coherence.Toolkit;
using Interfaces;
using UnityEngine;

namespace Interactables.WorldInteractable
{
    public class BeerTapController : WorldInteractable
    {
         [SerializeField] private CoherenceSync sync;
        [SerializeField] private Transform handle;
        [SerializeField] private float minAngle = 0f;
        [SerializeField] private float maxAngle = 45f;
        [SerializeField] private float driveSpeed = 15f;
        [SerializeField] private float springSpeed = 4f;

        [SerializeField] private TapSpout spout;

        [Sync] public float syncedAngle;
        private bool isInteracting;
        private float currentAngle;

        private void Update()
        {
            if (sync.HasStateAuthority)
            {
                if (isInteracting) return;
                if (currentAngle > 0.01f)
                {
                    currentAngle = Mathf.Lerp(currentAngle, 0f, Time.deltaTime * springSpeed);
                    handle.localRotation = Quaternion.Euler(0f, 0f, -currentAngle);
                    syncedAngle = currentAngle;
                }
                return;
            }

            // Non-authority lerps toward synced value
            currentAngle = Mathf.Lerp(currentAngle, syncedAngle, Time.deltaTime * driveSpeed);
            handle.localRotation = Quaternion.Euler(0f, 0f, -currentAngle);
        }

        public override bool CanInteractWith(IHoldable heldItem) => heldItem == null;

        public override void OnInteract()
        {
            if (!sync.HasStateAuthority)
                sync.RequestAuthority(AuthorityType.Full);
            isInteracting = true;
        }

        public override void OnInteractUpdate(Vector2 lookDirection)
        {
            if (!sync.HasStateAuthority) return;
            
            currentAngle = Mathf.Clamp(
                currentAngle + -lookDirection.y * driveSpeed * Time.deltaTime,
                minAngle,
                maxAngle);
            handle.localRotation = Quaternion.Euler(0f, 0f, -currentAngle);
            syncedAngle = currentAngle;
            
            spout.OnUpdate(currentAngle, maxAngle);
        }

        public override void OnInteractRelease()
        {
            isInteracting = false;
        }
    }
}