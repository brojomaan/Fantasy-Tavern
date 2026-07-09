using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Liquids
{
    [System.Serializable]
    public class LiquidEntry
    {
        public LiquidData liquidData;
        public float amount;

        public LiquidEntry(LiquidData data, float amt)
        {
            liquidData = data;
            amount = amt;
        }
    }

    public class LiquidMixer
    {
        private List<LiquidEntry> contents = new List<LiquidEntry>();
        public IReadOnlyList<LiquidEntry> Contents => contents;

        public void Add(LiquidData liquid, float amount)
        {
            LiquidEntry existing = contents.FirstOrDefault(e => e.liquidData == liquid);
            if (existing != null)
                existing.amount += amount;
            else
                contents.Add(new LiquidEntry(liquid, amount));
        }

        public float GetTotal() => contents.Sum(e => e.amount);

        public float GetNormalisedAmount(LiquidData liquid)
        {
            float total = GetTotal();
            if (total <= 0f) return 0f;
            LiquidEntry entry = contents.FirstOrDefault(e => e.liquidData == liquid);
            return entry == null ? 0f : entry.amount / total;
        }

        public bool Matches(LiquidMixer recipe, float tolerance)
        {
            if (recipe == null || GetTotal() <= 0f) return false;

            foreach (LiquidEntry entry in recipe.Contents)
            {
                float diff = Mathf.Abs(GetNormalisedAmount(entry.liquidData) - entry.amount);
                if (diff > tolerance) return false;
            }
            return true;
        }

        public void Clear() => contents.Clear();

        public Color GetMixedColor()
        {
            if (contents.Count == 0) return Color.clear;

            float total = GetTotal();
            Color mixed = Color.black;

            foreach (LiquidEntry entry in contents)
            {
                float weight = entry.amount / total;
                mixed += entry.liquidData.LiquidColor * weight;
            }

            mixed.a = 1f;
            
            return mixed;
        }

        // Serialize by ID string for networking
        public string Serialize()
        {
            string result = "";
            foreach (LiquidEntry entry in contents)
            {
                float rounded = Mathf.Round(entry.amount * 1000f) / 1000f;
                result += $"{entry.liquidData.LiquidId}:{rounded};";
            }
            return result;
        }

        // Deserialize needs LiquidRegistry to look up LiquidData by ID
        public void Deserialize(string data, LiquidRegistry registry)
        {
            contents.Clear();
            if (string.IsNullOrEmpty(data)) return;

            foreach (string entry in data.Split(';'))
            {
                if (string.IsNullOrEmpty(entry)) continue;
                string[] parts = entry.Split(':');
                if (parts.Length != 2) continue;

                LiquidData liquid = registry.GetLiquid(parts[0]);
                if (liquid != null && float.TryParse(parts[1], out float amount))
                    contents.Add(new LiquidEntry(liquid, amount));
            }
        }
    }
}