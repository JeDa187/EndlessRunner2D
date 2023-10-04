using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    private InfiniteParallaxBackground parallaxBackground; // Referenssi taustan skriptiin
    private float layerSpeed; // Layerin nopeus, jota haluamme seurata (t‰ss‰ tapauksessa index 4: "Ground_Second")

    void Start()
    {
        // Oletetaan, ett‰ InfiniteParallaxBackground-skripti sijaitsee samassa objektissa kuin t‰m‰ skripti tai voit m‰‰ritt‰‰ sen manuaalisesti
        parallaxBackground = FindObjectOfType<InfiniteParallaxBackground>();
        if (parallaxBackground && parallaxBackground.LayerScrollSpeeds.Length > 4) // Varmista, ett‰ index 4 on olemassa
        {
            layerSpeed = parallaxBackground.LayerScrollSpeeds[4];
        }
    }

    void Update()
    {
        // Liikuta esteit‰ perustuen Ground_Second layerin nopeuteen
        if (parallaxBackground && parallaxBackground.enableScrolling)
        {
            transform.position += Vector3.left * layerSpeed * Time.deltaTime;
        }
    }
}
