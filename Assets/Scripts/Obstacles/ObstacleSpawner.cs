using System.Collections;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] downObstaclePrefabs;
    public GameObject[] upObstaclePrefabs;
    public Camera mainCamera;
    public float obstacleSpawnRate;

    private float spawnXPositionOffset = 6f;

    private void Start()
    {
        StartCoroutine(SpawnObstacles());
    }

    IEnumerator SpawnObstacles()
    {
        while (true)
        {
            obstacleSpawnRate = Random.Range(3.0f, 12.0f);
            yield return new WaitForSeconds(obstacleSpawnRate);

            float cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
            float spawnXPosition = mainCamera.transform.position.x + cameraHalfWidth + spawnXPositionOffset;

            int listChoice = Random.Range(0, 2);
            GameObject obstaclePrefab;
            float randomY;
            Quaternion rotation = Quaternion.identity; // Alustetaan oletusarvoiseksi

            if (listChoice == 0)
            {
                obstaclePrefab = downObstaclePrefabs[Random.Range(0, downObstaclePrefabs.Length)];
                randomY = Random.Range(-12f, -2.85f);
            }
            else
            {
                obstaclePrefab = upObstaclePrefabs[Random.Range(0, upObstaclePrefabs.Length)];
                randomY = Random.Range(4f, 13f);
                rotation = Quaternion.Euler(180, 0, 0); // Jos "up"-este, käännä ylösalaisin
            }

            Instantiate(obstaclePrefab, new Vector2(spawnXPosition, randomY), rotation); // Käytetään rotation-muuttujaa
        }
    }
}
