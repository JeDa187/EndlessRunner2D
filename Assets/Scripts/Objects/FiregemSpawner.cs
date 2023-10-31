using UnityEngine;
using System.Collections;

public class FiregemSpawner : MonoBehaviour
{
    public GameObject firegemPrefab; // Firegemin prefab.
    private float initialWaitTime = 30f; // Ensimm�inen odotusaika (30 sekuntia).
    private float minSpawnInterval = 20f; // Minimi aika seuraavan Firegemin spawnaukseen.
    private float maxSpawnInterval = 40f; // Maksimi aika seuraavan Firegemin spawnaukseen.

    public InfiniteParallaxBackground parallaxBackground; // Viittaus InfiniteParallaxBackground -komponenttiin.

    private void Start()
    {
        // Aloita odottaminen 30 sekuntia ja sen j�lkeen aloita Firegemien spawnaus.
        StartCoroutine(SpawnFiregems());

        // Liitt�� FiregemSpawner:n InfiniteParallaxBackground:in tapahtumiin.
        foreach (var layer in parallaxBackground.parallaxLayers)
        {
            layer.LayerShifted += HandleLayerShifted;
        }
    }

    private IEnumerator SpawnFiregems()
    {
        yield return new WaitForSeconds(initialWaitTime);

        while (true)
        {
            SpawnFiregem();

            float randomWaitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(randomWaitTime);
        }
    }

    private void SpawnFiregem()
    {
        // Etsit��n objekti tagilla "Ground_Second".
        GameObject[] grounds = GameObject.FindGameObjectsWithTag("Ground_Second");
        if (grounds.Length == 0)
        {
            Debug.LogError("No object with tag 'Ground_Second' found in the scene.");
            return;
        }

        // Otetaan oikeanpuoleisin objekti, jos on useita.
        GameObject rightmostGround = grounds[0];
        foreach (var ground in grounds)
        {
            if (ground.transform.position.x > rightmostGround.transform.position.x)
            {
                rightmostGround = ground;
            }
        }

        Vector3 spawnPosition;

        while (true)
        {
            spawnPosition = rightmostGround.transform.position + new Vector3(15f, Random.Range(4f, 19f), 0);

            // Tarkista, onko spawn-paikassa t�rm�yst� (esim. este)
            Collider2D hitCollider = Physics2D.OverlapCircle(spawnPosition, 0.5f); // Voit s��t�� '0.5f' arvoa tarvittaessa

            // Jos t�rm�yst� ei ole, lopetetaan loop
            if (hitCollider == null)
                break;
        }

        GameObject spawnedGem = Instantiate(firegemPrefab, spawnPosition, Quaternion.identity);
        spawnedGem.transform.SetParent(rightmostGround.transform);  // T�m� rivi asettaa spawnatun FireGemin Ground_Second objektin lapseksi.
    }


    private void HandleLayerShifted(Transform shiftedLayer)
    {
        // Tuhotaan kaikki Firegemit, jotka ovat t�m�n shiftedLayerin lapsia.
        foreach (Transform child in shiftedLayer)
        {
            if (child.CompareTag("FireGem"))
            {
                Destroy(child.gameObject);
            }
        }
    }
}
