using UnityEngine;
using TMPro;

public class DragonflyController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 12f;              // Force applied when dragonfly jumps

    private bool canMove = false;                                // Flag to check if dragonfly can move
    private Rigidbody2D rb;                                      // Rigidbody2D component for physics
    private Vector2 originalPosition;                            // Original position of dragonfly for resetting
    private Renderer rend;                                       // Renderer component for bounds checking

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Initialize the Rigidbody2D component here instead of Start
    }

    private void Start()
    {
        originalPosition = transform.position; // Save the original position
        rend = GetComponent<Renderer>(); // Get the Renderer component
    }

    private void Update()
    {
        if (!canMove) return;                                    // If canMove is false, exit the function

        HandleInput();                                           // Check and process player input
        CheckOutOfScreenBounds();                                // Check if dragonfly is out of screen bounds
    }

    private void HandleInput()
    {
        // Check for mouse click or touch input
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            rb.velocity = Vector2.up * jumpForce;                // Apply jump force upwards
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Tarkista, onko törmäys tapahtunut objektin kanssa, joka voi tuhota hahmon
        if (collision.gameObject.CompareTag("Hazard"))
        {
            GameManager.Instance.GameOver();
            Destroy(gameObject);
        }
    }

    private void CheckOutOfScreenBounds()
    {
        // Convert object bounds to viewport positions
        Vector2 viewportPositionMin = Camera.main.WorldToViewportPoint(rend.bounds.min);
        Vector2 viewportPositionMax = Camera.main.WorldToViewportPoint(rend.bounds.max);

        // Get the height of the object in viewport coordinates
        float objectHeightInViewport = viewportPositionMax.y - viewportPositionMin.y;

        // If dragonfly is more than half outside the screen vertically
        if (viewportPositionMax.y < 0.5f * objectHeightInViewport || viewportPositionMin.y > 1 - 0.5f * objectHeightInViewport)
        {
            GameManager.Instance.GameOver(); // End the game
            Destroy(gameObject); // Destroy the dragonfly game object
        }
    }

    public void ToggleRigidbodyMovement(bool allowMovement)
    {
        canMove = allowMovement;                                 // Set the movement flag
        rb.isKinematic = !allowMovement;                         // Toggle kinematic state based on movement

        if (!allowMovement)
        {
            rb.velocity = Vector2.zero;                          // Stop any movement
        }
    }

    public void ResetDragonfly()
    {
        gameObject.SetActive(true);                              // Activate the dragonfly object
        transform.position = originalPosition;                   // Reset position
        rb.velocity = Vector2.zero;                              // Stop any movement
    }
}
