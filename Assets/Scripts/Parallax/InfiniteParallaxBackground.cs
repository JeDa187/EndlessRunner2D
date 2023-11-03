using UnityEngine;

public class InfiniteParallaxBackground : MonoBehaviour
{
    public bool enableScrolling = true;
    public float cameraSpeed = 1.5f;
    public float accelerationFactor; // Kiihtyvyyskerroin taustoille

    public ParallaxLayer[] parallaxLayers;
    private Transform mainCamera;
    private Vector3 cameraMoveVector; // K‰ytet‰‰n ennalta luotua vektoria kameran siirtoon

    void Start()
    {
        mainCamera = Camera.main.transform;
        cameraMoveVector = new Vector3(cameraSpeed * Time.deltaTime, 0, 0);
        foreach (var layer in parallaxLayers)
        {
            layer.Initialize();
        }
    }

    private void Update()
    {
        if (Time.timeScale == 0) return;

        if (enableScrolling)
        {
            cameraSpeed += accelerationFactor * Time.deltaTime; // T‰m‰ lis‰‰ nopeutta tasaisesti
            mainCamera.position += cameraMoveVector;

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

    [System.Serializable]
    public class ParallaxLayer
    {
        public delegate void OnLayerShifted(Transform shiftedLayer);
        public event OnLayerShifted LayerShifted;

        public float scrollSpeed;
        public Transform parentObject; // Viittaus parent GameObjectiin
        private Transform[] childSprites = new Transform[3]; // Lapsispritet
        private float spriteWidth;
        private SpriteRenderer spriteRenderer; // Tallennetaan sprite rendererin viittaus t‰h‰n

        public void Initialize()
        {
            for (int i = 0; i < 3; i++)
            {
                childSprites[i] = parentObject.GetChild(i);
            }

            // K‰ytet‰‰n ennalta haettua sprite renderer -viittausta sen sijaan, ett‰ haettaisiin se joka kerta
            spriteRenderer = childSprites[0].GetComponent<SpriteRenderer>();
            spriteWidth = spriteRenderer.sprite.bounds.size.x;
        }

        public void Scroll(float cameraPosition, float cameraSpeed)
        {
            Vector3 scrollVector = new(-scrollSpeed * Time.deltaTime * cameraSpeed, 0, 0);
            Vector3 resetVector = new(3 * spriteWidth, 0, 0);
            for (int i = 0; i < childSprites.Length; i++)
            {
                childSprites[i].position += scrollVector;

                // Kun yksitt‰inen childSprite menee kameran vasemmalle puolelle
                if (childSprites[i].position.x < cameraPosition - spriteWidth)
                {
                    childSprites[i].position += resetVector;

                    // T‰m‰ laukaisee tapahtuman
                    LayerShifted?.Invoke(childSprites[i]);
                }
            }
        }
    }
}
