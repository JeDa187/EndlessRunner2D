using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    private InfiniteParallaxBackground parallaxBackground;

    void Start()
    {
        // Automatically get the reference to the InfiniteParallaxBackground script
        parallaxBackground = FindObjectOfType<InfiniteParallaxBackground>();
    }

    void Update()
    {
        // Get the speed of the specified parallax layer in real-time
        float layerSpeed = parallaxBackground.LayerScrollSpeeds[4];

        // Move the obstacle at the same speed as the parallax layer but to the left
        transform.position += Vector3.left * layerSpeed * Time.deltaTime;
    }
}