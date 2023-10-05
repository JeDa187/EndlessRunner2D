using UnityEngine;

public class ObstacleDestruction : MonoBehaviour
{
    public Camera mainCamera;

    void Update()
    {
        float cameraLeftEdge = mainCamera.transform.position.x - (mainCamera.orthographicSize * mainCamera.aspect);

        if (transform.position.x < cameraLeftEdge)
        {
            Destroy(gameObject);
        }
    }
}
