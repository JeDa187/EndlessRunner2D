using UnityEngine;
using TMPro;

public class DragonflyController : MonoBehaviour
{
    public float jumpForce = 12f;
    private Rigidbody2D rb;
    private Vector2 originalPosition; // Store the original position
    private Renderer rend; // Reference to the character's Renderer

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalPosition = transform.position; // Save the original position
        rend = GetComponent<Renderer>(); // Get the Renderer component
    }

    private void Update()
    {
        // Check if the screen has been touched or clicked
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            rb.velocity = Vector2.up * jumpForce;
        }

        // Convert world coordinates to viewport coordinates
        Vector2 viewportPositionMin = Camera.main.WorldToViewportPoint(rend.bounds.min);
        Vector2 viewportPositionMax = Camera.main.WorldToViewportPoint(rend.bounds.max);

        // Check if the character is completely outside the screen bounds
        if (viewportPositionMax.y < 0 || viewportPositionMin.y > 1)
        {
            GameManager.Instance.GameOver();
        }
    }

    public void ResetDragonfly()
    {
        transform.position = originalPosition; // Reset the position
        rb.velocity = Vector2.zero; // Reset velocity
    }
}
