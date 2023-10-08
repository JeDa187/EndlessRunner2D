using System.Collections;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    private float minSpawnRate = 2.0f;
    private float maxSpawnRate = 6.0f;
    private ObstacleManager obstacleManager;

    private void Start()
    {
        obstacleManager = GetComponent<ObstacleManager>();
        obstacleManager.SpawnObstacle();
        StartCoroutine(SpawnObstacles());
    }

    IEnumerator SpawnObstacles()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnRate, maxSpawnRate));
            obstacleManager.SpawnObstacle();
        }
    }
}
