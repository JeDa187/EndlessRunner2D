using UnityEngine;

public class InfiniteParallaxBackground : MonoBehaviour
{
    public bool enableScrolling = true;
    public float cameraSpeed = 1.5f;
    public float accelerationFactor; // Kiihtyvyyskerroin taustoille

    [System.Serializable]
    public class ParallaxLayer
    {
        public float scrollSpeed;
        public Transform parentObject; // Viittaus parent GameObjectiin
        private Transform[] childSprites = new Transform[3]; // Lapsispritet
        private float spriteWidth;

        public void Initialize()
        {
            for (int i = 0; i < 3; i++)
            {
                childSprites[i] = parentObject.GetChild(i);
            }

            spriteWidth = childSprites[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        }

        public void Scroll(float cameraPosition, float cameraSpeed)
        {
            for (int i = 0; i < childSprites.Length; i++)
            {
                childSprites[i].position += new Vector3(-scrollSpeed * Time.deltaTime * cameraSpeed, 0, 0);

                if (childSprites[i].position.x < cameraPosition - spriteWidth)
                {
                    childSprites[i].position += new Vector3(3 * spriteWidth, 0, 0);
                }
            }
        }
    }

    public ParallaxLayer[] parallaxLayers;
    private Transform mainCamera;

    void Start()
    {
        mainCamera = Camera.main.transform;
        foreach (var layer in parallaxLayers)
        {
            layer.Initialize();
        }
    }

    private void Update()
    {
        if (enableScrolling)
        {
            cameraSpeed += accelerationFactor * Time.deltaTime; // Tämä lisää nopeutta tasaisesti
            mainCamera.position += cameraSpeed * Time.deltaTime * Vector3.right;

            foreach (var layer in parallaxLayers)
            {
                layer.Scroll(mainCamera.position.x, cameraSpeed);
            }
        }
    }

    public float CameraSpeed
    {
        get { return cameraSpeed; }
        set { cameraSpeed = value; }
    }
}
