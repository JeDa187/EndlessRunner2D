using UnityEngine;
using System.Collections;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemyToSpawn;
    [SerializeField] float initialSpawnInterval = 2.0f;
    [SerializeField] float spawnIntervalMin = 2.0f;
    [SerializeField] float spawnIntervalMax = 4.0f;
    [SerializeField] float currentSpawnInterval;
    [SerializeField] float spawnOffsetX = 10f;
    [SerializeField] float minDeltaY = 1f;
    [SerializeField] float minY = -7f;  // Alin mahdollinen korkeus
    [SerializeField] float maxY = 7.5f;  // Ylin mahdollinen korkeus

    private float lastY;
    private InfiniteParallaxBackground parallaxBackground;  // Lis‰tty t‰h‰n viittaus InfiniteParallaxBackground-objektiin

    void Start()
    {
        GameManager.Instance.OnCountdownFinished += StartSpawning;
        currentSpawnInterval = initialSpawnInterval;
        parallaxBackground = FindObjectOfType<InfiniteParallaxBackground>();  // Haetaan viittaus InfiniteParallaxBackground-objektiin
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnObjects());
    }

    IEnumerator SpawnObjects()
    {
        while (true)
        {
            GameObject objectToSpawn = enemyToSpawn[Random.Range(0, enemyToSpawn.Length)];

            float nextY;
            do
            {
                nextY = Random.Range(minY, maxY);
            }
            while (Mathf.Abs(nextY - lastY) < minDeltaY);

            Vector2 spawnPosition = new Vector2(Camera.main.transform.position.x +
                Camera.main.orthographicSize * Camera.main.aspect + spawnOffsetX, nextY);

            GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
            ObjectMover mover = spawnedObject.GetComponent<ObjectMover>();
            if (mover)
            {
                mover.SetSpeed(parallaxBackground.CameraSpeed);  // K‰ytet‰‰n haettua viittausta
            }

            lastY = nextY;

            currentSpawnInterval = Random.Range(spawnIntervalMin, spawnIntervalMax);
            yield return new WaitForSeconds(currentSpawnInterval);
        }
    }

    void OnDestroy()
    {
        GameManager.Instance.OnCountdownFinished -= StartSpawning;
    }
}
