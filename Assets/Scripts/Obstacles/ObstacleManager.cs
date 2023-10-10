using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public GameObject[] downObstaclePrefabs;
    public GameObject[] upObstaclePrefabs;
    public GameObject[] downObstacle2Prefabs;
    public GameObject[] upObstacle2Prefabs;
    public float spawnXPositionOffset = 6f;
    private GroundManager groundManager;

    private void Awake()
    {
        groundManager = GetComponent<GroundManager>();
    }

    private bool IsLocationFree(Vector2 location, GameObject[] prefabs)
    {
        foreach (GameObject prefab in prefabs)
        {
            if (!IsLocationFreeForPrefab(location, prefab))
                return false;
        }

        return true;
    }

    private bool IsLocationFreeForPrefab(Vector2 location, GameObject prefab)
    {
        Collider2D collider = prefab.GetComponent<Collider2D>();
        if (collider == null) return true;

        float obstacleWidth = collider.bounds.size.x + 15;
        float obstacleHeight = collider.bounds.size.y + 30;

        Collider2D hit = Physics2D.OverlapBox(location, new Vector2(obstacleWidth, obstacleHeight), 0, LayerMask.GetMask("Obstacle"));

        return hit == null;
    }

    public void SpawnObstacle()
    {
        GameObject rightmostGround = groundManager.GetRightmostGroundSecond();
        BoxCollider2D groundCollider = rightmostGround.GetComponent<BoxCollider2D>();

        if (groundCollider == null) return;

        float groundWidth = groundCollider.size.x * rightmostGround.transform.localScale.x;
        float spawnXPosition;
        int maxAttempts = 10;
        int currentAttempt = 0;

        int listChoice = Random.Range(0, 4);
        GameObject obstaclePrefab;
        float randomY;

        GameObject[][] obstacleGroups =
        {
            downObstaclePrefabs, upObstaclePrefabs, downObstacle2Prefabs, upObstacle2Prefabs
        };

        float[][] yRangeGroups =
        {
            new float[] {-12f, -2.85f},
            new float[] {4f, 13f},
            new float[] {-7.5f, -4.8f},
            new float[] {7.1f, 8.5f}
        };

        do
        {
            spawnXPosition = rightmostGround.transform.position.x - groundWidth / 2 + spawnXPositionOffset + Random.Range(0, groundWidth - 2 * spawnXPositionOffset);
            obstaclePrefab = obstacleGroups[listChoice][Random.Range(0, obstacleGroups[listChoice].Length)];
            randomY = Random.Range(yRangeGroups[listChoice][0], yRangeGroups[listChoice][1]);
            currentAttempt++;
        }
        while (!IsLocationFree(new Vector2(spawnXPosition, randomY), obstacleGroups[listChoice]) && currentAttempt < maxAttempts);

        if (currentAttempt == maxAttempts)
        {
            return;
        }

        Quaternion rotation = Random.Range(0, 2) == 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;

        bool isUpObstacle2Index2 = (listChoice == 3 && obstaclePrefab == upObstacle2Prefabs[2]);

        if ((listChoice == 1) || (listChoice == 3 && !isUpObstacle2Index2))
        {
            rotation *= Quaternion.Euler(0, 0, 180);
        }

        GameObject newObstacle = Instantiate(obstaclePrefab, new Vector2(spawnXPosition, randomY), rotation);
        newObstacle.tag = "Hazard";
        newObstacle.transform.parent = rightmostGround.transform;
    }
}
