# Score-järjestelmän kehittäminen

## Projektin tavoite

Tavoitteenamme oli kehittää pistejärjestelmä peliprojektiin. Pisteet lisääntyvät kahdella pisteellä sekunnissa niin kauan kuin pelaajan hahmo on elossa. Pisteytys alkaa heti, kun aloituslaskuri saavuttaa nollan ja peli alkaa. Halusimme myös näyttää pelin aikana kertyneet pisteet näytön yläreunassa ja tallentaa istunnon korkeimman pistemäärän, joka näytetään, kun peli päättyy.

## Työvaiheet

### Score-järjestelmän suunnittelu

Suunnittelimme järjestelmän, joka laskee pelaajan pisteitä kahdella pisteellä sekunnissa. Tämä toteutettiin `UpdateScore()`-rutiinilla, joka lisää pisteitä aina 0,5 sekunnin välein.

### Pisteytyksen aloittaminen pelin alkaessa

Aloitimme pisteytyksen heti kun aloituslaskuri päättyy ja peli alkaa. Tämä toteutettiin `CountdownToStart()`-rutiinilla.

### Pistetietojen näyttäminen pelaajalle

Näytämme pelin aikana kertyneet pisteet näytön yläreunassa ja näytämme istunnon korkeimman pistemäärän pelin päättyessä. Tämä toteutettiin käyttämällä Unityn TextMeshPro-kirjastoa ja luomalla `scoreTextObject`- ja `highScoreTextObject`-tekstikomponentit.

### High Scoren tallentaminen

Tallensimme istunnon korkeimman pistemäärän ja näytimme sen, kun peli päättyy. Tämä toteutettiin `GameOver()`-rutiinilla, joka tarkistaa, onko pelaajan pistemäärä suurempi kuin aiempi korkein pistemäärä, ja päivittää korkeimman pistemäärän tarvittaessa.

### Pistetietojen piilottaminen pelin päättyessä

Piilotimme pelin aikana näkyvät pistetiedot, kun peli päättyy. Tämä toteutettiin myös `GameOver()`-rutiinilla.

## Kehitystyön tulokset

Onnistuimme kehittämään pisteytysjärjestelmän, joka laskee pelaajan pisteitä kahdella pisteellä sekunnissa niin kauan kuin hahmo on elossa. Järjestelmä tallentaa istunnon korkeimman pistemäärän PlayerPrefsiin ja näyttää sen, kun peli päättyy. Järjestelmä toimii tässä vaiheessa luotettavasti ja täyttää vaatimukset.
