using Interactables.WorldInteractable;
using TMPro;
using UnityEngine;

namespace Components.NPCComponents
{
    public class NpcOrderComponent : MonoBehaviour
    {
        [SerializeField] private GameObject speechBubble;
        [SerializeField] private TextMeshProUGUI orderText;
        [SerializeField] private float bubbleDisplayTime = 5f;
        [SerializeField] private float matchTolerance = 0.2f;

        private LiquidMixer targetRecipe;
        private float bubbleTimer;
        private bool bubbleVisible;
        
        public bool Initialize()
        {
            if (speechBubble == null) { Debug.LogError($"NpcOrderComponent::Initialize(): speechBubble is null."); return false; }
            if (orderText == null) { Debug.LogError($"NpcOrderComponent::Initialize(): orderText is null."); return false; }

            targetRecipe = new LiquidMixer();
            targetRecipe.Add("beer", 1.0f);
            orderText.text = "Beer!";
            
            speechBubble.SetActive(false);
            return true;
        }

        public void OnUpdate()
        {
            if (!bubbleVisible) return;
            bubbleTimer -= Time.deltaTime;
            if (bubbleTimer <= 0f) HideBubble();
        }

        public void ShowBubble()
        {
            bubbleVisible = true;
            bubbleTimer = bubbleDisplayTime;
            speechBubble.SetActive(true);
        }

        public void HideBubble()
        {
            bubbleVisible = false;
            speechBubble.SetActive(false);
        }

        public bool TryFulfillOrder(LiquidMixer contents, float fillLevel, float targetFillLevel, float acceptableRange)
        {
            if (contents == null) return false;
            if (fillLevel < targetFillLevel - acceptableRange) return false;
            return contents.Matches(targetRecipe, matchTolerance);
        }
    }
}