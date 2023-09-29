# Latauspaneelin Luominen

Halusin lisätä peliin latauspaneelin, joka näyttäisi pelaajalle, kun peli lataa tietoja taustalla. Tämän oppaan avulla selvitän, miten toteutin tämän ja miten voit käyttää latauspaneelia myös muissa kohtauksissa.

## 1. **LoadingPanelManager -skriptin luonti**

**Tehtävä:** Halusin hallinnoida latauspaneelin näkyvyyttä skriptillä.

**Miten tein sen:**
- Avattuani Unity Editorin, navigoin "Project" paneeliin.
- Klikkasin oikealla "Scripts" kansiossa ja valitsin "Create" > "C# Script".
- Annoin skriptille nimen "LoadingPanelManager".
- Kirjoitin skriptiin koodin latauspaneelin näyttämiseen ja piilottamiseen.

![[Loadingpanel.png]]

## 2. **LoadingPanel Prefab-palikan luonti**

**Tehtävä:** Halusin luoda visuaalisen paneelin latausilmoitusta varten.

**Miten tein sen:**
- Luo uusi UI-paneeli valitsemalla GameObject > UI > Panel.
- Nimeä paneeli "LoadingPanel".
- Lisäsin tekstikomponentin paneeliin ja kirjoitin siihen "Ladataan...".
- Deaktivoituani "LoadingPanel", varmistin, että se ei näkynyt heti kun peli käynnistyi.
- Tein "LoadingPanel"-objektista prefab-palikan vetämällä se projektikansioon.
- Liitin "LoadingPanelManager"-skriptin siihen.

![[Loadingpaneel.png]]
## 3. **LoadingPanel Prefab-palikan lisääminen muihin kohtauksiin**

**Tehtävä:** Halusin, että latauspaneeli voidaan lisätä muihin kohtauksiin yhtä helposti.

**Miten tein sen:**
- Avattuani kohtauksen, johon halusin lisätä latauspaneelin.
- Vedettyäni "LoadingPanel" prefab-palikkaa, sijoitin sen kohtaukseen.
- Varmistin, että LoadingPanelManager-skripti oli liitetty oikein.

**Miten voit käyttää tätä muissa kohtauksissasi:**
Kun olet luonut LoadingPanel prefab-palikan kerran, voit helposti lisätä sen muihin kohtauksiin. Avaa vain haluamasi kohtaus, ja vedä ja pudota prefab-palikka kohtaukseen. Näin voit näyttää tai piilottaa latauspaneelin kutsuessasi ShowLoadingPanel() ja HideLoadingPanel() -metodeja LoadingPanelManager-skriptistä.# Latauspaneelin Animaation Rakentaminen

## Tavoite

Rakentaa latauspaneeli Unityssa, joka näyttää "LOADING..." -tekstin sekä PlayFab-logon, ja animoida ne näytölle tyylikkäästi.

## Työvaiheet

### Alkutilanne
- Alkuperäisessä versiossa oli yksinkertainen skripti, joka pystyi näyttämään ja piilottamaan latauspaneelin.

### TextMeshPro-tekstin lisäys
1. Lisäsin `TMP_Text`-muuttujan, joka viittaa TextMeshPro-tekstiin latauspaneelissa.
2. Halusin, että teksti "LOADING..." animoitaisiin niin, että jokainen kirjain näkyy yksitellen.

### Tekstin animointi
1. Rakensin koroutiinin, joka lisää jokaisen kirjaimen yksitellen "LOADING..." -tekstiin.
2. Koroutiini odottaa pienen hetken ennen seuraavaa kirjainta, jolloin saadaan aikaan animaation vaikutelma.

### PlayFab-logon lisäys
1. Lisäsin `Image`-muuttujan, joka viittaa PlayFab-logoon latauspaneelissa.
2. Halusin, että logo animoitaisiin fadeIn avulla.

### PlayFab-logon animointi
1. Ennen tekstin animointia, rakensin fadeIn-animaation logolle.
2. Animaation aikana logon alfa-arvo (läpinäkyvyys) muuttuu ajan myötä.

### Kirjainten fadeIn-animaatio
1. Jokaisen kirjaimen lisäämisen yhteydessä toteutin yksittäisen kirjaimen fadeIn-animaation koroutiinin avulla.
2. Tämä saavutetaan manipuloimalla TextMeshPro-objektin verteksitietoja.

### Viivästys ennen paneelin piilottamista
1. Kun kaikki kirjaimet ovat näkyvissä, lisäsin 0,2 sekunnin odotusajan ennen latauspaneelin piilottamista.

# Latauspaneelin Animaation Rakentaminen

## Tavoite

Rakentaa latauspaneeli Unityssa, joka näyttää "LOADING..." -tekstin sekä PlayFab-logon, ja animoida ne näytölle tyylikkäästi.

## Työvaiheet

### Alkutilanne
- Alkuperäisessä versiossa oli yksinkertainen skripti, joka pystyi näyttämään ja piilottamaan latauspaneelin.

### TextMeshPro-tekstin lisäys
1. Lisäsin `TMP_Text`-muuttujan, joka viittaa TextMeshPro-tekstiin latauspaneelissa.
2. Halusin, että teksti "LOADING..." animoitaisiin niin, että jokainen kirjain näkyy yksitellen.

### Tekstin animointi
1. Rakensin koroutiinin, joka lisää jokaisen kirjaimen yksitellen "LOADING..." -tekstiin.
2. Koroutiini odottaa pienen hetken ennen seuraavaa kirjainta, jolloin saadaan aikaan animaation vaikutelma.

### PlayFab-logon lisäys
1. Lisäsin `Image`-muuttujan, joka viittaa PlayFab-logoon latauspaneelissa.
2. Halusin, että logo animoitaisiin fadeIn avulla.

### PlayFab-logon animointi
1. Ennen tekstin animointia, rakensin fadeIn-animaation logolle.
2. Animaation aikana logon alfa-arvo (läpinäkyvyys) muuttuu ajan myötä.

### Kirjainten fadeIn-animaatio
1. Jokaisen kirjaimen lisäämisen yhteydessä toteutin yksittäisen kirjaimen fadeIn-animaation koroutiinin avulla.
2. Tämä saavutetaan manipuloimalla TextMeshPro-objektin verteksitietoja.

### Viivästys ennen paneelin piilottamista
1. Kun kaikki kirjaimet ovat näkyvissä, lisäsin 0,2 sekunnin odotusajan ennen latauspaneelin piilottamista.

## Loppusanat

Nyt on toimiva latauspaneeli, joka näyttää tyylikkäältä ja ammattimaiselta. Jokainen komponentti animoituu sulavasti, mikä parantaa käyttäjäkokemusta ja antaa sovellukselle viimeistellyn ilmeen.

