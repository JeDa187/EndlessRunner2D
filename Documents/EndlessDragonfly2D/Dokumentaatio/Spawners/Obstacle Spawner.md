# Alkuperäisen Obstacle Spawner -järjestelmän Rakentaminen 

## 1. Johdanto 

Tässä dokumentaatiossa kuvataan alkuperäisen Obstacle Spawner -järjestelmän rakentamisen vaiheet ja rakenne ennen muutostöitä. Järjestelmän tarkoituksena on luoda esteitä tiettyjen aikavälien kuluttua pelaajan tielle pelissä. 

## 2. Yleiskatsaus 

Alkuperäisessä järjestelmässä oli yksi esteiden tyyppi, jotka luotiin satunnaisin väliajoin yhteen paikkaan pelikentällä. 

## 3. Alkuperäinen Koodi 

### 3.1 Muuttujat 

- **obstaclePrefab:** Esteen prefab, joka instansoidaan kentälle. 
- **mainCamera:** Viittaus pääkameraan, käytetään spawnauksen sijainnin laskemiseen. 
- **spawnRate:** Aika sekunteina esteiden spawnauksien välillä. 
- **parallaxBackground:** Viittaus taustan parallax-liikkeeseen nopeuden säätämiseen. 
- **spawnXPositionOffset:** Lisätty etäisyys x-akselilla spawnaukseen. 

### 3.2 Coroutine 

Koodissa käytettiin coroutinea `SpawnObstacles()`, joka on vastuussa esteiden spawnauksesta. 

```csharp
private void Start() 
{ 
    StartCoroutine(SpawnObstacles()); 
} 
 
IEnumerator SpawnObstacles() 
{ 
    while (true) 
    { 
        spawnRate = Random.Range(3.0f, 10.0f); 
        yield return new WaitForSeconds(spawnRate); 
         
        // Esteiden spawnauksen koodi 
    } 
} ```

### 3.3 Esteiden Spawnaus

- **cameraHalfWidth**: Lasketaan kameran leveyden perusteella.
- **spawnXPosition**: Lasketaan kameran x-sijainnin ja cameraHalfWidth perusteella.
- **randomY**: Esteiden y-sijainti valitaan satunnaisesti väliltä [-12, -2.85].

Este instansioidaan `obstaclePrefab`-prefabin perusteella määritettyyn `spawnXPosition` ja `randomY` sijaintiin.

```csharp
GameObject obstacle = Instantiate(obstaclePrefab, new Vector2(spawnXPosition, randomY), Quaternion.identity);
```

### 3.4 Esteiden Liike ja Tuhoaminen

- **layer0Speed**: Lasketaan `parallaxBackground`-komponentin ja `CameraSpeed`- ja `LayerScrollSpeeds`-arvojen perusteella.
- **destructionPosition**: Lasketaan kameran x-sijainnin ja `cameraHalfWidth` perusteella.

```csharp
obstacle.GetComponent<ObstacleMovement>().SetSpeed(layer0Speed);
obstacle.GetComponent<ObstacleMovement>().SetDestructionXPosition(destructionPosition);
```

### 4. Yhteenveto

Alkuperäinen järjestelmä on yksinkertainen mutta toimiva tapa spawnata esteitä pelikentälle. Se kuitenkin rajoittuu yhteen esteiden tyyppiin ja spawnauksen sijaintiin, mikä johti myöhempiin muutostöihin järjestelmän monipuolistamiseksi ja parantamiseksi.

## 2. Muutokset

### 2.1 Esteiden Prefab-listat

Aluksi koodiin lisättiin kaksi erillistä prefab-listaa: yksi alaesteille ja toinen yläesteille.

```csharp
public GameObject[] downObstaclePrefabs;
public GameObject[] upObstaclePrefabs;
```

### 2.2 Coroutinen Muokkaus

Alkuperäinen coroutine-funktio muokattiin siten, että se voi spawnata esteitä sekä ylä- että alareunasta. Tätä varten lisättiin satunnaislukugeneraattori, joka valitsee, kumpi esteiden tyyppi spawnataan.

```csharp
IEnumerator SpawnObstacles() {     while (true)     {         obstacleSpawnRate = Random.Range(3.0f, 12.0f);         yield return new WaitForSeconds(obstacleSpawnRate);         // Muu koodi     } }
```

### 2.3 Esteiden Spawnaus

Kun este spawnaataan, valitaan ensin satunnaisesti, kumpi lista valitaan (ala- tai yläeste).

```csharp
`int listChoice = Random.Range(0, 2);`
```

Sitten este instansoidaan ja sen rotaatio asetetaan tarvittaessa. Jos valittu lista on yläesteiden lista (`listChoice == 1`), esteen x-rotaatio asetetaan 180 asteeseen.

```csharp
GameObject obstacle = Instantiate(obstaclePrefab, new Vector2(spawnXPosition, randomY), Quaternion.identity);  if (listChoice == 1) {     obstacle.transform.eulerAngles = new Vector3(180, 0, 0); }
```

### 2.4 Nopeuden ja Tuhoamisposition Asetus

Lopuksi esteen nopeus ja tuhoamispositio asetetaan normaalisti.

```csharp
float layer0Speed = parallaxBackground.CameraSpeed * (1 - parallaxBackground.LayerScrollSpeeds[0]); obstacle.GetComponent<ObstacleMovement>().SetSpeed(layer0Speed);  

float destructionPosition = mainCamera.transform.position.x - cameraHalfWidth - 2f; obstacle.GetComponent<ObstacleMovement>().SetDestructionXPosition(destructionPosition);
```

## 3. Yhteenveto

Muutokset mahdollistavat nyt eri esteiden spawnaamisen sekä ylä- että alareunasta satunnaisesti valitun ajanjakson jälkeen, ja yläesteiden rotaation asettamisen 180 asteeseen x-akselilla automaattisesti.

## Päivitys

Tässä päivityksessä lisättiin rajoitus esteiden spawnaukseen, joka estää saman tyyppisten esteiden spawnaamisen peräkkäin enempää kuin kolme kertaa. Tämä lisäys varmistaa, että pelissä nähdään molempia esteiden tyyppiä tasaisesti ja pelaajille tarjotaan jatkuvasti vaihtelevia haasteita.

### Toteutus

#### Lisätyt Muuttujat

Kaksi muuttujaa (`consecutiveDown` ja `consecutiveUp`) lisättiin laskemaan, kuinka monta kertaa kumpaakin esteen tyyppiä on spawnattu peräkkäin.

#### Logiikan Muutos Spawnauksessa

Ennen esteen spawnausta tarkistetaan, onko jompikumpi `consecutiveDown` tai `consecutiveUp` muuttujista saavuttanut arvon 3.

Jos kumpikaan ei ole, esteiden tyyppi valitaan satunnaisesti.

Jos jompikumpi muuttuja on saavuttanut arvon 3, seuraava este valitaan automaattisesti olemaan toista tyyppiä, ja vastaava `consecutive`-muuttuja nollataan.

### Hyödyt

Tämä päivitys pitää pelin haastavana ja mielenkiintoisena pelaajille, sillä se estää samaa esteiden tyyppiä spawnautumasta liian monta kertaa peräkkäin. Se myös varmistaa, että molemmat esteiden tyypit ovat edustettuina pelissä tasaisesti.

---
**Siirtyminen Spawnereista Poolaukseen: Lyhyt Selvitys**

Pelissämme käytimme aiemmin spawnereita objektien, kuten vihollisten ja esteiden, luomiseen. Tämä tarkoitti, että joka kerta kun tarvitsimme uuden objektin, loimme sen tyhjästä ja tuhosimme sen, kun emme enää tarvinneet sitä. Vaikka tämä menetelmä on yksinkertainen ja helppokäyttöinen, se ei ole tehokas, varsinkin kun pelissä on paljon objekteja, jotka luodaan ja tuhotaan jatkuvasti.

Tämä johti suorituskykyongelmiin, kuten pätkiviin taustoihin ja viiveisiin, erityisesti laitteissa, joissa on rajoitettu laskentateho. 

Siirryimme käyttämään objektipoolausjärjestelmää vastataksemme tähän haasteeseen. Poolauksen avulla voimme "kierrättää" jo luotuja objekteja sen sijaan, että loisimme ja tuhoaisimme niitä jatkuvasti. Kun objekti "tuodaan takaisin eloon", se otetaan pois poolista, ja kun se "kuolee", se palautetaan takaisin pooliin uudelleenkäytettäväksi. 

Tämä muutos vähensi huomattavasti CPU:n ja muistin kuormitusta, mikä paransi pelimme suorituskykyä ja varmisti sulavan pelikokemuksen pelaajille. Poolauksen käyttöönotto oli kriittinen askel pelimme optimoinnissa ja suorituskyvyn parantamisessa.

[[Objektipoolausjärjestelmä]]