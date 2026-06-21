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

        public void Initialize()
        {
            activeStreamMaterial = new Material(liquidStream.material);
            liquidStream.material = activeStreamMaterial;
            activeStreamMaterial.SetFloat(Active, 0f);
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
