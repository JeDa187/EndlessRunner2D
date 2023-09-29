# Inventaarion ja Kykyjen Hallinta

Tutkimme alkuperäistä ongelmaa, jossa pelaajan inventaario täyttyi kerättävistä tavaroista, mutta pelaaja ei voinut käyttää kuin yhtä tavaraa kerrallaan. Päädyimme tarkastelemaan pelin komponentteja ja niiden toimintaa. Tässä on koko prosessin keskeiset vaiheet:

## Komponenttien erittely

Aluksi pureuduimme pelin komponenttien toimintaan ja niiden väliseen vuorovaikutukseen. Erityisesti kiinnitimme huomioita InventoryManageriin, Collectables-scriptiin, ItemSO-tiedostoihin, AbilitySO-tiedostoihin, InputHandleriin ja AbilityManageriin.

## Inventaarion ja kykyjen hallinta

Kun keskityimme InventoryManageriin, ymmärsimme sen keskeisen roolin kerättyjen tavaroiden ja kykyjen hallinnassa. Tämä komponentti vastasi tavaroiden keräämisestä, inventaarion ylläpidosta ja kykyjen säilyttämisestä.

## Inventaarion rajoittaminen

Ratkaisumme keskiössä oli inventaarion rajoittaminen kolmeen erilaiseen kykyyn samanaikaisesti. Tämä tarkoitti, että pelaaja voi kerätä useita kykyjä, mutta käyttää niitä yksi kerrallaan. Tämä lisäsi pelaajan valinnanvapautta kykyjen suhteen ja teki pelikokemuksesta monipuolisemman.

## Tiedonhallinta ja siisteys

Siistimme tietorakenteita ja poistimme turhia listoja AbilityManagerista. Tämä selkeytti koodiamme ja teki sen ylläpidosta helpompaa.

## Muutokset AddItem-metodiin

Estimme enemmän kuin kolmen tavaran lisäämisen inventaarioon lisäämällä ehdon AddItem-metodiin. Lisäsimme myös ilmoituksen, jos inventaario oli jo täynnä.

## Lopputulos

Pelaaja voi nyt kerätä useita collectableja ja käyttää niitä joustavasti yksi kerrallaan. Inventaarion koko on rajoitettu kolmeen tavaraan samanaikaisesti, mikä lisää pelin taktista syvyyttä. InventoryManagerin keskeinen rooli ratkaisussamme on varmistaa, että pelaaja voi tehokkaasti hallita kerättyjä tavaroita ja kykyjä pelikokemuksensa parantamiseksi.

## Toiminnossa Käytettävät Luokat

### Collectables Script

- **Rooli**: Määrittää kerättävien objektien toiminnallisuuden.
- **Tehtävä**: Tarkkailee, minkä tyyppinen item on kyseessä ja määrittelee, mitä tapahtuu, kun pelaaja kerää kyseisen objektin.

### ItemSO (ScriptableObject)

- **Rooli**: Määrittää yksittäisen itemin tiedot, kuten kyvyn ja kyvyn kuvauksen.
- **Tehtävä**: Tarjoaa tietoa inventaarion hallinnalle ja pelaajan kykyjen säilyttämiselle.

### AbilitySO (ScriptableObject)

- **Rooli**: Säilyttää kaikki kykyihin liittyvät tiedot ja arvot.
- **Tehtävä**: Tarjoaa tietoa pelaajan kykyjen toiminnallisuudelle ja niiden käytölle.

### InputHandler Luokka

- **Rooli**: Reagoi pelaajan syötteisiin ja käynnistää toimintoja, kun pelaaja painaa näppäimistön näppäimiä.
- **Tehtävä**: Käynnistää UseCurrentAbility-funktion, kun pelaaja painaa esimerkiksi "Space"-näppäintä.

### AbilityManager Luokka

- **Rooli**: Hallinnoi pelaajan kykyjä ja niiden käyttöä.
- **Tehtävä**: Seuraa pelaajan kykyjä ja niiden tilaa, käynnistää kykyjen käytön ja mahdollistaa niiden vaihtamisen.

### Pelaajan Controller Luokka

- **Rooli**: Hallinnoi pelaajan hahmon toimintaa ja käyttää kykyjä.
- **Tehtävä**: Sisältää toiminnallisuuden UseAbility, joka käynnistyy, kun pelaaja haluaa käyttää kykyä.

### InventoryManager Luokka

- **Rooli**: Hallinnoi pelaajan inventaariota ja kerättyjä tavaroita ja kykyjä.
- **Tehtävä**: Seuraa kerättyjä tavaroita ja kykyjä, rajoittaa inventaarion koon kolmeen tavaraan ja mahdollistaa tavaran lisäämisen ja poistamisen inventaariosta. Tarjoaa myös toiminnallisuuden kykyjen käyttämiseen ja inventaarion ylläpitämiseen.