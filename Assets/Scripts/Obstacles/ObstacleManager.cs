using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public GameObject[] downObstaclePrefabs;
    public GameObject[] upObstaclePrefabs;
    public float spawnXPositionOffset = 6f;
    private GroundManager groundManager;

    private void Awake()
    {
        groundManager = GetComponent<GroundManager>();
    }

    public void SpawnObstacle()
    {
        GameObject rightmostGround = groundManager.GetRightmostGroundSecond();
        BoxCollider2D groundCollider = rightmostGround.GetComponent<BoxCollider2D>();

        if (groundCollider == null) return;

        float groundWidth = groundCollider.size.x * rightmostGround.transform.localScale.x;
        float spawnXPosition = rightmostGround.transform.position.x - groundWidth / 2 + spawnXPositionOffset + Random.Range(0, groundWidth - 2 * spawnXPositionOffset);

        int listChoice = Random.Range(0, 2);
        GameObject obstaclePrefab;
        float randomY;

        if (listChoice == 0)
        {
            obstaclePrefab = downObstaclePrefabs[Random.Range(0, downObstaclePrefabs.Length)];
            randomY = Random.Range(-12f, -2.85f);
        }
        else
        {
            obstaclePrefab = upObstaclePrefabs[Random.Range(0, upObstaclePrefabs.Length)];
            randomY = Random.Range(4f, 13f);
        }

        Quaternion rotation = Quaternion.identity;

        if (listChoice == 1)
        {
            rotation = Quaternion.Euler(0, 0, 180);
        }

        GameObject newObstacle = Instantiate(obstaclePrefab, new Vector2(spawnXPosition, randomY), rotation);

        newObstacle.tag = "Hazard";
        newObstacle.transform.parent = rightmostGround.transform;
    }
}
