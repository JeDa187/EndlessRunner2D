using UnityEngine;
using System.Collections.Generic;

public class ObstaclePooler : MonoBehaviour
{
    public GameObject[] downObstaclePrefabs;
    public GameObject[] upObstaclePrefabs;
    public GameObject[] downObstacle2Prefabs;
    public GameObject[] upObstacle2Prefabs;
    public float spawnXPositionOffset = 6f;

    private GroundManager groundManager;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    private const string TAG_OBSTACLE_DOWN1 = "ObstacleDown1";
    private const string TAG_OBSTACLE_UP1 = "ObstacleUp1";
    private const string TAG_OBSTACLE_DOWN2 = "ObstacleDown2";
    private const string TAG_OBSTACLE_UP2 = "ObstacleUp2";

    #region Singleton
    public static ObstaclePooler Instance;

    private void Awake()
    {
        Instance = this;
        groundManager = GetComponent<GroundManager>();
        InitializePool();
    }
    #endregion

    void InitializePool()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        // Initialising pools from predefined obstacle arrays
        InitializeObstaclePool(downObstaclePrefabs, TAG_OBSTACLE_DOWN1);
        InitializeObstaclePool(upObstaclePrefabs, TAG_OBSTACLE_UP1);
        InitializeObstaclePool(downObstacle2Prefabs, TAG_OBSTACLE_DOWN2);
        InitializeObstaclePool(upObstacle2Prefabs, TAG_OBSTACLE_UP2);
    }

    void InitializeObstaclePool(GameObject[] obstaclePrefabs, string tag)
    {
        Queue<GameObject> objectPool = new Queue<GameObject>();

        for (int i = 0; i < obstaclePrefabs.Length; i++)
        {
            for (int j = 0; j < 2; j++)  // Lisätään jokainen este kaksi kertaa
            {
                GameObject obj = Instantiate(obstaclePrefabs[i]);
                obj.transform.SetParent(this.transform);  // Asettaa objektin `ObstaclePooler`-objektin lapseksi
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
        }

        poolDictionary.Add(tag, objectPool);
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
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

        if (listChoice == 1 || listChoice == 3)
        {
            rotation *= Quaternion.Euler(0, 0, 180);
        }

        string poolTag = GetPoolTag(listChoice);
        GameObject newObstacle = SpawnFromPool(poolTag, new Vector2(spawnXPosition, randomY), rotation);
        newObstacle.transform.parent = rightmostGround.transform;
    }

    public void ReturnToPool(string tag, GameObject obj)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return;
        }

        obj.SetActive(false);
        obj.transform.SetParent(this.transform); // Ensure that the obstacle is returned under the correct parent
        poolDictionary[tag].Enqueue(obj);
    }

    private string GetPoolTag(int listChoice)
    {
        switch (listChoice)
        {
            case 0: return TAG_OBSTACLE_DOWN1;
            case 1: return TAG_OBSTACLE_UP1;
            case 2: return TAG_OBSTACLE_DOWN2;
            case 3: return TAG_OBSTACLE_UP2;
            default: return "";
        }
    }
}
