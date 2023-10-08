using UnityEngine;

public class ParallaxEventHandler : MonoBehaviour
{
    public InfiniteParallaxBackground infiniteParallaxBackground;

    private void OnEnable()
    {
        foreach (var layer in infiniteParallaxBackground.parallaxLayers)
        {
            layer.onLayerShifted += HandleLayerShifted;
        }
    }

    private void OnDisable()
    {
        foreach (var layer in infiniteParallaxBackground.parallaxLayers)
        {
            layer.onLayerShifted -= HandleLayerShifted;
        }
    }

    private void HandleLayerShifted(Transform shiftedLayer)
    {
        if (shiftedLayer.parent.CompareTag("Ground_Second"))
        {
            foreach (Transform child in shiftedLayer)
            {
                if (child.CompareTag("Hazard"))
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }
}
