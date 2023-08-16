using UnityEngine;
using System.Collections;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject[] objectsToSpawn;
    public float spawnInterval = 2.0f;
    public float spawnOffsetX = 10f;
    public float minDeltaY = 1f;  // Minimietäisyys edellisestä korkeudesta

    private float lastY;  // Viimeksi spawnatun objektin korkeus

    void Start()
    {
        GameManager.Instance.OnCountdownFinished += StartSpawning;
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnObjects());
    }

    IEnumerator SpawnObjects()
    {
        while (true)
        {
            GameObject objectToSpawn = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];

            float minY = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane)).y;  // Kameran alareuna
            float maxY = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, Camera.main.nearClipPlane)).y;  // Kameran yläreuna

            float nextY;
            do
            {
                nextY = Random.Range(minY, maxY);
            }
            while (Mathf.Abs(nextY - lastY) < minDeltaY);  // Varmistetaan, että seuraava korkeus on riittävän kaukana edellisestä

            Vector2 spawnPosition = new Vector2(Camera.main.transform.position.x + Camera.main.orthographicSize * Camera.main.aspect + spawnOffsetX, nextY);
            Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);

            lastY = nextY;  // Tallennetaan viimeksi spawnatun objektin korkeus

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void OnDestroy()
    {
        GameManager.Instance.OnCountdownFinished -= StartSpawning;
    }
}
