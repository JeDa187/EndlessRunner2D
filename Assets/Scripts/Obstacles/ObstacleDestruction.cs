using UnityEngine;

public class ObstacleDestruction : MonoBehaviour
{
    public Camera mainCamera;
    public ObstaclePooler obstaclePooler;

    private void LateUpdate()
    {
        float cameraLeftEdge = mainCamera.transform.position.x - (mainCamera.orthographicSize * mainCamera.aspect);

        if (transform.position.x < cameraLeftEdge)
        {
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        gameObject.SetActive(false);

        // Käytämme ReturnToPool-metodia ObstaclePooler-luokassa.
        obstaclePooler.ReturnToPool(gameObject.tag, gameObject);
    }
}
