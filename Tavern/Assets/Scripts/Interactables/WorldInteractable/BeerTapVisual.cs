using Liquids;
using UnityEngine;

namespace Interactables.WorldInteractable
{
    public class BeerTapVisual : MonoBehaviour
    {
        [SerializeField] private MeshRenderer liquidStream;
        [SerializeField] private float activationThreshold = 5f;
        [SerializeField] private float transitionSpeed = 5f;

        private float currentActive;
        private Material activeStreamMaterial;

        private static readonly int Active = Shader.PropertyToID("_Active");
        private static readonly int Color = Shader.PropertyToID("_Color");

        public void Initialize(LiquidData liquidData)
        {
            activeStreamMaterial = new Material(liquidStream.material);
            liquidStream.material = activeStreamMaterial;
            activeStreamMaterial.SetFloat(Active, 0f);
            activeStreamMaterial.SetColor(Color, liquidData.LiquidColor);
        }

        public void OnUpdate(float currentAngle, bool isAuthority, float syncedActive)
        {
            if (isAuthority)
            {
                float targetActive = currentAngle > activationThreshold ? 1f : 0f;
                currentActive = Mathf.Lerp(currentActive, targetActive, Time.deltaTime * transitionSpeed);
            }
            else
            {
                currentActive = Mathf.Lerp(currentActive, syncedActive, Time.deltaTime * transitionSpeed);
            }

            activeStreamMaterial.SetFloat(Active, currentActive);
        }
    }
}
