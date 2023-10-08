**FiregemSpawner-järjestelmän luominen

---

### 1. Johdanto

`FiregemSpawner` on luokka jonka tarkoitus on luoda FireGem-objekteja pelimaailmaan määrätyin väliajoin tiettyyn sijaintiin.

---

### 2. Muuttujien ja Ominaisuuksien Esittely

```csharp
public GameObject firegemPrefab;
private float initialWaitTime = 30f;
private float minSpawnInterval = 20f;
private float maxSpawnInterval = 40f;
public InfiniteParallaxBackground parallaxBackground;
```

- **firegemPrefab**: FireGemin prefab, joka instansoidaan pelimaailmaan.
- **initialWaitTime**: Ensimmäinen odotusaika ennen FireGemien spawnauksen aloittamista.
- **minSpawnInterval** ja **maxSpawnInterval**: Arvot määrittävät satunnaisen aikavälin FireGemien spawnaukselle.
- **parallaxBackground**: Viittaus `InfiniteParallaxBackground`-komponenttiin, joka mahdollistaa parallaksiliikkeen ja FireGemin spawnauslogiikan integraation.

---

### 3. Toiminnallisuuden Kuvaus

#### 3.1 Aloitusfunktio (`Start`)

Kun peli alkaa, FiregemSpawner aloittaa odotuksen ennen ensimmäistä FireGemin spawnauksesta ja liittää itsensä parallaksitaustan tapahtumiin.

```csharp
private void Start()
{
    StartCoroutine(SpawnFiregems());
    foreach (var layer in parallaxBackground.parallaxLayers)
    {
        layer.onLayerShifted += HandleLayerShifted;
    }
}
```

#### 3.2 Firegemin Spawnauskorutiini (`SpawnFiregems`)

Tämä korutiini huolehtii FireGemin spawnauksesta satunnaisilla väliajoilla määriteltyjen minSpawnInterval ja maxSpawnInterval arvojen välillä.

```csharp
private IEnumerator SpawnFiregems()
{
    yield return new WaitForSeconds(initialWaitTime);
    while (true)
    {
        SpawnFiregem();
        float randomWaitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
        yield return new WaitForSeconds(randomWaitTime);
    }
}
```

#### 3.3 Firegemin Luontifunktio (`SpawnFiregem`)

Funktio luo FireGemin ja asettaa sen tietylle sijainnille pelimaailmassa. FireGem liitetään myös "Ground_Second" -objektin lapseksi.

```csharp
private void SpawnFiregem()
{
    GameObject[] grounds = GameObject.FindGameObjectsWithTag("Ground_Second");
    // ... (Logiikka oikeanpuoleisimman objektin valitsemiseksi)
    Vector3 spawnPosition = rightmostGround.transform.position + new Vector3(15f, Random.Range(4f, 19f), 0);
    GameObject spawnedGem = Instantiate(firegemPrefab, spawnPosition, Quaternion.identity);
    spawnedGem.transform.SetParent(rightmostGround.transform);
}
```

#### 3.4 Tapahtumankäsittelijä (`HandleLayerShifted`)

Tämä funktio käsittelee `onLayerShifted` -tapahtumaa, joka laukaistaan, kun parallaksitaustan kerros siirtyy. Se tuhoaa kaikki FireGem-objektit, jotka ovat tämän kerroksen lapsia.

```csharp
private void HandleLayerShifted(Transform shiftedLayer)
{
    foreach (Transform child in shiftedLayer)
    {
        if (child.CompareTag("FireGem"))
        {
            Destroy(child.gameObject);
        }
    }
}
```

---
Tässä osiossa keskitymme erityisesti viimeaikaisiin muutoksiin.

### Muutokset SpawnFiregem-metodissa

#### Alkuperäinen suunnitelma:

Alkuperäisessä toteutuksessa `SpawnFiregem`-metodi loi yksinkertaisesti `Firegem`-esineen satunnaiseen Y-koordinaatin paikkaan ja kiinteään X-koordinaatin paikkaan oikeanpuoleisimman `Ground_Second`-tagin omaavan objektin yläpuolella.

#### Ongelma:

Endless runner -pelityyppisissä peleissä on usein esteitä tai muita objekteja, jotka pelaajan tulee välttää. Jos `Firegem`-esine luodaan sattumanvaraisesti, on mahdollista, että se luodaan esteen päälle tai liian lähelle estettä, mikä voi aiheuttaa pelaajalle sekaannusta tai tehdä pelistä epäreilun.

#### Ratkaisu:

Lisätään törmäystarkistus, joka varmistaa, että `Firegem`-esine ei luo paikkaan, jossa on jo jokin toinen objekti (esim. este).

### Muokatun SpawnFiregem-metodin selitys:

1. **Objektin etsiminen**: Kuten alkuperäisessä metodissa, ensin etsitään kaikki `Ground_Second`-tagilliset objektit.
    
2. **Oikeanpuoleisimman objektin valitseminen**: Etsitään oikeanpuoleisin `Ground_Second`-objekti, koska se on todennäköisin paikka, jossa pelaaja näkee seuraavaksi.
    
3. **Spawn-paikan valinta**: Tässä on suurin muutos. Käytämme `while`-silmukkaa yrittääksemme löytää sopiva spawn-paikka. Joka kierroksella valitaan satunnainen Y-koordinaatti ja kiinteä X-koordinaatti.
    
4. **Törmäystarkistus**: Käytämme `Physics2D.OverlapCircle`-metodia tarkistamaan, onko valitussa spawn-paikassa törmäystä. Jos törmäystä ei ole (eli palautettu `Collider2D` on `null`), `Firegem` luodaan tähän paikkaan ja silmukasta poistutaan. Jos törmäystä on, silmukka jatkaa uuden spawn-paikan valintaa.
    
5. **Firegemin luominen**: Kun sopiva paikka on löydetty, `Firegem`-esine luodaan käyttäen `Instantiate`-metodia ja asetetaan lapsena oikeanpuoleisimmalle `Ground_Second`-objektille.
    

### Lopullinen ajatus:

Tämän muutoksen avulla `Firegem`-esineiden luonti on johdonmukaisempaa ja pelaajaystävällisempää. Estämällä `Firegem`-esineiden luominen esteiden päälle varmistamme, että pelaajilla on selkeä ja reilu mahdollisuus kerätä ne pelissä.