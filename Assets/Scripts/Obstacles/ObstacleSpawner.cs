using System.Collections;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public Camera mainCamera;
    public float spawnRate;
    public InfiniteParallaxBackground parallaxBackground;

    private float spawnXPositionOffset = 6f;

    private void Start()
    {
        StartCoroutine(SpawnObstacles());
    }

    IEnumerator SpawnObstacles()
    {
        while (true)
        {
            spawnRate = Random.Range(3.0f, 10.0f);  // Aseta spawnRate satunnaisesti v‰lille 3-10
            yield return new WaitForSeconds(spawnRate);

            float cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
            float spawnXPosition = mainCamera.transform.position.x + cameraHalfWidth + spawnXPositionOffset;

            float randomY = Random.Range(-12f, -2.85f);
            GameObject obstacle = Instantiate(obstaclePrefab, new Vector2(spawnXPosition, randomY), Quaternion.identity);

            float layer0Speed = parallaxBackground.CameraSpeed * (1 - parallaxBackground.LayerScrollSpeeds[0]);
            obstacle.GetComponent<ObstacleMovement>().SetSpeed(layer0Speed);

            float destructionPosition = mainCamera.transform.position.x - cameraHalfWidth - 2f; // 2f on ylim‰‰r‰inen puskuri
            obstacle.GetComponent<ObstacleMovement>().SetDestructionXPosition(destructionPosition);
        }
    }

}
