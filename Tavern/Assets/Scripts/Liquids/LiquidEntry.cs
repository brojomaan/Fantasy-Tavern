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

    [System.Serializable]
    public class LiquidMixerData
    {
        public List<LiquidEntry> entries = new List<LiquidEntry>();
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

        public float GetAmount(LiquidData liquid)
        {
            LiquidEntry entry = contents.FirstOrDefault(e => e.liquidData == liquid);
            return entry?.amount ?? 0f;
        }

        public float GetNormalisedAmount(LiquidData liquid)
        {
            float total = GetTotal();
            if (total <= 0f) return 0f;
            return GetAmount(liquid) / total;
        }

        public bool Matches(LiquidMixer recipe, float tolerance)
        {
            if (recipe == null) return false;
            float total = GetTotal();
            if (total <= 0f) return false;

            foreach (LiquidEntry entry in recipe.Contents)
            {
                float normalisedAmount = GetNormalisedAmount(entry.liquidData);
                float normalisedTarget = entry.amount;

                if (System.Math.Abs(normalisedAmount - normalisedTarget) > tolerance)
                    return false;
            }

            return true;
        }

        public void Clear() => contents.Clear();

        public string Serialize()
        {
            List<LiquidEntry> rounded = contents.Select(e => 
                new LiquidEntry(e.liquidData, Mathf.Round(e.amount * 1000f) / 1000f)).ToList();
            return JsonUtility.ToJson(new LiquidMixerData { entries = rounded });
        }

        public void Deserialize(string json)
        {
            if (string.IsNullOrEmpty(json)) return;
            LiquidMixerData data = JsonUtility.FromJson<LiquidMixerData>(json);
            contents = data.entries;
        }

        [System.Serializable]
        private class LiquidMixerData
        {
            public List<LiquidEntry> entries;
        }
    }
}