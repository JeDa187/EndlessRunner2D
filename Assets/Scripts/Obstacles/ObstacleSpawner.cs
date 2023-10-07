using UnityEngine;
using System.Collections;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] downObstaclePrefabs;
    public GameObject[] upObstaclePrefabs;
    public Camera mainCamera;
    public float obstacleSpawnRate;
    public InfiniteParallaxBackground infiniteParallaxBackground;
    public PolygonCollider2D playerCollider;
    public LayerMask obstacleLayerMask;

    private float gameTime = 0.0f; // Added to track the elapsed game time
    private float decreaseRateInterval = 60.0f; // 60 seconds or 1 minute
    private float decreaseAmount = 1.0f; // Decrease by 1 second

    private float spawnXPositionOffset = 6f;
    private float safeDistance;

    private float? lastDownObstacleY = null;

    private void Start()
    {
        safeDistance = 5 * playerCollider.bounds.size.y; // 5 times the player height as safe distance.
        obstacleSpawnRate = Random.Range(2.0f, 6.0f);
        SpawnObstacle();
        StartCoroutine(SpawnObstacles());

        foreach (var layer in infiniteParallaxBackground.parallaxLayers)
        {
            layer.onLayerShifted += HandleLayerShifted;
        }
    }

    private void HandleLayerShifted(Transform shiftedLayer)
    {
        if (shiftedLayer.parent.CompareTag("Ground_Second"))
        {
            foreach (Transform child in shiftedLayer)
            {
                if (child.CompareTag("Hazard"))
                {
                    Destroy(child.gameObject);
                }
            }
        }
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
    private void SpawnObstacle()
    {
        GameObject rightmostGround = GetRightmostGroundSecond();
        BoxCollider2D groundCollider = rightmostGround.GetComponent<BoxCollider2D>();

        if (groundCollider == null)
            return;

        float groundWidth = groundCollider.size.x * rightmostGround.transform.localScale.x;
        float spawnXPosition = rightmostGround.transform.position.x - groundWidth / 2 + spawnXPositionOffset + Random.Range(0, groundWidth - 2 * spawnXPositionOffset);

        int listChoice = Random.Range(0, 2);
        GameObject obstaclePrefab;
        float randomY;
        Quaternion rotation = Quaternion.identity;

        int flipChance = Random.Range(0, 2);
        bool flipObstacle = flipChance == 1;

        if (listChoice == 0)
        {
            obstaclePrefab = downObstaclePrefabs[Random.Range(0, downObstaclePrefabs.Length)];
            randomY = Random.Range(-12f, -2.85f);

            // Store the Y position of the lower obstacle
            lastDownObstacleY = randomY;

            rotation = flipObstacle ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
        }
        else
        {
            obstaclePrefab = upObstaclePrefabs[Random.Range(0, upObstaclePrefabs.Length)];
            randomY = Random.Range(4f, 13f);

            // If we have the Y position of the lower obstacle, check the gap
            if (lastDownObstacleY.HasValue && randomY - lastDownObstacleY.Value < playerCollider.bounds.size.y * 2)
            {
                return; // If the gap is smaller than double the player height, don't spawn the upper obstacle
            }

            rotation = flipObstacle ? Quaternion.Euler(180, 180, 0) : Quaternion.Euler(180, 0, 0);
        }

        PolygonCollider2D newObstacleCollider = obstaclePrefab.GetComponent<PolygonCollider2D>();
        if (newObstacleCollider == null)
            return;

        float obstacleHeight = newObstacleCollider.bounds.size.y;
        Collider2D[] nearbyObstacles = Physics2D.OverlapCircleAll(new Vector2(spawnXPosition, randomY), obstacleHeight / 2 + safeDistance, obstacleLayerMask);

        if (nearbyObstacles.Length > 0)
        {
            return; // Don't spawn if other obstacles are too close.
        }

        GameObject newObstacle = Instantiate(obstaclePrefab, new Vector2(spawnXPosition, randomY), rotation);
        newObstacle.tag = "Hazard";
        newObstacle.transform.parent = rightmostGround.transform;
    }

    IEnumerator SpawnObstacles()
    {
        while (true)
        {
            yield return new WaitForSeconds(obstacleSpawnRate);

            gameTime += obstacleSpawnRate; // Increase the game time by the spawn rate (this assumes obstacleSpawnRate won't change drastically in short durations)

            if (gameTime >= decreaseRateInterval)
            {
                float maxSpawnRate = Mathf.Clamp(6.0f - (gameTime / decreaseRateInterval) * decreaseAmount, 2.0f, 6.0f); // Decrease the max spawn rate over time
                obstacleSpawnRate = Random.Range(2.0f, maxSpawnRate);
                gameTime -= decreaseRateInterval; // Reset the interval
            }
            else
            {
                obstacleSpawnRate = Random.Range(2.0f, 6.0f);
            }

            SpawnObstacle();
        }
    }

    private void OnDestroy()
    {
        foreach (var layer in infiniteParallaxBackground.parallaxLayers)
        {
            layer.onLayerShifted -= HandleLayerShifted;
        }
    }
}
