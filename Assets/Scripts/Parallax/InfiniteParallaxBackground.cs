using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteParallaxBackground : MonoBehaviour
{
    public bool enableScrolling = false;
    public float CameraSpeed = 1.5f;

    [Header("Layer Settings")]
    public float[] LayerScrollSpeeds = new float[7];
    public GameObject[] Layers = new GameObject[7];

    private Transform mainCamera;
    private float[] initialPositions = new float[7];
    private float spriteWidth;
    private float spriteSizeX;

    void Start()
    {
        mainCamera = Camera.main.transform;
        spriteSizeX = Layers[0].transform.localScale.x;
        spriteWidth = Layers[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        for (int i = 0; i < Layers.Length; i++)  // Muutettu arvo 5 arvoon Layers.Length
        {
            initialPositions[i] = Layers[i].transform.position.x;  // Muutettu mainCamera.position.x arvoon Layers[i].transform.position.x
        }
    }


    void Update()
    {
        if (enableScrolling)
        {
            mainCamera.position += Vector3.right * Time.deltaTime * CameraSpeed;
            for (int i = 0; i < Layers.Length; i++)  // Muutettu arvo 5 arvoon Layers.Length
            {
                float offset = (mainCamera.position.x * (1 - LayerScrollSpeeds[i]));
                float scrollDistance = mainCamera.position.x * LayerScrollSpeeds[i];
                Layers[i].transform.position = new Vector2(initialPositions[i] + scrollDistance, Layers[i].transform.position.y);  // Muutettu mainCamera.position.y arvoon Layers[i].transform.position.y

                if (offset > initialPositions[i] + spriteWidth * spriteSizeX)
                {
                    initialPositions[i] += spriteWidth * spriteSizeX;
                }
                else if (offset < initialPositions[i] - spriteWidth * spriteSizeX)
                {
                    initialPositions[i] -= spriteWidth * spriteSizeX;
                }

                if (Layers[i].transform.position.x < mainCamera.position.x - spriteWidth * spriteSizeX)
                {
                    Layers[i].transform.position += Vector3.right * spriteWidth * spriteSizeX * 2;
                }
            }
        }
    }

}
