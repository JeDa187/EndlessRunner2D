## **Johdanto**

Tarkoituksena oli luoda peliin toiminto, jossa pelaajat voivat kirjautua sisään, luoda uusia tilejä ja nähdä pisteytyksensä leaderboardissa. Projekti käyttää PlayFab-palvelua pelaajien kirjautumiseen, uusien tiliöiden luomiseen ja pelaajien pisteytyksen näyttämiseen leaderboardissa.

Alkuperäisessä versiossa pelaajat voivat kirjautua sisään käyttäen neljän merkin pituista nimeä. Tässä muodostui ongelma, jos jotkut pelaajat käyttävät samaa nimimerkkiä. Päivitetyssä versiossa lisäsin salasanan vaatimuksen, jotta samalla nimimerkillä ei voi kirjautua useampi pelaaja. Lisäksi päivitetyssä versiossa näytetään viesti "Creating new user" tai "Logging in" riippuen siitä, luodaanko uusi käyttäjä vai kirjaudutaanko olemassa olevalla käyttäjällä.

## **Työvaiheet**

### _Pelaajan Kirjautuminen ja Rekisteröinti_

Alkuperäisessä versiossa pelaajat voivat kirjautua sisään peliin käyttäen neljän merkin pituista nimeä. Päivitetyssä versiossa pelaajan on annettava neljän merkin pituinen nimi ja vähintään viiden merkin pituinen salasana. Pelaajan nimi ja salasana tarkistetaan ja lähetetään PlayFab-palveluun CheckPlayerName-metodilla. Jos kirjautuminen onnistuu, pelaaja siirretään päävalikkoon. Jos käyttäjää ei löydy, yritetään luoda uusi käyttäjä CreateNewAccount-metodilla. Jos rekisteröinti onnistuu, pelaaja siirretään päävalikkoon.

### _Pelin Hallinta_

`GameManager`-luokka huolehtii pelin logiikasta. Tässä luokassa on metodi `GameOver`, jota kutsutaan kun peli päättyy. Metodi näyttää "Game Over" -paneelin, piilottaa pisteet, pysäyttää ajan ja päivittää korkeimman pistemäärän. Tämän jälkeen se lähettää pelaajan pistemäärän leaderboardiin `SendScoreToLeaderboard`-metodilla.

`SendScoreToLeaderboard`-metodi lähettää pelaajan pistemäärän PlayFab-palveluun. Metodi ottaa pelaajan nimen `PlayerPrefs`-luokasta ja lähettää sen ja pistemäärän PlayFab-palveluun `UpdatePlayerStatistics`-metodilla.

### _Leaderboard_

`LeaderboardManager`-luokka näyttää kymmenen parasta pistemäärää. Luokka hakee leaderboard-tiedot PlayFab-palvelusta `GetLeaderboard`-metodilla ja näyttää ne ruudulla.

## **Koodin Rakenne**

Toiminnossa on kolme pääluokkaa: `LoginScene`, `GameManager` ja `LeaderboardManager`.

### _LoginScene-luokka_

`LoginScene`-luokassa on kolme päämetodia:

- `OnContinueButtonClicked`: Tämä metodi kutsutaan, kun "Continue" -painiketta painetaan. Metodi ottaa pelaajan nimen ja salasanan, tarkistaa ne ja kutsuu `CheckPlayerName`-metodia.
- `CheckPlayerName`: Tämä metodi tarkistaa pelaajan nimen ja salasanan PlayFab-palvelusta. Jos kirjautuminen onnistuu, pelaaja siirretään päävalikkoon. Jos käyttäjää ei löydy, yritetään luoda uusi käyttäjä `CreateNewAccount`-metodilla.
- `CreateNewAccount`: Tämä metodi luo uuden käyttäjän PlayFab-palveluun. Jos rekisteröinti onnistuu, pelaaja siirretään päävalikkoon.

### _GameManager-luokka_

`GameManager`-luokassa on kaksi päämetodia:

- `GameOver`: Tämä metodi kutsutaan kun peli päättyy. Metodi näyttää "Game Over" -paneelin, piilottaa pisteet, pysäyttää ajan ja päivittää korkeimman pistemäärän. Tämän jälkeen se lähettää pelaajan pistemäärän leaderboardiin `SendScoreToLeaderboard`-metodilla.
- `SendScoreToLeaderboard`: Tämä metodi lähettää pelaajan pistemäärän PlayFab-palveluun. Metodi ottaa pelaajan nimen `PlayerPrefs`-luokasta ja lähettää sen ja pistemäärän PlayFab-palveluun `UpdatePlayerStatistics`-metodilla.

### _LeaderboardManager-luokka_

`LeaderboardManager`-luokassa on kaksi päämetodia:

- `Start`: Tämä metodi kutsutaan kun skripti alustetaan. Metodi kutsuu `GetLeaderboard`-metodia.
- `GetLeaderboard`: Tämä metodi hakee leaderboard-tiedot PlayFab-palvelusta ja näyttää ne ruudulla.

