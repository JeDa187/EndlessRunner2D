using UnityEngine;

public class InfiniteParallaxBackground : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxLayer
    {

        public delegate void OnLayerShifted(Transform shiftedLayer);
        public event OnLayerShifted LayerShifted;

        private float elapsedTime = 0f;
        [SerializeField] float scrollSpeed = 2.0f; // Parallax-scrollin nopeus
        [SerializeField] float layerScrollSpeed; // Scrollin oma nopeus
        float exponentFactor = 0.005f;

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
        //public void SetScrollSpeed()
        //{
        //    layerScrollSpeed = CalculateExponentialScroll();
        //}

        private Vector3 CalculateScrollVector()
        {
            return new Vector3(-layerScrollSpeed * Time.deltaTime, 0, 0);
        }
        //private float CalculateExponentialScroll(float cameraPosition)
        //{
        //    return Mathf.Exp(exponentFactor * cameraPosition) * scrollSpeed;
        //}
        private float CalculateExponentialScroll()
        {
            return Mathf.Exp(exponentFactor * elapsedTime) * scrollSpeed;
        }
        private Vector3 CreateResetVector()
        {
            float smoothness = 1f; // S‰‰d‰ t‰m‰ arvo sopivaksi, 0.0f tarkoittaa ‰killist‰ siirtym‰‰, 1.0f tarkoittaa pehme‰‰ siirtym‰‰
            Vector3 targetPosition = new Vector3(3 * spriteWidth, 0, 0);
            return Vector3.Lerp(Vector3.zero, targetPosition, smoothness);
        }
        public void SetElapsedTime(float time)
        {
            elapsedTime = time;
        }

        public void Scroll(float cameraPosition, float deltaTime)
        {
            layerScrollSpeed = CalculateExponentialScroll();
            elapsedTime += deltaTime;
            Vector3 scrollVector = CalculateScrollVector();
            Vector3 resetVector = CreateResetVector();
            for (int i = 0; i < childSprites.Length; i++)
            {
                if (childSprites[i] != null)  // Tarkista, ettei childSprites[i] ole null
                {
                    childSprites[i].position += scrollVector;

                    // Kun yksitt‰inen childSprite menee kameran vasemmalle puolelle
                    if (childSprites[i].position.x < cameraPosition -spriteWidth)
                    {
                        childSprites[i].position += resetVector;

                        // T‰m‰ laukaisee tapahtuman
                        LayerShifted?.Invoke(childSprites[i]);
                    }
                }
            }
        }
    }




    public bool enableScrolling = true;
    public float cameraSpeed = 1.5f;
    public float accelerationFactor; // Kiihtyvyyskerroin taustoille
    public ParallaxLayer[] parallaxLayers;
    private Transform mainCamera;
    private Vector3 cameraMoveVector; // K‰ytet‰‰n ennalta luotua vektoria kameran siirtoon

    void Start()
    {

        mainCamera = Camera.main.transform;
        UpdateCameraMoveVector();
        foreach (var layer in parallaxLayers)
        {
            layer.Initialize();
        }
    }
    private void Update()
    {
        if (Time.timeScale == 0 || !enableScrolling) return;
        foreach (var layer in parallaxLayers)
        {

            layer.Scroll(mainCamera.position.x, Time.deltaTime);
        }
        UpdateCameraMoveVector();
    }
    private void LateUpdate()
    {
        if (Time.timeScale == 0) return;

        if (enableScrolling)
        {
            CameraSpeed += accelerationFactor * Time.deltaTime; // T‰m‰ lis‰‰ nopeutta tasaisesti
            UpdateCameraMoveVector();
            mainCamera.position += cameraMoveVector;
        }
    }
    private void UpdateCameraMoveVector()
    {
        cameraMoveVector = new Vector3(CameraSpeed * Time.deltaTime, 0, 0);
    }

    public float CameraSpeed
    {
        get { return cameraSpeed; }
        set { cameraSpeed = value; }
    }
}
