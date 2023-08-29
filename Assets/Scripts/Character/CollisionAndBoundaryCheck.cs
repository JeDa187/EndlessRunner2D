using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAndBoundaryCheck : MonoBehaviour
{

    //private DragonflyController dragonflyController;
    private Renderer rend;

    private void Awake()
    {
        //dragonflyController = GetComponent<DragonflyController>();
        rend = GetComponent<Renderer>(); // Get the Renderer component

    }
    private void Update()
    {
        CheckOutOfScreenBounds();
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
}
