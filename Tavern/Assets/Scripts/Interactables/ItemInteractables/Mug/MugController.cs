using Coherence.Toolkit;
using Interfaces;
using UnityEngine;

namespace Interactables.ItemInteractables.Mug
{
    public class MugController : ItemController, IFillable
    {
        [SerializeField] private float fillSpeed = 0.1f;
        [SerializeField] private float targetFillLevel = 0.8f;
        [SerializeField] private float acceptableRange = 0.15f;
        [SerializeField] private float maxCapacity = 1f;
        [SerializeField] private MugVisual mugVisual;

        private float currentFillRate;

        [Sync] public float fillLevel;

        public float FillLevel => fillLevel;
        public float TargetFillLevel => targetFillLevel;
        public float AcceptableRange => acceptableRange;
        public float MaxCapacity => maxCapacity;
        public bool IsOverflowing => fillLevel > maxCapacity;
        public bool IsInSweetSpot => fillLevel <= targetFillLevel + acceptableRange &&
                                     fillLevel <= maxCapacity + acceptableRange;

        private void Start()
        {
            Initialize();
        }
        public override void Initialize()
        {
            base.Initialize();
            if (!mugVisual.Initialize()) 
                Debug.LogError("MugController::Initialize(): mugVisual failed.");

        }

        private void Update()
        {
            if (!hasInitialized) return;
            if (!sync.HasStateAuthority) currentFillRate = 0f;
            Debug.Log($"fill Level: {currentFillRate}");
            mugVisual.OnUpdate(fillLevel, currentFillRate);

        }
        public void Fill(float amount)
        {
            if (!sync.HasStateAuthority) return;
            currentFillRate = amount * 10;
            fillLevel = Mathf.Clamp(fillLevel + amount * fillSpeed * Time.deltaTime, 0f, maxCapacity + 0.2f);
        }
    }
}