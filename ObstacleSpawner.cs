using UnityEngine;
using System.Collections.Generic;

public class ObstacleSpawner : MonoBehaviour
{
    [Tooltip("List of possible obstacle prefabs to spawn")]
    public GameObject[] obstaclePrefabs;

    [Tooltip("Spawn points for obstacles on this plot of land")]
    public Transform[] spawnPoints;

    [Tooltip("Number of obstacles to spawn when the land is purchased")]
    public int obstaclesToSpawn = 10;
    Obstacle obstacleScript;
    private List<int> usedIndices = new List<int>();

    public void SpawnObstacles()
    {
        if (spawnPoints.Length == 0 || obstaclePrefabs.Length == 0)
        {
            Debug.LogWarning("Missing spawn points or obstacle prefabs.");
            return;
        }

        usedIndices.Clear();

        int spawnCount = Mathf.Min(obstaclesToSpawn, spawnPoints.Length);

        for (int i = 0; i < spawnCount; i++)
        {
            int index = GetUniqueRandomIndex();
            if (index == -1) break;

            Transform spawnPoint = spawnPoints[index];
            GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
            GameObject obstacle = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        }
    }

    private int GetUniqueRandomIndex()
    {
        if (usedIndices.Count >= spawnPoints.Length)
            return -1;

        int index;
        do
        {
            index = Random.Range(0, spawnPoints.Length);
        }
        while (usedIndices.Contains(index));

        usedIndices.Add(index);
        return index;
    }
}