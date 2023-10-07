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

    private float spawnXPositionOffset = 6f;
    private float safeDistance;
    private GameObject lastRightmostGround;

    private void Start()
    {
        safeDistance = 10 * playerCollider.bounds.extents.y;
        obstacleSpawnRate = Random.Range(2.0f, 6.0f);
        SpawnObstacle();
        StartCoroutine(SpawnObstacles());

        // Tilaa tapahtuma
        foreach (var layer in infiniteParallaxBackground.parallaxLayers)
        {
            layer.onLayerShifted += HandleLayerShifted;
        }
    }

    private void HandleLayerShifted(Transform shiftedLayer)
    {
        // Tarkista, onko siirretty kerros oikea
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

        // T‰m‰ lis‰ttiin, satunnainen mahdollisuus k‰‰nt‰‰ esteit‰ y-akselin ymp‰ri
        int flipChance = Random.Range(0, 2);
        bool flipObstacle = flipChance == 1;

        if (listChoice == 0)
        {
            obstaclePrefab = downObstaclePrefabs[Random.Range(0, downObstaclePrefabs.Length)];
            randomY = Random.Range(-12f, -2.85f);
            rotation = flipObstacle ? Quaternion.Euler(0, 180, 0) : Quaternion.identity; // Jos flipObstacle on tosi, k‰‰nnet‰‰n este y-akselin ymp‰ri
        }
        else
        {
            obstaclePrefab = upObstaclePrefabs[Random.Range(0, upObstaclePrefabs.Length)];
            randomY = Random.Range(4f, 13f);
            rotation = flipObstacle ? Quaternion.Euler(180, 180, 0) : Quaternion.Euler(180, 0, 0); // Samoin t‰‰ll‰, mutta ottaen huomioon, ett‰ upObstacle on jo k‰‰ntynyt 180 astetta
        }

        Collider2D[] nearbyObstacles = Physics2D.OverlapBoxAll(new Vector2(spawnXPosition, randomY), new Vector2(1f, safeDistance), 0f, obstacleLayerMask);
        foreach (Collider2D nearbyObstacle in nearbyObstacles)
        {
            if (Mathf.Abs(nearbyObstacle.bounds.min.y - randomY) < safeDistance || Mathf.Abs(nearbyObstacle.bounds.max.y - randomY) < safeDistance)
            {
                return;
            }
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
            obstacleSpawnRate = Random.Range(2.0f, 6.0f);
            SpawnObstacle();
        }
    }

    private void OnDestroy()
    {
        // Poista tapahtumankuuntelija, kun objekti tuhoutuu
        foreach (var layer in infiniteParallaxBackground.parallaxLayers)
        {
            layer.onLayerShifted -= HandleLayerShifted;
        }
    }

}
