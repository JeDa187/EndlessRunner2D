### Esteiden Spawnaus- ja Tuhoamisjärjestelmä `Ground_Second` -kerroksen kanssa

---

#### **Yleiskatsaus**

Tavoitteena on luoda esteitä pelissä siten, että ne spawnataan etummaiseen `Ground_Second` -objektiin. Kun ne liikkuvat pois kameran näkökentästä vasemmalla, ne tuhoutuvat.

---

#### **Työvaiheet**

1. **Hanki viittaus etummaiseen `Ground_Second` -objektiin:**

    - Etsi kaikki `Ground_Second` -objektit pelimaailmassa ja valitse niistä se, joka sijaitsee kauimpana oikealla.

2. **Muuta esteiden spawnauspaikkaa:**

    - Kun esteitä spawnataan, ne tehdään etummaisen `Ground_Second` -objektin lapsiksi.

3. **Toteuta esteiden liike ja tuhoaminen:**

    - Ottaen huomioon esteen sijainnin suhteessa sen vanhempaan (`Ground_Second` -objekti), päivitä skriptiä varmistaaksesi, että esteet tuhoutuvat oikeassa kohdassa.

---

#### **Koodiesimerkit**

- **Hanki oikein sijaitseva `Ground_Second` -objekti:**

```csharp
private GameObject GetRightmostGroundSecond()
{
    GameObject[] grounds = GameObject.FindGameObjectsWithTag("Ground_Second"); 
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
```

- **Spawnaa esteet:**

```csharp
IEnumerator SpawnObstacles()
{
    // ... [koodi, joka määrittää spawn-paikan ja esteen tyypin]

    GameObject newObstacle = Instantiate(obstaclePrefab, new Vector2(spawnXPosition, randomY), rotation);
    newObstacle.transform.parent = GetRightmostGroundSecond().transform;
}
```

- **Päivitetty esteiden tuhoamiskoodi:**

```csharp
using UnityEngine;

public class ObstacleDestruction : MonoBehaviour
{
    private float destructionXPosition;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        float globalDestructionXPosition = mainCamera.transform.position.x + destructionXPosition + transform.parent.position.x;

        if (transform.position.x < globalDestructionXPosition)
        {
            Destroy(gameObject);
        }
    }

    public void SetDestructionXPosition(float position)
    {
        destructionXPosition = position;
    }
}
```

---

**Loppusanat**

Tämän dokumentin avulla voit yhdistää esteiden spawnausjärjestelmän `Ground_Second` -kerroksen kanssa, jolloin saat esteet liikkumaan sujuvasti ja tuhoutumaan kun ne liikkuvat pois kameran näkökentästä. Varmista aina, että `Ground_Second` tagi on oikein asetettu ja että `destructionXPosition` on asetettu oikein esteiden tuhoamiseksi oikeassa kohdassa.


---

## Lisämuutokset `ObstacleSpawner` skriptiin

Tavoitteena oli saada ensimmäinen este ilmestymään heti pelin alussa, joten teimme seuraavat muutokset:

1. **Uusi `SpawnObstacle` metodi**: Jotta voisimme helposti hallita yksittäisen esteen luomista, erottelimme esteen luomislogiikan omaksi metodikseen nimeltään `SpawnObstacle`.

```csharp
private void SpawnObstacle()
{
    // ... (esteiden luontilogiikka) ...
}
```

2. **Muutokset `Start` metodissa**: Kutsuimme `SpawnObstacle` metodia heti `Start` metodin alussa, jotta ensimmäinen este ilmestyisi pelin käynnistyttyä.

```csharp
private void Start()
{
    obstacleSpawnRate = Random.Range(3.0f, 12.0f) / infiniteParallaxBackground.CameraSpeed;
    
    // Spawn the first obstacle immediately
    SpawnObstacle();

    StartCoroutine(SpawnObstacles());
}
```

3. **Muutokset `SpawnObstacles` korutiinissa**: Nyt kun meillä on `SpawnObstacle` metodi, voimme yksinkertaisesti kutsua sitä korutiinin sisällä joka kerta kun haluamme luoda uuden esteen. Tämä tekee koodista selkeämmän ja helpomman ylläpitää.

```csharp
IEnumerator SpawnObstacles()
{
    while (true)
    {
        yield return new WaitForSeconds(obstacleSpawnRate);
        obstacleSpawnRate = Random.Range(3.0f, 12.0f) / infiniteParallaxBackground.CameraSpeed;

        SpawnObstacle();
    }
}
```

---

