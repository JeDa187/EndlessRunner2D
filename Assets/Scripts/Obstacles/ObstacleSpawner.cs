using UnityEngine;
using System.Collections;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] downObstaclePrefabs;
    public GameObject[] upObstaclePrefabs;
    public Camera mainCamera;
    public float obstacleSpawnRate;
    public InfiniteParallaxBackground infiniteParallaxBackground;

    private float spawnXPositionOffset = 6f;
    private bool firstObstacleSpawned = false;

    private void Start()
    {
        // Aseta obstacleSpawnRate ensimmäiselle spawnille
        obstacleSpawnRate = 0.0f; // Tämä voi olla mikä tahansa arvo, joka tarkoittaa "heti"

        // Kutsu SpawnObstacle-funktiota kerran pelin alussa
        SpawnObstacles();

        // Aloita normaali ajoitettu spawnaus
        StartCoroutine(SpawnObstacles());
    }

    private GameObject GetRightmostGroundSecond()
    {
        GameObject[] grounds = GameObject.FindGameObjectsWithTag("Ground_Second");
        GameObject rightmostGround = null;
        float maxX = float.NegativeInfinity;
        foreach (GameObject ground in grounds)
        {
            if (ground.transform.position.x > maxX)
            {
                maxX = ground.transform.position.x;
                rightmostGround = ground;
            }
        }
        return rightmostGround;
    }

    IEnumerator SpawnObstacles()
    {
        while (true)
        {
            yield return new WaitForSeconds(obstacleSpawnRate);

            if (!firstObstacleSpawned)
            {
                firstObstacleSpawned = true;
            }
            else
            {
                obstacleSpawnRate = Random.Range(3.0f, 12.0f) / infiniteParallaxBackground.CameraSpeed;
            }

            float cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
            float spawnXPosition = mainCamera.transform.position.x + cameraHalfWidth + spawnXPositionOffset;

            int listChoice = Random.Range(0, 2);
            GameObject obstaclePrefab;
            float randomY;
            Quaternion rotation = Quaternion.identity;

            if (listChoice == 0)
            {
                obstaclePrefab = downObstaclePrefabs[Random.Range(0, downObstaclePrefabs.Length)];
                randomY = Random.Range(-12f, -2.85f);
            }
            else
            {
                obstaclePrefab = upObstaclePrefabs[Random.Range(0, upObstaclePrefabs.Length)];
                randomY = Random.Range(4f, 13f);
                rotation = Quaternion.Euler(180, 0, 0);
            }

            GameObject newObstacle = Instantiate(obstaclePrefab, new Vector2(spawnXPosition, randomY), rotation);
            newObstacle.transform.parent = GetRightmostGroundSecond().transform;
        }
    }
}
