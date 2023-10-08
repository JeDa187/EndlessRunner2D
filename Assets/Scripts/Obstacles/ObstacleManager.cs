using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public GameObject[] downObstaclePrefabs;
    public GameObject[] upObstaclePrefabs;
    public float spawnXPositionOffset = 6f;
    private GroundManager groundManager;

    [SerializeField]
    private float minimumObstacleDistance = 2.0f;  

    private void Awake()
    {
        groundManager = GetComponent<GroundManager>();
    }
    

    private bool IsLocationFree(Vector2 location, GameObject prefab)
    {
        Collider2D collider = prefab.GetComponent<Collider2D>();
        if (collider == null) return true;

        float obstacleWidth = collider.bounds.size.x + minimumObstacleDistance;
        float obstacleHeight = collider.bounds.size.y + minimumObstacleDistance;

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

        int listChoice = Random.Range(0, 2);
        GameObject obstaclePrefab;
        float randomY;

        do
        {
            spawnXPosition = rightmostGround.transform.position.x - groundWidth / 2 + spawnXPositionOffset + Random.Range(0, groundWidth - 2 * spawnXPositionOffset);

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

            currentAttempt++;
        }
        while (!IsLocationFree(new Vector2(spawnXPosition, randomY), obstaclePrefab) && currentAttempt < maxAttempts);

        if (currentAttempt == maxAttempts) return; // Jos emme löydä vapaata paikkaa, lopeta.

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
