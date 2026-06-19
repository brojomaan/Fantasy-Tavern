using UnityEngine;

namespace Interactables.ItemInteractables.Mug
{
    public class MugVisual : ItemVisual
    {
        [SerializeField] private MeshRenderer overflowMesh;
        [SerializeField] private Material overflowMaterial;
        [SerializeField] private MeshRenderer liquidMesh;
        [SerializeField] private Material liquidMaterial;
        [SerializeField] private float minLiquidScale = 0f;
        [SerializeField] private float maxLiquidScale = 1f;

        private float wobbleX;
        private float wobbleZ;
        private float wobbleAmplitude;
        
        private static readonly int FillLevel = Shader.PropertyToID("_FillLevel");
        private static readonly int WobbleX = Shader.PropertyToID("_WobbleX");
        private static readonly int WobbleZ = Shader.PropertyToID("_WobbleZ");
        private static readonly int WobbleAmount = Shader.PropertyToID("_WobbleAmount");

        public override bool Initialize()
        {
            base.Initialize();
            if (liquidMesh == null) { Debug.LogError("MugVisual::Initialize(): liquidMesh is null."); return false; }
            if (overflowMesh == null) { Debug.LogError("MugVisual::Initialize(): overflowMesh is null."); return false; }
            liquidMesh.material = new Material(liquidMaterial);
            
            
            overflowMesh.gameObject.SetActive(false);
            return true;
        }

        public void OnUpdate(float fillLevel, float fillRate)
        {
            //Wobble Decays over time
            wobbleX = Mathf.Lerp(wobbleX, 0f, Time.deltaTime * 2f);
            wobbleZ = Mathf.Lerp(wobbleZ, 0f, Time.deltaTime * 2f);
            
            //Fill rate adds wobble
            wobbleAmplitude = Mathf.Lerp(wobbleAmplitude, fillRate * 0.5f, Time.deltaTime * 5f);
            
            //Animate Wobble
            wobbleX += Time.deltaTime * 10f * wobbleAmplitude;
            wobbleZ += Time.deltaTime * 10f * wobbleAmplitude;

            liquidMesh.material.SetFloat(FillLevel, fillLevel);
            liquidMesh.material.SetFloat(WobbleX, wobbleX);
            liquidMesh.material.SetFloat(WobbleZ, wobbleZ);
            liquidMesh.material.SetFloat(WobbleAmount, wobbleAmplitude);
        }

        private void HandleLiquidScale(float fillLevel)
        {
            
        }

        private void HandleOverflow(float fillLevel, float maxCapacity)
        {
            overflowMesh.gameObject.SetActive(fillLevel > maxCapacity);
        }
    }
}
