using UnityEngine;

namespace NPC
{
    public class NpcSpawner : MonoBehaviour
    {
        [SerializeField] private NpcController npcPrefab;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Transform exitPoint;
        [SerializeField] private Transform chairPoint;
        [SerializeField] private float spawnInterval = 10f;

        private float spawnTimer;
        private bool isActive = false;

        public void SetActive(bool active)
        {
            isActive = active;
            spawnTimer = 0f;
        }

        private void Update()
        {
            if (!isActive) return;

            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnInterval)
            {
                spawnTimer = 0f;
                SpawnNpc();
            }
        }
        
        private void SpawnNpc()
        {
            if (npcPrefab == null || spawnPoint == null || exitPoint == null) return;
            NpcController npc = Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);
            npc.Initialize(chairPoint, exitPoint);

        }
    }
}