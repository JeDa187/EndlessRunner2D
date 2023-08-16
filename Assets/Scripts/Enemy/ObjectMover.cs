using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public float moveSpeed = 5.0f; // Speed at which the object moves

    void Update()
    {
        // Move the object to the left
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        // Check if the object has gone off the left side of the screen
        Vector2 screenPosition = Camera.main.WorldToViewportPoint(transform.position);
        if (screenPosition.x < -0.1)
        {
            // Destroy the object
            Destroy(gameObject);
        }
    }
}
