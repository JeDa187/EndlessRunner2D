# Parallax-taustan implementointi ja rakentaminen

Tämä dokumentti käy läpi vaiheet, jotka ovat tarpeen parallax-taustan implementointiin ja rakentamiseen.

## 1. Suunnittelu

Ennen koodin kirjoittamista tein yksityiskohtaisen suunnitelman siitä, miten halusin parallax-taustan toimivan pelissäni. Suunnittelin eri kerrosten nopeudet ja mietin, kuinka paljon liikettä kukin kerros tarvitsisi näyttääkseen realistiselta ja antaakseen syvyyttä pelimaailmaan.

## 2. Resurssien valinta

Valitsin sopivat taustakuvat, jotka vastaisivat suunnitelmaani. Huolehdin siitä, että kuvat olivat tarpeeksi suuria ja laadukkaita, jotta ne näyttäisivät hyvältä pelissä. Valitsin testikuvia eri kerroksiin luodakseni monikerroksisen parallax-efektin.

## 3. Koodin rakentaminen

### InfiniteParallaxBackground

Tämä luokka luo loputtoman parallaksirullauksen taustalle pelissä. 

#### Muuttujat:

- `enableScrolling`: Tämä on boolean-arvo, joka määrittää, onko parallaksirullaus käytössä vai ei.
- `CameraSpeed`: Tämä määrittää kameran nopeuden, jolla se liikkuu oikealle.
- `LayerScrollSpeeds`: Tämä on taulukko, joka määrittää jokaisen taustakerroksen liikkumisnopeuden.
- `Layers`: Tämä on taulukko, joka sisältää viittaukset GameObject-olioihin, jotka edustavat taustakuvien kerroksia.
- `mainCamera`: Tämä on viittaus pelin pääkameraan.
- `initialPositions`: Tämä on taulukko, joka sisältää alkuperäiset X-koordinaatit jokaiselle taustakerrokselle.
- `spriteWidth`: Tämä on leveys yksittäiselle sprite-kuvanleikkeelle.
- `spriteSizeX`: Tämä on kerroksen X-koordinaatin skaalausarvo.

#### Start()-metodi:

1. Haetaan viittaus pelin pääkameraan.
2. Haetaan viittaus taustakerroksen spriteen ja lasketaan sen leveys ja skaalausarvo.
3. Alustetaan `initialPositions`-taulukko jokaisen taustakerroksen alkuperäisellä X-koordinaatilla.

#### Update()-metodi:

1. Tarkistetaan, onko parallaksirullaus käytössä. Jos ei, lopeta tämän metodin suorittaminen.
2. Liikuta kameraa oikealle `CameraSpeed`-nopeudella.
3. Käy läpi jokainen taustakerros ja tee seuraavat toimenpiteet:
   a. Laske etäisyys, jonka kerros on liikkunut kameran suhteen.
   b. Aseta kerroksen uusi X-koordinaatti sen alkuperäiseen sijaintiin plus laskettu etäisyys.
   c. Tarkista, onko kerros liikkunut niin paljon, että se on mennyt kameran näkymän ulkopuolelle. Jos on, siirrä kerrosta takaisin näkymän sisälle.

Tämän avulla saavutetaan illuusio loputtomasta taustasta, joka rullaa loputtomasti oikealle kameran liikkuessa.

## 7. Integrointi peliin

Integroin parallax-taustan peliin ja varmistin, että se toimi hyvin muiden pelielementtien kanssa. Tein tarvittavat muutokset pelin muuhun koodiin, jotta parallax-tausta toimisi oikein.

## 8. Lopullinen testaus

Tein lopulliset testaukset varmistaakseni, että parallax-tausta toimi oikein ja näytti hyvältä pelissä. Tarkistin myös, että taustan vaihtaminen toimi oikein ja näytti hyvältä.

![[Parallax tausta.png]]
## Jatkojalostus: Vaikeustason lisäys illuusio

### Suunnittelu

Mietin, mitä muutoksia koodiin pitää tehdä, jotta liikuttelun nopeus kasvaisi ajan myötä ja taustat liikkuisivat vasemmalle. Päätin lisätä uuden muuttujan, joka määrittää, kuinka paljon liikuttelun nopeus kasvaa joka sekunti. Lisäksi päätin vaihtaa `LayerScrollSpeeds`-muuttujan arvon negatiiviseksi.

### Koodaus

Muutin koodia lisäämällä uuden muuttujan `SpeedIncreaseRate` ja muuttamalla `LayerScrollSpeeds`-muuttujan arvon negatiiviseksi. Lisäsin myös koodiin toiminnallisuuden, joka kasvattaa `LayerScrollSpeeds`-muuttujan arvoja ajan myötä.

### Testaus

Testasin muutettua koodia varmistaakseni, että parallax-taustan liikuttelun nopeus todella kasvaa ajan myötä ja että taustat liikkuvat vasemmalle. Havaitsin, että muutokset toimivat odotetusti ja saavat aikaan halutun efektin.

---> 
[[Parllax refakturointi ja optimointi]]
