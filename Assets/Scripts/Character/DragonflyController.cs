using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class DragonflyController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 12f; // Force applied when the dragonfly jumps

    private bool canMove = false; // Flag to check if the dragonfly can move
    private Rigidbody2D rb;       // Rigidbody2D component for physics
    private Vector2 originalPosition; // Original position of the dragonfly for resetting
    private Renderer rend;            // Renderer component for bounds checking

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
        if (!canMove) return; // If canMove is false, exit the function

        HandleInput();        // Check and process player input
        CheckOutOfScreenBounds(); // Check if the dragonfly is out of screen bounds
    }

    private void HandleInput()
    {
        // Tarkista, onko hiiri tai kosketus UI-elementin päällä
        if (EventSystem.current.IsPointerOverGameObject() || IsTouchOverUI())
        {
            return; // Jos on, älä suorita seuraavaa koodia
        }

        // Tarkista hiiren napsautus tai kosketussyöte
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            rb.velocity = Vector2.up * jumpForce; // Soveltaa hyppyvoimaa ylöspäin
        }
    }

    private bool IsTouchOverUI()
    {
        if (Input.touchCount > 0)
        {
            // Tarkista ensimmäinen kosketus (voit myös käydä läpi kaikki kosketukset tarvittaessa)
            Touch touch = Input.GetTouch(0);
            int touchID = touch.fingerId;

            // Tarkista, onko kosketus UI-elementin päällä
            return EventSystem.current.IsPointerOverGameObject(touchID);
        }

        return false;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if collision occurred with an object that can destroy the character
        if (collision.gameObject.CompareTag("Hazard"))
        {
            GameManager.Instance.GameOver(); // End the game
            Destroy(gameObject);            // Destroy the dragonfly game object
        }
    }

    private void CheckOutOfScreenBounds()
    {
        // Convert object bounds to viewport positions
        Vector2 viewportPositionMin = Camera.main.WorldToViewportPoint(rend.bounds.min);
        Vector2 viewportPositionMax = Camera.main.WorldToViewportPoint(rend.bounds.max);

        // Get the height of the object in viewport coordinates
        float objectHeightInViewport = viewportPositionMax.y - viewportPositionMin.y;

        // If the dragonfly is more than half outside the screen vertically
        if (viewportPositionMax.y < 0.5f * objectHeightInViewport || viewportPositionMin.y > 1 - 0.5f * objectHeightInViewport)
        {
            GameManager.Instance.GameOver(); // End the game
            Destroy(gameObject); // Destroy the dragonfly game object
        }
    }

    public void ToggleRigidbodyMovement(bool allowMovement)
    {
        canMove = allowMovement; // Set the movement flag
        rb.isKinematic = !allowMovement; // Toggle kinematic state based on movement

        if (!allowMovement)
        {
            rb.velocity = Vector2.zero; // Stop any movement
        }
    }

    public void ResetDragonfly()
    {
        gameObject.SetActive(true); // Activate the dragonfly object
        transform.position = originalPosition; // Reset position
        rb.velocity = Vector2.zero; // Stop any movement
    }
}
