using UnityEngine;

namespace Interactables.ItemInteractables.Mug
{
    public class MugVisual : ItemVisual
    {
        [SerializeField] private MeshRenderer overflowMesh;
        [SerializeField] private MeshRenderer liquidMesh;
        [SerializeField] private float maxWobbleAmount = 0.05f;
        [SerializeField] private float minLiquidScale = 0f;
        [SerializeField] private float maxLiquidScale = 1f;
        [SerializeField] private float overflowFadeDuration = 0.25f;
        private float overflowFadeTimer;

        private float wobbleX;
        private float wobbleZ;
        private float wobbleAmplitude;
        private float wobbleTime;
        
        private Material activeLiquidMaterial;
        private Material activeOverflowMaterial;
        
        //Liquid Fill Shader
        private static readonly int FillLevel = Shader.PropertyToID("_FillLevel");
        private static readonly int WobbleX = Shader.PropertyToID("_WobbleX");
        private static readonly int WobbleZ = Shader.PropertyToID("_WobbleZ");
        private static readonly int WobbleAmount = Shader.PropertyToID("_WobbleAmount");
        
        //liquid Stream Shader
        private static readonly int Active = Shader.PropertyToID("_Active");
        private static readonly int ScrollSpeed = Shader.PropertyToID("ScrollSpeed");

        public override bool Initialize()
        {
            base.Initialize();
            if (liquidMesh == null) { Debug.LogError("MugVisual::Initialize(): liquidMesh is null."); return false; }
            if (overflowMesh == null) { Debug.LogError("MugVisual::Initialize(): overflowMesh is null."); return false; }
            
            activeLiquidMaterial = new Material(liquidMesh.material);
            liquidMesh.material = activeLiquidMaterial;

            activeOverflowMaterial = new Material(overflowMesh.material);
            overflowMesh.material = activeOverflowMaterial;
            activeOverflowMaterial.SetFloat(FillLevel, 1f);

            overflowMesh.gameObject.SetActive(false);
            
            overflowFadeTimer = overflowFadeDuration;
            
            return true;
        }

        public void OnUpdate(float fillLevel, float fillRate, float maxCapacity)
        {
            // Fill rate drives amplitude, clamped to max
            wobbleAmplitude = Mathf.Lerp(wobbleAmplitude, fillRate * maxWobbleAmount, Time.deltaTime * 5f);
            wobbleAmplitude = Mathf.Clamp(wobbleAmplitude, 0f, maxWobbleAmount);

            // Time just keeps ticking to animate the sine wave
            wobbleTime += Time.deltaTime;

            // X and Z are just offset sine inputs driven by time
            wobbleX = wobbleTime * 10f;
            wobbleZ = wobbleTime * 8f;

            activeLiquidMaterial.SetFloat(FillLevel, fillLevel);
            activeLiquidMaterial.SetFloat(WobbleX, wobbleX);
            activeLiquidMaterial.SetFloat(WobbleZ, wobbleZ);
            activeLiquidMaterial.SetFloat(WobbleAmount, wobbleAmplitude);
            
            HandleOverflow(fillLevel, maxCapacity);
        }

        private void HandleOverflow(float fillLevel, float maxCapacity)
        {
            bool isOverflowing = fillLevel > maxCapacity;
            overflowMesh.gameObject.SetActive(isOverflowing);
    
            if (isOverflowing)
            {
                float overflowAmount = fillLevel - maxCapacity;
                activeOverflowMaterial.SetFloat(Active, 1f);
                activeOverflowMaterial.SetFloat(ScrollSpeed, overflowAmount * 10f);
            }
            else
            {
                activeOverflowMaterial.SetFloat(Active, 0f);
            }
        }
    }
}
