using UnityEngine;
using TMPro;

public class DragonflyController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 12f;

    private bool canMove = false;
    private Rigidbody2D rb;
    private Vector2 originalPosition;
    private Renderer rend;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalPosition = transform.position;
        rend = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (!canMove) return;

        HandleInput();
        CheckOutOfScreenBounds();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            rb.velocity = Vector2.up * jumpForce;
        }
    }

    private void CheckOutOfScreenBounds()
    {
        Vector2 viewportPositionMin = Camera.main.WorldToViewportPoint(rend.bounds.min);
        Vector2 viewportPositionMax = Camera.main.WorldToViewportPoint(rend.bounds.max);

        if (viewportPositionMax.y < 0 || viewportPositionMin.y > 1)
        {
            GameManager.Instance.GameOver();
        }
    }

    public void ToggleRigidbodyMovement(bool allowMovement)
    {
        canMove = allowMovement;
        rb.isKinematic = !allowMovement;

        if (!allowMovement)
        {
            rb.velocity = Vector2.zero;
        }
    }

    public void ResetDragonfly()
    {
        gameObject.SetActive(true);
        transform.position = originalPosition;
        rb.velocity = Vector2.zero;
    }
}
