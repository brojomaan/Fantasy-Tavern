using UnityEngine;

namespace Liquids
{
    [CreateAssetMenu(fileName = "LiquidRegistry", menuName = "Tavern/LiquidRegistry")]
    public class LiquidRegistry : ScriptableObject
    {
        public static LiquidRegistry Instance { get; private set; }

        [SerializeField] private LiquidData[] liquids;

        private void OnEnable()
        {
            Instance = this;
        }

        public LiquidData GetLiquid(string liquidId)
        {
            foreach (LiquidData liquid in liquids)
            {
                if (liquid.LiquidId == liquidId)
                {
                    return liquid;
                }
            }
            
            Debug.LogError($"LiquidRegistry: Could not find liquid with id {liquidId}");
            return null;
        }
    }
}
