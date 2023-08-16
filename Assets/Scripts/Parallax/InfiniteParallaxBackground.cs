using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteParallaxBackground : MonoBehaviour
{
    public bool enableScrolling = false;
    public float CameraSpeed = 1.5f;
    public float SpeedIncreaseRate = 50f; // Lisätty uusi muuttuja

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
        for (int i = 0; i < Layers.Length; i++)
        {
            initialPositions[i] = Layers[i].transform.position.x;
        }
    }

    void Update()
    {
        if (enableScrolling)
        {
            // Lisätty koodi kerrosten liikuttelun nopeuden lisäämiseksi ajan myötä
            for (int i = 0; i < LayerScrollSpeeds.Length; i++)
            {
                LayerScrollSpeeds[i] -= SpeedIncreaseRate * Time.deltaTime;
            }

            mainCamera.position += Vector3.right * Time.deltaTime * CameraSpeed;
            for (int i = 0; i < Layers.Length; i++)
            {
                float offset = (mainCamera.position.x * (1 - LayerScrollSpeeds[i]));
                float scrollDistance = mainCamera.position.x * LayerScrollSpeeds[i];
                Layers[i].transform.position = new Vector2(initialPositions[i] + scrollDistance, Layers[i].transform.position.y);

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
