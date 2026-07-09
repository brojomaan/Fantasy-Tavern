using System.Collections.Generic;
using Interactables.WorldInteractable;
using UnityEngine;

namespace Liquids
{
    [CreateAssetMenu(fileName = "DrinkRecipe", menuName = "Tavern/Recipe/DrinkRecipe")]
    public class DrinkRecipe : ScriptableObject
    {
        [SerializeField] private string displayName;
        [SerializeField] private List<LiquidEntry> ingredients;

        public string DisplayName => displayName;
        public IReadOnlyList<LiquidEntry> Ingredients => ingredients;

        public LiquidMixer ToLiquidMixer()
        {
            LiquidMixer mixer = new LiquidMixer();
            foreach (LiquidEntry entry in ingredients)
            {
                mixer.Add(entry.liquidData, entry.amount);
            }

            return mixer;
        }
    }
}
