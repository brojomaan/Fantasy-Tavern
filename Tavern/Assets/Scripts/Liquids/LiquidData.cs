using UnityEngine;

namespace Liquids
{
    [CreateAssetMenu(fileName = "LiquidData", menuName = "Tavern/Liquid/LiquidData")]
    public class LiquidData : ScriptableObject
    {
        [SerializeField] private string liquidId;
        [SerializeField] private string displayName;
        [SerializeField] private Color liquidColor;

        public string LiquidId => liquidId;
        public string DisplayName => displayName;
        public Color LiquidColor => liquidColor;
    }
}
