using System.Collections;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField]
    private float minSpawnRate = 2.0f;
    [SerializeField]
    private float maxSpawnRate = 6.0f;

    private ObstaclePooler obstaclePooler; // P‰ivitetty viittaus

    public InfiniteParallaxBackground backgroundScroller;

    private const float CAMERA_SPEED_LIMIT_FOR_SPAWN_ADJUSTMENT = 8.0f; // M‰‰ritelty raja, jonka j‰lkeen spawn-tahti ei en‰‰ nopeudu

    private void Start()
    {
        obstaclePooler = GetComponent<ObstaclePooler>(); // P‰ivitetty viittaus
        obstaclePooler.SpawnObstacle();
        StartCoroutine(SpawnObstacles());
    }

    IEnumerator SpawnObstacles()
    {
        while (true)
        {
            float currentCameraSpeed = Mathf.Min(backgroundScroller.CameraSpeed, CAMERA_SPEED_LIMIT_FOR_SPAWN_ADJUSTMENT); // Spawn-tahti ei nopeudu rajoitteen j‰lkeen
            float cameraSpeedModifier = currentCameraSpeed / 1.5f;
            float adjustedMinSpawnRate = minSpawnRate / cameraSpeedModifier;
            float adjustedMaxSpawnRate = maxSpawnRate / cameraSpeedModifier;

            float timeToNextSpawn = Random.Range(adjustedMinSpawnRate, adjustedMaxSpawnRate);

            yield return new WaitForSeconds(timeToNextSpawn);
            obstaclePooler.SpawnObstacle(); // P‰ivitetty viittaus
        }
    }

}
