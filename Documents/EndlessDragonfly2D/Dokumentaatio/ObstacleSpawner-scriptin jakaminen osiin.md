
### 1. Johdanto

Käsittelimme alkuperäistä `ObstacleSpawner`-scriptiä, joka oli yksi monimutkainen yksikkö vastuussa useista tehtävistä. Tavoitteena oli jakaa tämä scripti erillisiksi osiksi selkeyden ja ylläpidettävyyden lisäämiseksi.

### 2. Alkuperäinen rakenne

Alkuperäinen scripti sisälsi seuraavat päätoiminnot:
- Oikeanpuoleisimman `Ground_Second`-tagillisen objektin haku.
- Esteiden spawn-logiikka.
- Parallaksi-taustojen käsittely, mukaan lukien esteiden poisto.

### 3. Uusi rakenne

Päätimme jakaa alkuperäisen scriptin kolmeen erilliseen osaan:

1. **GroundManager**: Hallinnoi kaikkia `Ground_Second`-tagillisia objekteja ja tarjoaa pääsyn oikeanpuoleisimpaan maahan.
2. **ObstacleManager**: Vastuussa esteiden luomisesta ja sijoittelusta.
3. **ParallaxEventHandler**: Käsittelee kaikki tapahtumat, jotka liittyvät parallaksi-taustoihin.

### 4. Koodin jakaminen

#### 4.1 GroundManager

```csharp
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    private GameObject[] grounds;

    private void Start()
    {
        grounds = GameObject.FindGameObjectsWithTag("Ground_Second");
    }

    public GameObject GetRightmostGroundSecond()
    {
        GameObject rightmostGround = null;
        float maxX = float.NegativeInfinity;

        foreach (GameObject ground in grounds)
        {
            if (ground.transform.position.x > maxX)
            {
                maxX = ground.transform.position.x;
                rightmostGround = ground;
            }
        }

        return rightmostGround;
    }
}
```

#### 4.2 ObstacleManager

```csharp
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public GameObject[] downObstaclePrefabs;
    public GameObject[] upObstaclePrefabs;
    private GroundManager groundManager;
    private float spawnXPositionOffset = 6f;

    private void Start()
    {
        groundManager = GetComponent<GroundManager>();
        SpawnObstacle();
        StartCoroutine(SpawnObstacles());
    }

    private void SpawnObstacle()
    {
        GameObject rightmostGround = groundManager.GetRightmostGroundSecond();
        BoxCollider2D groundCollider = rightmostGround.GetComponent<BoxCollider2D>();
        ...
        // Loput alkuperäisen SpawnObstacle-metodin koodista
    }

    IEnumerator SpawnObstacles()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2.0f, 6.0f));
            SpawnObstacle();
        }
    }
}
```

#### 4.3 ParallaxEventHandler

```csharp
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

    private void HandleLayerShifted(Transform shiftedLayer)
    {
        ...
        // Loput alkuperäisen HandleLayerShifted-metodin koodista
    }

    private void OnDisable()
    {
        foreach (var layer in infiniteParallaxBackground.parallaxLayers)
        {
            layer.onLayerShifted -= HandleLayerShifted;
        }
    }
}
```

### 5. Yhteenveto

Jakamalla alkuperäinen `ObstacleSpawner`-scripti kolmeen erilliseen osaan olemme tehneet koodista selkeämmän ja helpommin ylläpidettävän. Jokainen osa voi nyt toimia itsenäisesti ja tarjota erityiset palvelut muille komponenteille, mikä lisää modularisuutta ja joustavuutta jatkokehitykselle.

[[New Obtacle Spawner]]