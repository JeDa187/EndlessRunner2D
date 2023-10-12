
**Yleiskatsaus:**

`EnemyPooler` on erikoistunut objektipoolausluokka, joka käsittelee vihollisobjektien luomista ja uudelleenkäyttöä pelissä. Se perii ominaisuudet `ObjectPoolingSystem`-luokasta ja laajentaa niitä vihollisobjektien käsittelyä varten.

---

**Pääpiirteet:**

1. **Pool**: Sisäinen luokka, joka määrittelee yksittäisen objektipoolin rakenteen. Se sisältää:
    - `tag`: Tunniste poolille.
    - `prefab`: GameObject-prefab, joka lisätään pooliin.
    - `size`: Kuinka monta objektia on aluksi tässä poolissa.

2. **pools**: Lista, joka sisältää kaikki määritellyt poolit.

3. **SpawnIntervals**: Parametrit, jotka määrittelevät, kuinka usein viholliset ilmestyvät peliin.

4. **SpawnSettings**: Parametrit, kuten `spawnOffsetX`, `minDeltaY`, `minY` ja `maxY`, jotka määrittelevät, missä ja miten viholliset ilmestyvät pelikentälle.

5. **Singleton Pattern**: Varmistaa, että on vain yksi `EnemyPooler`-instanssi pelissä.

---

**Toimintaperiaate:**

- Luokka alustaa vihollisten objektipoolit käynnistyksen yhteydessä `Awake`-metodissa. Se luo määritellyn määrän vihollisobjekteja kutakin poolia kohti ja lisää ne poolin jonoon.
  
- `SpawnObjects`-korutiini käynnistyy, kun `BeginSpawning`-metodi kutsutaan. Tämä korutiini valitsee satunnaisesti poolin, määrittää sijainnin ja luo vihollisen pelikentälle.

---

**Tärkeimmät metodit:**

1. **Awake**: Suorittaa perustoiminnot, kuten Singleton-alustuksen, kameran viitteen hakemisen ja objektipoolien alustamisen.
2. **InitializePools**: Luo poolit käyttäen `Pool`-luokan määriteltyjä prefab-malleja ja kokoa.
3. **BeginSpawning**: Käynnistää korutiinin, joka hoitaa vihollisten luomisen pelikentälle.
4. **SpawnObjects**: Korutiini, joka luo vihollisia määritellyin väliajoin. Se valitsee satunnaisesti poolin, määrittää sijainnin ja luo vihollisen.
5. **Start** ja **OnDestroy**: Metodit tapahtumien tilausten ja peruutusten hallintaan.

---


