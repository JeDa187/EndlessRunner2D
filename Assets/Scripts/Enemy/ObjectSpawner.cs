using UnityEngine;
using System.Collections;

public class ObjectSpawner : MonoBehaviour
{
    // An array of game objects that can be spawned
    public GameObject[] objectsToSpawn;

    // The time interval between spawns
    public float spawnInterval = 2.0f;

    // The horizontal offset for spawning objects
    public float spawnOffsetX = 10f;

    // The minimum vertical distance between consecutive spawned objects
    public float minDeltaY = 1f;

    // The vertical position of the last spawned object
    private float lastY;

    void Start()
    {
        // Subscribe to the OnCountdownFinished event
        GameManager.Instance.OnCountdownFinished += StartSpawning;
    }

    public void StartSpawning()
    {
        // Start the SpawnObjects coroutine
        StartCoroutine(SpawnObjects());
    }

    IEnumerator SpawnObjects()
    {
        while (true)
        {
            // Randomly choose an object to spawn from the array
            GameObject objectToSpawn = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];

            // Get the vertical position of the camera's bottom edge
            float minY = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane)).y;

            // Get the vertical position of the camera's top edge
            float maxY = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, Camera.main.nearClipPlane)).y;

            float nextY;

            // Randomly generate a vertical position within the camera's view
            // Ensure that it is sufficiently far from the last spawned object's vertical position
            do
            {
                nextY = Random.Range(minY, maxY);
            }
            while (Mathf.Abs(nextY - lastY) < minDeltaY);

            // Calculate the spawn position
            Vector2 spawnPosition = new Vector2(Camera.main.transform.position.x + Camera.main.orthographicSize * Camera.main.aspect + spawnOffsetX, nextY);

            // Instantiate the object at the spawn position with no rotation
            Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);

            // Save the vertical position of the spawned object
            lastY = nextY;

            // Wait for the specified spawn interval before spawning the next object
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from the OnCountdownFinished event
        GameManager.Instance.OnCountdownFinished -= StartSpawning;
    }
}

