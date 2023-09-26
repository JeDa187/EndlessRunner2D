using System.Collections;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] downObstaclePrefabs;
    public GameObject[] upObstaclePrefabs;
    public Camera mainCamera;
    public float obstacleSpawnRate;
    public float speedIncreaseFactor = 0.1f; // New variable to increase speed with each new object
    public InfiniteParallaxBackground parallaxBackground;

    private float speedIncrease = 0f; // Track the increased speed
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

            if (listChoice == 0)
            {
                obstaclePrefab = downObstaclePrefabs[Random.Range(0, downObstaclePrefabs.Length)];
                randomY = Random.Range(-12f, -2.85f);
            }
            else
            {
                obstaclePrefab = upObstaclePrefabs[Random.Range(0, upObstaclePrefabs.Length)];
                randomY = Random.Range(4f, 13f);
            }

            GameObject obstacle = Instantiate(obstaclePrefab, new Vector2(spawnXPosition, randomY), Quaternion.identity);

            if (listChoice == 1)
            {
                obstacle.transform.eulerAngles = new Vector3(180, 0, 0);
            }

            speedIncrease += speedIncreaseFactor; // Increase the speed by speedIncreaseFactor
            float layer0Speed = parallaxBackground.CameraSpeed * (1 - parallaxBackground.LayerScrollSpeeds[0]) + speedIncrease;
            obstacle.GetComponent<ObstacleMovement>().SetSpeed(layer0Speed);

            float destructionPosition = mainCamera.transform.position.x - cameraHalfWidth - 2f;
            obstacle.GetComponent<ObstacleMovement>().SetDestructionXPosition(destructionPosition);
        }
    }
}
