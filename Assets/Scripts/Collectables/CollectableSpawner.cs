using UnityEngine;
using System.Collections;

public class CollectableSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] collectablesToSpawn;
    [SerializeField] float spawnInterval = 10.0f;
    [SerializeField] float spawnOffsetX = 10f;
    [SerializeField] float minDeltaY = 1f;  // Minimiet�isyys edellisest� korkeudesta

    private float lastY;  // Viimeksi spawnatun objektin korkeus

    void Start()
    {
        GameManager.Instance.OnCountdownFinished += SpawnCollectables;
    }

    public void SpawnCollectables()
    {
        StartCoroutine(Spawner());
    }

    IEnumerator Spawner()
    {
        while (true)
        {
            GameObject objectToSpawn = collectablesToSpawn[Random.Range(0, collectablesToSpawn.Length)];

            float minY = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane)).y;  // Kameran alareuna
            float maxY = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, Camera.main.nearClipPlane)).y;  // Kameran yl�reuna

            float nextY;
            do
            {
                nextY = Random.Range(minY, maxY);
            }
            while (Mathf.Abs(nextY - lastY) < minDeltaY);  // Varmistetaan, ett� seuraava korkeus on riitt�v�n kaukana edellisest�

            Vector2 spawnPosition = new Vector2(Camera.main.transform.position.x +
                Camera.main.orthographicSize * Camera.main.aspect + spawnOffsetX, nextY);
            Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);

            lastY = nextY;  // Tallennetaan viimeksi spawnatun objektin korkeus

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void OnDestroy()
    {
        GameManager.Instance.OnCountdownFinished -= SpawnCollectables;
    }
}
