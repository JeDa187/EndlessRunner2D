**ObstaclePooler Dokumentaatio**

---

### 1. **Yleiskatsaus**:

`ObstaclePooler` on luokka, joka periytyy `ObjectPoolingSystem`-luokasta ja käsittelee esteiden poolauksen ja sijoittamisen peliin. Se tarjoaa modulaarisen tavan käsitellä erityyppisiä esteitä ja niiden sijainnit pelissä.

### 2. **Ominaisuudet**:

- **Esteiden Prefabit**: Sisältää prefabit alaspäin ja ylöspäin suuntautuville esteille sekä niiden variaatioille.
- **Esteiden Sijoittaminen**: Määrittää, missä kohtaa pelialuetta esteet näkyvät.
- **Törmäysten Tunnistaminen**: Varmistaa, että esteet sijoitetaan alueelle, jossa ei ole muita esteitä.
- **Singleton-suunnittelumalli**: Varmistaa, että luokasta on olemassa vain yksi ilmentymä.



### 3. **Modulaarisuus**:

Luokan modulaarinen rakenne mahdollistaa esteiden käsittelyn yhdessä paikassa, mikä tekee siitä joustavamman ja ylläpidettävämmän. `ObstaclePooler` periytyy `ObjectPoolingSystem`-luokasta, joka tarjoaa perustietorakenteet ja toiminnot objektipoolaukselle. Tämä tarkoittaa, että voit helposti lisätä uusia esteiden tyyppejä ja niiden käyttäytymistä, ottamatta huomioon yleistä poolausrakennetta.

### 4. **Käyttö**:

Kun haluat lisätä uuden esteen tai muuttaa olemassa olevaa, sinun tarvitsee vain lisätä tai muokata esteen prefab `ObstaclePooler`-luokassa ja määrittää sen sijoittelu ja käyttäytyminen. Luokka hoitaa loput: poolauksen, sijoittelun ja törmäyksen tunnistamisen.

---

**Yhteenveto**:

`ObstaclePooler` tarjoaa joustavan ja tehokkaan tavan hallita esteitä pelissä. Sen modulaarinen suunnittelu mahdollistaa esteiden helpon lisäämisen, muokkaamisen ja sijoittamisen, samalla kun se pitää huolen suorituskyvystä ja optimoinnista poolauksen avulla.