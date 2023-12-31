
**Yleiskatsaus**:

`ObstacleSpawner` on komponentti, joka hallinnoi esteiden automaattista luontia (spawning) pelissä. Komponentin avulla esteiden spawn-tahti mukautuu kameran liikkeen nopeuteen, mutta tietyssä pisteessä tämä nopeuden vaikutus rajataan. Tämä mahdollistaa pelin sopeutumisen pelaajan etenemiseen ilman, että esteet ilmestyvät liian tiheään.

---

**Ominaisuudet ja Muuttujat**:

1. **minSpawnRate**: Pienin mahdollinen aikaväli (sekunteina) kahden esteen spawnaamisen välillä.
2. **maxSpawnRate**: Suurin mahdollinen aikaväli (sekunteina) kahden esteen spawnaamisen välillä.
3. **obstacleManager**: Yksityinen viittaus `ObstacleManager` -komponenttiin, joka huolehtii esteiden varsinaisesta luomisesta.
4. **backgroundScroller**: Julkinen viittaus `InfiniteParallaxBackground` -komponenttiin, joka antaa tiedon kameran nykyisestä liikenopeudesta.
5. **CAMERA_SPEED_LIMIT_FOR_SPAWN_ADJUSTMENT**: Yksityinen vakio, joka määrittelee rajan, jonka jälkeen spawn-tahti ei enää nopeudu vaikka kameran nopeus kasvaa.

---

**Metodit**:

- **Start()**:
    - Metodi suoritetaan kun peli alkaa.
    - Määrittelee `obstacleManager` -viittauksen.
    - Luo ensimmäisen esteen.
    - Käynnistää `SpawnObstacles` korutiinin esteiden luomiseksi.

- **SpawnObstacles() (IEnumerator)**:
    - Jatkuva loop, joka hallinnoi esteiden luomista.
    - Käyttää kameran nopeutta määrittääkseen spawnausvälin muokkauskerroin. 
    - Soveltuu spawnausvälit kameran nopeuteen.
    - Luo esteen määrätyin väliajoin.
    - Huomioi kameran nopeusrajoituksen spawnausvälissä.

---

**Käyttö ja Sovellukset**:

`ObstacleSpawner`-skripti on suunniteltu peleihin, joissa esteiden spawnaustahtiin vaikuttavat ulkoiset tekijät, kuten kameran nopeus. Skriptin avulla voidaan tehdä pelistä dynaamisempi ja haastavampi pelaajan etenemisen mukaisesti.

---

**Huomioita ja Parannusehdotuksia**:

- Koodissa oleva arvo 1.5 määrittelee kuinka spawn-tahti sopeutuu kameran nopeuteen. Arvoa voi muokata tarpeen mukaan.
- Vaikka spawnausväli mukautuu kameran nopeuteen, on tärkeää pitää mielessä, että on olemassa yläraja, jonka jälkeen spawn-tahti ei nopeudu, vaikka kamera liikkuisi nopeammin.
- Koodin logiikkaa voidaan laajentaa ottamaan huomioon muita pelimekaniikkoja tai pelaajan toimintoja, jotka vaikuttavat spawnausväliin.

---
**Siirtyminen Spawnereista Poolaukseen: Lyhyt Selvitys**

Pelissämme käytimme aiemmin spawnereita objektien, kuten vihollisten ja esteiden, luomiseen. Tämä tarkoitti, että joka kerta kun tarvitsimme uuden objektin, loimme sen tyhjästä ja tuhosimme sen, kun emme enää tarvinneet sitä. Vaikka tämä menetelmä on yksinkertainen ja helppokäyttöinen, se ei ole tehokas, varsinkin kun pelissä on paljon objekteja, jotka luodaan ja tuhotaan jatkuvasti.

Tämä johti suorituskykyongelmiin, kuten pätkiviin taustoihin ja viiveisiin, erityisesti laitteissa, joissa on rajoitettu laskentateho. 

Siirryimme käyttämään objektipoolausjärjestelmää vastataksemme tähän haasteeseen. Poolauksen avulla voimme "kierrättää" jo luotuja objekteja sen sijaan, että loisimme ja tuhoaisimme niitä jatkuvasti. Kun objekti "tuodaan takaisin eloon", se otetaan pois poolista, ja kun se "kuolee", se palautetaan takaisin pooliin uudelleenkäytettäväksi. 

Tämä muutos vähensi huomattavasti CPU:n ja muistin kuormitusta, mikä paransi pelimme suorituskykyä ja varmisti sulavan pelikokemuksen pelaajille. Poolauksen käyttöönotto oli kriittinen askel pelimme optimoinnissa ja suorituskyvyn parantamisessa.

[[Objektipoolausjärjestelmä]]