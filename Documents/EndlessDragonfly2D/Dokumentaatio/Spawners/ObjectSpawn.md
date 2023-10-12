# Objektien Spawnaus Pelissä
![[Pasted image 20230929090316.png]]
## ObjectSpawner-skripti

Tämän skriptin tavoitteena on spawnata objekteja pelissä. Jokainen objekti spawnataan satunnaisesti ylä- ja alareunan välillä ja liikkuu vasemmalle, kunnes se ylittää kameran vasemman reunan ja tuhoutuu.

### Työvaiheet:

1. **Skriptin suunnittelu:**
    - Tavoitteena oli spawnata erilaisia objekteja tietyin väliajoin.
    - Varmistetaan, että ne eivät spawnattaisi liian lähelle toisiaan korkeussuunnassa.

2. **Skriptin perusominaisuuksien toteutus:**
    - Luotiin `ObjectSpawner`-luokka.
    - Määritettiin muuttujat `objectsToSpawn`, `spawnInterval` ja `spawnOffsetX`.
    - Luotiin `Start`-metodi ja käynnistettiin `SpawnObjects`-korutiini.

3. **Spawnauksen logiikka:**
    - Toteutettiin spawnauksen logiikka `SpawnObjects`-korutiinissa.
    - Valittiin satunnainen objekti `objectsToSpawn`-taulukosta ja määritettiin sen spawnauksen sijainti.

4. **Objektien tuhoaminen:**
    - Koodi tuhoaa objektit, kun ne ylittävät kameran vasemman reunan.
    - Käytettiin `OnBecameInvisible`-metodia objektin tuhoamiseen automaattisesti, kun se ei ole enää näkyvissä.

5. **Objektien spawnauksen optimointi:**
    - Tehtiin muutoksia spawnauksen logiikkaan varmistamaan, että objektit spawnataan riittävän kaukana toisistaan korkeussuunnassa.

## ObjectMover-skripti
![[Näyttökuva 2023-09-29 090551.png]]
### Työvaiheet:

1. **Skriptin suunnittelu:**
    - Tavoitteena oli liikuttaa objekteja vasemmalle ja tuhota ne, kun ne eivät ole enää näkyvissä.

2. **Skriptin perusominaisuuksien toteutus:**
    - Luotiin `ObjectMover`-luokka.
    - Määriteltiin muuttuja `moveSpeed`.

3. **Objektien liikuttaminen:**
    - Toteutettiin objektien liikuttaminen `Update`-metodissa.

4. **Objektien tuhoaminen:**
    - Koodi tuhoaa objektit, kun ne ylittävät kameran vasemman reunan.

5. **Testaus ja optimointi:**
    - Varmistettiin, että pelissä objektit liikkuvat vasemmalle ja tuhoutuvat, kun ne ylittävät kameran vasemman reunan.


[[Uusi Enemy Pooler]]

**Siirtyminen Spawnereista Poolaukseen: Lyhyt Selvitys**

Pelissämme käytimme aiemmin spawnereita objektien, kuten vihollisten ja esteiden, luomiseen. Tämä tarkoitti, että joka kerta kun tarvitsimme uuden objektin, loimme sen tyhjästä ja tuhosimme sen, kun emme enää tarvinneet sitä. Vaikka tämä menetelmä on yksinkertainen ja helppokäyttöinen, se ei ole tehokas, varsinkin kun pelissä on paljon objekteja, jotka luodaan ja tuhotaan jatkuvasti.

Tämä johti suorituskykyongelmiin, kuten pätkiviin taustoihin ja viiveisiin, erityisesti laitteissa, joissa on rajoitettu laskentateho. 

Siirryimme käyttämään objektipoolausjärjestelmää vastataksemme tähän haasteeseen. Poolauksen avulla voimme "kierrättää" jo luotuja objekteja sen sijaan, että loisimme ja tuhoaisimme niitä jatkuvasti. Kun objekti "tuodaan takaisin eloon", se otetaan pois poolista, ja kun se "kuolee", se palautetaan takaisin pooliin uudelleenkäytettäväksi. 

Tämä muutos vähensi huomattavasti CPU:n ja muistin kuormitusta, mikä paransi pelimme suorituskykyä ja varmisti sulavan pelikokemuksen pelaajille. Poolauksen käyttöönotto oli kriittinen askel pelimme optimoinnissa ja suorituskyvyn parantamisessa.

[[Objektipoolausjärjestelmä]]

