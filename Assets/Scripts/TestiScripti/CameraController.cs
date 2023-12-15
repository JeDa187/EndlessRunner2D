using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool enableScrolling = true;
    public float accelerationFactor;

    private float cameraSpeed = 1.5f;
    private Transform mainCamera;
    private Vector3 cameraMoveVector;

    public void Initialize()
    {
        mainCamera = Camera.main.transform;
        cameraMoveVector = new Vector3(cameraSpeed * Time.deltaTime, 0, 0);
    }

    public void UpdateCamera()
    {
        if (Time.timeScale == 0) return;

        if (enableScrolling)
        {
            cameraSpeed += accelerationFactor * Time.deltaTime;
            mainCamera.position += cameraMoveVector;
        }
    }
    public Vector3 GetCameraPosition()
    {
        return transform.position;
    }

    public float GetCameraSpeed()
    {
        return cameraSpeed;
    }

    public void SetCameraSpeed(float speed)
    {
        cameraSpeed = speed;
    }
}
