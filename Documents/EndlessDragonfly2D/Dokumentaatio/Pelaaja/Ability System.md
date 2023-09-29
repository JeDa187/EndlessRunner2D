# Yleinen Analyysi Ability Systeemistä

## AbilitySO

Tämä on ScriptableObject, joka kuvaa kyvyn. Jokaisella kyvyllä on nimi, kuvaus, kuvake, kesto, vaikutus ja hiukkasefekti. Tämä sisältää myös kyvyn tyypin (kuten "FireBreath").

## InventoryManager

Singleton-luokka, joka hallitsee kerättyjä esineitä. Esineet lisätään inventaarioon ja kun kykyä käytetään, esine poistetaan inventaariosta. Inventaarion maksimikoko on 3 esinettä.

## AbilityManager

Tämä hallitsee pelaajan aktiivisen kyvyn. Voit asettaa nykyisen kyvyn ja käyttää sitä.

## ItemSO

Tämä on ScriptableObject, joka kuvaa kerättävän esineen. Jokaisella esineellä on kyky, joka myönnetään pelaajalle, kun esine kerätään.

## Collectables

Tämä edustaa maailmassa olevaa kerättävää esinettä. Kun pelaaja osuu tähän objektiin, esine lisätään pelaajan inventaarioon.

## DragonflyController

Tämä on pelaajan hahmon ohjauskoodi. Sisältää koodin liikkumiseen, hyppäämiseen ja kykyjen käyttämiseen. Siinä on myös koodi pelaajan tekemiseksi kuolemattomaksi ja kameran nopeuden muuttamiseksi kyvyn käytön aikana.

## InputHandling

Tämä käsittelee pelaajan syötteitä, kuten hiiren napsautuksia ja näppäimistön painalluksia. Se tunnistaa, milloin pelaaja haluaa hyppiä tai käyttää kykyä.

## Yhteenveto

Tämä mekaniikka perustuu kerättävien esineiden keräämiseen ja niiden käyttämiseen. Kun pelaaja kerää esineen, pelaaja saa kyvyn, jonka pelaaja voi halutessaan aktivoida. Käyttämällä eri kykyjä pelaaja saa erilaisia etuja, kuten nopeamman kameran liikkeen mikä toteutetaan SpeedBoost abilitylla ja hetkellisen kuolemattomuuden toiminnon ajaksi. Myös SpeedBoost toiminnon aikana pelaajan scorelle on asetettu kerroin. Tämä tarkoittaa sitä että pelaajan score pointsit nousee kiihtyvällä vaudilla toiminnon aikana.

Jokaisella luokalla on selkeä vastuualue, ja suurin osa toiminnallisuudesta on jaettu eri skripteihin.
# Skriptien Välinen Yhteys

## Collectables

Pelaaja liikkuu pelimaailmassa ja törmää kerättävään esineeseen. Törmäyksen tapahtuessa Collectables-skripti aktivoituu ja tallentaa esineen tiedot (ItemSO).

## ItemSO

Sisältää tietoa kerättävästä esineestä, mukaan lukien minkä kyvyn se antaa pelaajalle. Kun esine kerätään, tämän tiedon perusteella määritetään, mikä kyky pelaajalle annetaan.

## InventoryManager

Kun esine on kerätty, InventoryManager vastaanottaa tiedon ja lisää esineen inventaarioon (joka on rajattu kolmeen esineeseen). InventoryManager tietää myös, mikä esine on aktiivinen ja mitä kykyä pelaaja voi käyttää.

## AbilitySO

Tämä on kykyjen kuvaustiedosto, joka määrittää, mitä tapahtuu, kun kyky aktivoituu. Kun pelaaja päättää käyttää kykyä, AbilityManager käyttää tätä skriptiä määrittämään kyvyn vaikutukset.

## AbilityManager

Tämä skripti on yhteyspiste kykyjen ja pelaajan välillä. Kun pelaaja päättää käyttää kykyä (esim. painamalla tiettyä näppäintä), AbilityManager aktivoituu, ottaa tiedon aktiivisesta kyvystä InventoryManagerista, ja käyttää sitten AbilitySO:ta määrittämään, mitä tapahtuu.

## DragonflyController & InputHandling

Pelaaja antaa syötteitä (esim. näppäimistöltä tai hiireltä) käyttämällä kykyä tai liikkumaan. InputHandling käsittelee nämä syötteet ja kertoo DragonflyControllerille, mitä tehdä (esim. liikkua tai käyttää kykyä). Jos kykyä käytetään, DragonflyController viestii AbilityManagerille ja prosessi jatkuu siitä.

## Toimintaketju Yksinkertaistetusti

1. **Pelaaja**
2. **Collectables**
3. **ItemSO**
4. **InventoryManager**
5. **AbilityManager**
6. **AbilitySO**

**Samanaikaisesti:**

1. **Pelaaja (syötteet)**
2. **InputHandling**
3. **DragonflyController**
4. **AbilityManager**

