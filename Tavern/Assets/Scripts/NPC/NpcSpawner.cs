using UnityEngine;

namespace NPC
{
    public class NpcSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject npcPrefab;

        private void Start()
        {
            SpawnNPC();
        }

        private void SpawnNPC()
        {
            Instantiate(npcPrefab, new Vector3(3f, 0f, 3f), Quaternion.identity);
        }
    }
}