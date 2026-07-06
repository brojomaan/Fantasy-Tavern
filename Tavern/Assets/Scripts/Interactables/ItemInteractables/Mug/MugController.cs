using Coherence.Toolkit;
using Interactables.WorldInteractable;
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

        [SerializeField] private LiquidMixer liquidMixer = new LiquidMixer();

        private float currentFillRate;

        [Sync] public float fillLevel;
        [Sync] public float syncedActive;

        public float FillLevel => fillLevel;
        public float TargetFillLevel => targetFillLevel;
        public float AcceptableRange => acceptableRange;
        public float MaxCapacity => maxCapacity;
        public bool IsOverflowing => fillLevel > maxCapacity;
        public LiquidMixer GetLiquidMixer() => liquidMixer;

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
            
            if (sync.HasStateAuthority)
            {
                currentFillRate = Mathf.Lerp(currentFillRate, 0f, Time.deltaTime * 5f);
        
                // Drain excess back to max capacity when not filling
                if (fillLevel > maxCapacity && currentFillRate < 0.01f)
                    fillLevel = Mathf.Lerp(fillLevel, maxCapacity - 0.05f, Time.deltaTime * 3f);
            }
            else
            {
                currentFillRate = 0f;
            }
            
            mugVisual.OnUpdate(fillLevel, currentFillRate, maxCapacity);
        }
        
        public void Fill(float amount, string liquidId)
        {
            if (!sync.HasStateAuthority) return;
            currentFillRate = amount * 10;
            fillLevel = Mathf.Clamp(fillLevel + amount * fillSpeed * Time.deltaTime, 0f, maxCapacity + 0.2f);
            liquidMixer.Add(liquidId, amount * fillSpeed * Time.deltaTime);
            Debug.Log($"LiquidMixer contents: {liquidId} = {liquidMixer.GetAmount(liquidId):F3} / Total = {liquidMixer.GetTotal():F3}");
        }
    }
}