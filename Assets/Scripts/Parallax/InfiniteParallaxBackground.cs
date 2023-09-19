using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteParallaxBackground : MonoBehaviour
{

    public bool enableScrolling = false;                // A flag to enable or disable scrolling of the background
    public float cameraSpeed = 1.5f;                    // The speed at which the camera moves
    private float speedIncreaseRate = 0.05f;            // The rate at which the speed of the layers increases over time
    [Header("Layer Settings")]                          // Header in the Inspector for layer settings
    public float[] LayerScrollSpeeds = new float[7];    // The speed at which each layer scrolls
    public GameObject[] Layers = new GameObject[7];     // The layers to scroll
    private Transform mainCamera;                       // The main camera's transform component
    private float[] initialPositions = new float[7];    // The initial position of each layer
    private float spriteWidth;                          // The width of the sprite
    private float spriteSizeX;                          // The scale of the sprite on the X axis
    public float backgroundSpeed;

    public float CameraSpeed
    {
        get { return cameraSpeed; }
        set { cameraSpeed = value; }
    }

    void Start()
    {
        // Get the main camera's transform component
        mainCamera = Camera.main.transform;

        // Get the sprite's scale on the X axis
        spriteSizeX = Layers[0].transform.localScale.x;

        // Get the sprite's width
        spriteWidth = Layers[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;

        // Store the initial position of each layer
        for (int i = 0; i < Layers.Length; i++)
        {
            initialPositions[i] = Layers[i].transform.position.x;
        }
    }

    private void Update()
    {
        // If scrolling is enabled
        if (enableScrolling)
        {
            // Increase the speed of each layer over time
            for (int i = 0; i < LayerScrollSpeeds.Length; i++)
            {
                LayerScrollSpeeds[i] -= speedIncreaseRate * Time.deltaTime;
            }

            // Move the main camera to the right
            mainCamera.position += CameraSpeed * Time.deltaTime * Vector3.right;

            // Scroll each layer
            for (int i = 0; i < Layers.Length; i++)
            {
                // Calculate the offset based on the camera's position and the layer's scroll speed
                float offset = (mainCamera.position.x * (1 - LayerScrollSpeeds[i]));
                float scrollDistance = mainCamera.position.x * LayerScrollSpeeds[i];

                // Set the new position of the layer
                Layers[i].transform.position = new Vector2(initialPositions[i] + scrollDistance, Layers[i].transform.position.y);

                // Check if the layer has scrolled past its bounds and adjust its position if needed
                if (offset > initialPositions[i] + spriteWidth * spriteSizeX)
                {
                    initialPositions[i] += spriteWidth * spriteSizeX;
                }
                else if (offset < initialPositions[i] - spriteWidth * spriteSizeX)
                {
                    initialPositions[i] -= spriteWidth * spriteSizeX;
                }

                // If the layer has scrolled off the screen, move it back
                if (Layers[i].transform.position.x < mainCamera.position.x - spriteWidth * spriteSizeX)
                {
                    Layers[i].transform.position += 2 * spriteSizeX * spriteWidth * Vector3.right;
                }
            }
        }
    }
    public float GetLayer0Speed()
    {
        return LayerScrollSpeeds[0];
    }


    public void SetLayerSpeed(int layerIndex, float newSpeed)
    {
        if (layerIndex >= 0 && layerIndex < LayerScrollSpeeds.Length)
        {
            Debug.Log("Setting speed for layer " + layerIndex + " to " + newSpeed);
            LayerScrollSpeeds[layerIndex] = newSpeed;
        }
        else
        {
            Debug.LogError("Invalid layer index: " + layerIndex);
        }
    }
}
