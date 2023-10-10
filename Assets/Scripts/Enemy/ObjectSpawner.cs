using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyToSpawn;
    [SerializeField] private float initialSpawnInterval = 2.0f;
    [SerializeField] private float spawnIntervalMin = 2.0f;
    [SerializeField] private float spawnIntervalMax = 4.0f;
    [SerializeField] private float currentSpawnInterval;
    [SerializeField] private float spawnOffsetX = 10f;
    [SerializeField] private float minDeltaY = 1f;
    [SerializeField] private float minY = -7f;
    [SerializeField] private float maxY = 7.5f;
    [SerializeField] private int poolSize = 10;

    private float lastY;
    private GameObject lastSpawnedEnemy = null;
    private InfiniteParallaxBackground parallaxBackground;
    private Camera mainCamera;
    private Dictionary<GameObject, Queue<GameObject>> objectPools;

    #region Singleton
    public static ObjectSpawner Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("More than one instance of ObjectSpawner found!");
            Destroy(gameObject);
            return;
        }

        mainCamera = Camera.main;
        parallaxBackground = FindObjectOfType<InfiniteParallaxBackground>();
        InitializeObjectPools();
    }
    #endregion

    private void InitializeObjectPools()
    {
        objectPools = new Dictionary<GameObject, Queue<GameObject>>();

        foreach (var enemy in enemyToSpawn)
        {
            Queue<GameObject> poolQueue = new Queue<GameObject>();
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(enemy);
                obj.SetActive(false);
                poolQueue.Enqueue(obj);
            }
            objectPools.Add(enemy, poolQueue);
        }
    }

    void Start()
    {
        GameManager.Instance.OnCountdownFinished += StartSpawning;
        currentSpawnInterval = initialSpawnInterval;
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnObjects());
    }

    IEnumerator SpawnObjects()
    {
        while (true)
        {
            GameObject objectToSpawn;

            do
            {
                objectToSpawn = enemyToSpawn[Random.Range(0, enemyToSpawn.Length)];
            } while (objectToSpawn == lastSpawnedEnemy);

            lastSpawnedEnemy = objectToSpawn;

            float nextY;
            do
            {
                nextY = Random.Range(minY, maxY);
            }
            while (Mathf.Abs(nextY - lastY) < minDeltaY);

            Vector2 spawnPosition = new Vector2(mainCamera.transform.position.x +
                mainCamera.orthographicSize * mainCamera.aspect + spawnOffsetX, nextY);

            GameObject spawnedObject = GetObjectFromPool(objectToSpawn);
            if (spawnedObject == null) continue;

            spawnedObject.transform.position = spawnPosition;
            spawnedObject.transform.rotation = Quaternion.identity;
            spawnedObject.SetActive(true);

            EnemyMover mover = spawnedObject.GetComponent<EnemyMover>();
            if (mover)
            {
                mover.SetSpeed(parallaxBackground.CameraSpeed);
            }

            lastY = nextY;

            currentSpawnInterval = Random.Range(spawnIntervalMin, spawnIntervalMax);
            yield return new WaitForSeconds(currentSpawnInterval);
        }
    }

    private GameObject GetObjectFromPool(GameObject objectToSpawn)
    {
        if (!objectPools.ContainsKey(objectToSpawn) || objectPools[objectToSpawn].Count == 0)
        {
            Debug.LogWarning("No objects left in pool for " + objectToSpawn.name);
            return null;
        }

        GameObject objectFromPool = objectPools[objectToSpawn].Dequeue();
        return objectFromPool;
    }

    public void ReturnObjectToPool(GameObject objectToReturn)
    {
        objectToReturn.SetActive(false);
        foreach (var item in objectPools)
        {
            if (item.Key.name == objectToReturn.name)
            {
                item.Value.Enqueue(objectToReturn);
                return;
            }
        }
    }

    void OnDestroy()
    {
        GameManager.Instance.OnCountdownFinished -= StartSpawning;
    }
}
