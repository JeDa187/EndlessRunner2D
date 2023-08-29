using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed = 1.0f;

    private void Update()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {
        Vector3 movement = new Vector3(speed * Time.deltaTime, 0, 0);
        transform.position += movement;
    }
}

