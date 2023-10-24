using UnityEngine;

public class ParallaxEventHandler : MonoBehaviour
{
    public InfiniteParallaxBackground infiniteParallaxBackground;

    private void OnEnable()
    {
        foreach (var layer in infiniteParallaxBackground.parallaxLayers)
        {
            layer.LayerShifted += HandleLayerShifted;
        }
    }

    private void OnDisable()
    {
        foreach (var layer in infiniteParallaxBackground.parallaxLayers)
        {
            layer.LayerShifted -= HandleLayerShifted;
        }
    }

    private void HandleLayerShifted(Transform shiftedLayer)
    {
        if (shiftedLayer.parent.CompareTag("Ground_Second"))
        {
            // Etsi oikeanpuoleisin pala
            Transform rightmostChild = null;
            float maxX = float.MinValue;

            foreach (Transform child in shiftedLayer.parent)
            {
                if (child.position.x > maxX)
                {
                    maxX = child.position.x;
                    rightmostChild = child;
                }
            }

            // Jos oikeanpuoleista palaa ei löytynyt, lopeta tässä
            if (rightmostChild == null) return;

            // Käy läpi oikeanpuoleisen palan lapset
            foreach (Transform grandChild in rightmostChild)
            {
                if (grandChild.CompareTag("ObstacleDown1") ||
                    grandChild.CompareTag("ObstacleDown2") ||
                    grandChild.CompareTag("ObstacleUp1") ||
                    grandChild.CompareTag("ObstacleUp2"))
                {
                    // Palauta este pooliin sen tagin perusteella
                    ObstaclePooler.Instance.ReturnToPool(grandChild.tag, grandChild.gameObject);
                }
            }
        }
    }

}
