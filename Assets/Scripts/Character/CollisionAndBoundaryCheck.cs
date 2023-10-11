using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAndBoundaryCheck : MonoBehaviour
{
    private Renderer rend;
    private string[] tagsToCheck = { "Hazard", "Enemy1", "Enemy2", "ObstacleDown1", "ObstacleDown2", "ObstacleUp1", "ObstacleUp2" };

    private void Awake()
    {
        rend = GetComponent<Renderer>(); // Get the Renderer component
    }

    private void Update()
    {
        CheckOutOfScreenBounds();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsTagMatched(collision.gameObject.tag))
        {
            GameManager.Instance.GameOver(); // End the game
            Destroy(gameObject);            // Destroy the dragonfly game object
        }
    }

    private bool IsTagMatched(string tagToCheck)
    {
        foreach (string tag in tagsToCheck)
        {
            if (tagToCheck == tag)
            {
                return true;
            }
        }
        return false;
    }

    private void CheckOutOfScreenBounds()
    {
        Vector2 viewportPositionMin = Camera.main.WorldToViewportPoint(rend.bounds.min);
        Vector2 viewportPositionMax = Camera.main.WorldToViewportPoint(rend.bounds.max);

        float objectHeightInViewport = viewportPositionMax.y - viewportPositionMin.y;

        if (viewportPositionMax.y < 0.5f * objectHeightInViewport || viewportPositionMin.y > 1 - 0.5f * objectHeightInViewport)
        {
            GameManager.Instance.GameOver(); // End the game
            Destroy(gameObject); // Destroy the dragonfly game object
        }
    }
}
