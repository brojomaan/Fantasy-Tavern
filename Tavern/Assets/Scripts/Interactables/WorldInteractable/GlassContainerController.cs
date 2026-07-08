using System.Collections;
using Coherence;
using Coherence.Toolkit;
using Components.CharacterComponents;
using Interactables.ItemInteractables.Mug;
using Interfaces;
using UnityEngine;

namespace Interactables.WorldInteractable
{
    public class GlassContainerController : WorldInteractable
    {
        [SerializeField] private CoherenceSync sync;
        [SerializeField] private Transform lid;
        [SerializeField] private Transform glassSpawnPoint;
        [SerializeField] private GameObject glassPrefab;
        [SerializeField] private float minAngle = 0f;
        [SerializeField] private float maxAngle = 90f;
        [SerializeField] private float driveSpeed = 15f;
        [SerializeField] private float springSpeed = 4f;
        [SerializeField] private float stayOpenDuration = 5f;
        [SerializeField] private float openThreshold = 30f;

        [Sync] public float syncedAngle;
        private float currentAngle;
        private bool isInteracting;
        private Coroutine closeCoroutine;

        public bool IsOpen => currentAngle >= openThreshold;


        private void Update()
        {
            if (sync.HasStateAuthority)
            {
                if (isInteracting) return;

                if (currentAngle > 0.01f)
                {
                    currentAngle = Mathf.Lerp(currentAngle, 0f, Time.deltaTime * springSpeed);
                    lid.localRotation = Quaternion.Euler(-currentAngle, 0f, 0f);
                    syncedAngle = currentAngle;
                }
                return;
            }
            
            currentAngle = Mathf.Lerp(currentAngle, syncedAngle, Time.deltaTime * driveSpeed);
            lid.localRotation = Quaternion.Euler(-currentAngle, 0f, 0f);
        }

        public override void OnInteract()
        {
            if (!sync.HasStateAuthority)
                sync.RequestAuthority(AuthorityType.Full);
    
            isInteracting = true;

            if (closeCoroutine != null)
                StopCoroutine(closeCoroutine);

        }

        public override void OnInteractUpdate(Vector2 lookDirection)
        {
            if (!sync.HasStateAuthority) return;

            currentAngle = Mathf.Clamp(
                currentAngle + lookDirection.y * driveSpeed * Time.deltaTime,
                minAngle,
                maxAngle);
            lid.localRotation = Quaternion.Euler(-currentAngle, 0f, 0f);
            syncedAngle = currentAngle;
        }
        
        public override void OnInteractRelease()
        {
            if (IsOpen)
                closeCoroutine = StartCoroutine(CloseAfterDelay());
        }
        
        public void SpawnGlass(HoldComponent holdComponent)
        {
            if (!IsOpen) return;
            if (!sync.HasStateAuthority) return;

            GameObject glass = Instantiate(glassPrefab, glassSpawnPoint.position, glassSpawnPoint.rotation);
            IHoldable holdable = glass.GetComponent<IHoldable>();
            if (holdable != null)
            {
                holdComponent.PickUp(glass, holdable);
            }
        }
        
        private IEnumerator CloseAfterDelay()
        {
            yield return new WaitForSeconds(stayOpenDuration);
            isInteracting = false;
        }
    }
}