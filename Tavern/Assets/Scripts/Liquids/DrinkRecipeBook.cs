using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Liquids
{
    [CreateAssetMenu(fileName = "DrinkRecipeBook", menuName = "Tavern/Recipe/DrinkRecipeBook")]
    public class DrinkRecipeBook : ScriptableObject
    {
        public static DrinkRecipeBook Instance { get; private set; }
        
        [SerializeField] private DrinkRecipe[] recipes;

        private void OnEnable()
        {
            Instance = this;
        }

        public DrinkRecipe GetRandomRecipe()
        {
            if (recipes == null || recipes.Length == 0) return null;
            return recipes[Random.Range(0, recipes.Length)];
        }
    }
}
