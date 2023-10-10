using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    private void Update()
    {
        // Move the object to the left
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        // Check if the object has gone off the left side of the screen
        Vector2 screenPosition = Camera.main.WorldToViewportPoint(transform.position);

        // If the object's viewport position is less than -0.1 in the x axis (off the left side of the screen)
        if (screenPosition.x < -0.1)
        {
            ReturnToPool();
        }
    }

    public void SetSpeed(float cameraSpeed)
    {
        // Modify moveSpeed based on camera speed.
        moveSpeed = moveSpeed * cameraSpeed * 0.5f;
    }

    private void ReturnToPool()
    {
        EnemyPooler.Instance.ReturnToPool(gameObject.tag, gameObject);
    }
}
