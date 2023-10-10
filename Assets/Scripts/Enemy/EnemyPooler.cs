using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyPooler : MonoBehaviour
{
    // Class to represent individual pools of game objects.
    [System.Serializable]
    public class Pool
    {
        public string tag;      // Identifier for the pool.
        public GameObject prefab; // The prefab for the objects in this pool.
        public int size;        // The number of objects in this pool.
    }

    public List<Pool> pools;   // List of all available object pools.
    private Dictionary<string, Queue<GameObject>> poolDictionary; // Dictionary to quickly access object pools by tag.

    [SerializeField] private float initialSpawnInterval = 2.0f; // Initial interval between spawns.
    [SerializeField] private float spawnIntervalMin = 2.0f;     // Minimum interval between spawns.
    [SerializeField] private float spawnIntervalMax = 4.0f;     // Maximum interval between spawns.
    private float currentSpawnInterval;                         // Current interval between spawns.

    [SerializeField] private float spawnOffsetX = 10f;          // Horizontal offset for spawning objects.
    [SerializeField] private float minDeltaY = 1f;              // Minimum vertical difference between consecutive spawns.
    [SerializeField] private float minY = -7f;                  // Minimum vertical position for spawning.
    [SerializeField] private float maxY = 7.5f;                 // Maximum vertical position for spawning.

    private float lastY;          // Last vertical position where an object was spawned.
    private Camera mainCamera;    // Reference to the main camera.

    // Singleton pattern to make sure there's only one instance of this class.
    public static EnemyPooler Instance;

    private void Awake()
    {
        // Singleton initialization.
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("More than one instance of UnifiedObjectPooler found!");
            Destroy(gameObject);
            return;
        }

        mainCamera = Camera.main;  // Get reference to the main camera.
        InitializePools();         // Initialize all object pools.
    }

    // Create object pools based on provided definitions.
    private void InitializePools()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                obj.transform.SetParent(this.transform);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    void Start()
    {
        currentSpawnInterval = initialSpawnInterval;
        // Subscribe to the OnCountdownFinished event from GameManager
        if (GameManager.Instance != null)
            GameManager.Instance.OnCountdownFinished += BeginSpawning;
    }

    void OnDestroy()
    {
        // Unsubscribe when the ObjectPooler is destroyed or disabled
        if (GameManager.Instance != null)
            GameManager.Instance.OnCountdownFinished -= BeginSpawning;
    }

    void BeginSpawning()
    {
        StartCoroutine(SpawnObjects());
    }


    // Coroutine to spawn objects.
    IEnumerator SpawnObjects()
    {
        while (true)
        {
            // Choose a random object pool.
            string tag = pools[Random.Range(0, pools.Count)].tag;

            // Choose a vertical position for the spawn, ensuring some vertical difference from the last spawn.
            float nextY;
            do
            {
                nextY = Random.Range(minY, maxY);
            }
            while (Mathf.Abs(nextY - lastY) < minDeltaY);

            Vector2 spawnPosition = new Vector2(mainCamera.transform.position.x +
                mainCamera.orthographicSize * mainCamera.aspect + spawnOffsetX, nextY);

            SpawnFromPool(tag, spawnPosition, Quaternion.identity);

            lastY = nextY;  // Update the last spawn position.

            currentSpawnInterval = Random.Range(spawnIntervalMin, spawnIntervalMax); // Choose next spawn interval.
            yield return new WaitForSeconds(currentSpawnInterval);
        }
    }

    // Spawn an object from a specific pool.
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        return objectToSpawn;
    }

    // Return an object to its pool.
    public void ReturnToPool(string tag, GameObject objectToReturn)
    {
        objectToReturn.SetActive(false);
        if (poolDictionary.ContainsKey(tag))
        {
            poolDictionary[tag].Enqueue(objectToReturn);
        }
        else
        {
            Debug.LogWarning($"Trying to return an object to a pool with tag {tag} that doesn't exist.");
        }
    }
}
