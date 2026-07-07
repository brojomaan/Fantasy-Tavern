using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Interactables.WorldInteractable
{
    [System.Serializable]
    public class LiquidEntry
    {
        public string liquidId;
        public float amount;

        public LiquidEntry(string id, float amt)
        {
            liquidId = id;
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
        private IReadOnlyList<LiquidEntry> Contents => contents;

        public void Add(string liquidId, float amount)
        {
            LiquidEntry existing = contents.FirstOrDefault(e => e.liquidId == liquidId);
            if (existing != null)
            {
                existing.amount += amount;
            }
            else
            {
                contents.Add(new LiquidEntry(liquidId, amount));
            }
        }

        public float GetTotal()
        {
            return contents.Sum(e => e.amount);
        }

        public float GetAmount(string liquidId)
        {
            LiquidEntry entry = contents.FirstOrDefault(e => e.liquidId == liquidId);
            return entry?.amount ?? 0f;
        }

        public float GetNormalisedAmount(string liquidId)
        {
            float total = GetTotal();
            if (total <= 0f) return 0f;
            return GetAmount(liquidId) / total;
        }

        public bool Matches(LiquidMixer recipe, float tolerance)
        {
            if (recipe == null) return false;
            float total = GetTotal();
            if (total <= 0f) return false;

            foreach (LiquidEntry entry in recipe.contents)
            {
                float normalisedAmount = GetNormalisedAmount(entry.liquidId);
                float normalisedTarget = entry.amount;

                if (System.Math.Abs(normalisedAmount - normalisedTarget) > tolerance)
                {
                    return false;
                }
            }

            return true;
        }

        public void Clear()
        {
            contents.Clear();
        }

        public string Serialize()
        {
            List<LiquidEntry> rounded = contents.Select(e => new LiquidEntry(e.liquidId, Mathf.Round(e.amount * 1000f) / 1000f)).ToList();
            return JsonUtility.ToJson(new LiquidMixerData { entries = rounded });
        }

        public void Deserialize(string json)
        {
            if (string.IsNullOrEmpty(json)) return;
            LiquidMixerData data = JsonUtility.FromJson<LiquidMixerData>(json);
            contents = data.entries;
        }
    }
}