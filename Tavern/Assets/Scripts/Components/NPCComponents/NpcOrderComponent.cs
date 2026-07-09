using Interactables.WorldInteractable;
using Liquids;
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

        [SerializeField] private DrinkRecipe currentRecipe;

        private LiquidMixer targetRecipe;
        private float bubbleTimer;
        private bool bubbleVisible;
        private string orderName = "beer";
        
        public bool Initialize(DrinkRecipe drinkRecipe)
        {
            if (speechBubble == null) { Debug.LogError($"NpcOrderComponent::Initialize(): speechBubble is null."); return false; }
            if (orderText == null) { Debug.LogError($"NpcOrderComponent::Initialize(): orderText is null."); return false; }

            currentRecipe = drinkRecipe;

            targetRecipe = drinkRecipe.ToLiquidMixer();
            orderName = drinkRecipe.DisplayName;
            orderText.text = orderName;

            HideBubble();
            return true;
        }


        public void SetOrder(string order)
        {
            orderText.text = order;
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

        public string GetOrderName() => orderName;
        public bool IsBubbleVisible() => bubbleVisible;
    }
}