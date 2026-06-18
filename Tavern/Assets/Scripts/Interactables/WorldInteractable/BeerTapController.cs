using Interfaces;
using UnityEngine;

namespace Interactables.WorldInteractable
{
    public class BeerTapController : WorldInteractable
    {

        [SerializeField] private Transform handle;
        [SerializeField] private float minAngle = 0f;
        [SerializeField] private float maxAngle = 45f;
        [SerializeField] private float driveSpeed = 50f;
        [SerializeField] private float springSpeed = 10f;

        private bool isBeingInteracted;
        private float currentAngle;

        public override bool CanInteractWith(IHoldable heldItem) => heldItem == null;

        public override void OnInteract() { }

        public override void OnInteractUpdate(Vector2 lookDirection)
        {
            isBeingInteracted = true;
            Drive(lookDirection.y);
        }
        public override void OnInteractRelease()
        {
            Release();
        }

        public void Drive(float mouseDelta)
        {
            currentAngle = Mathf.Clamp(
                currentAngle + -mouseDelta * driveSpeed * Time.deltaTime,
                minAngle,
                maxAngle);
            handle.localRotation = Quaternion.Euler(0f, 0f, -currentAngle);
        }

        public void Release()
        {
            isBeingInteracted = false;
        }

        private void Update()
        {
            if (!isBeingInteracted)
            {
                currentAngle = Mathf.Lerp(currentAngle, 0f, Time.deltaTime * springSpeed);
                handle.localRotation = Quaternion.Euler(0f, 0f, -currentAngle);
            }
        }
    }
}