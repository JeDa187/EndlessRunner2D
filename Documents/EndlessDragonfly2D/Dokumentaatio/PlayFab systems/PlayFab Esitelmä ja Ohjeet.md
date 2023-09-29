## PlayFab Leaderboardin, Käyttäjähallinnan ja Tietokantatoiminnan Käyttö Unity 2D -Pelissä

---

### Johdanto:

Tässä tutustutaan siihen, miten voi toteuttaa leaderboardin, käyttäjähallinnan ja tietokantatoiminnon Unity 2D -pelissä käyttämällä PlayFab-palvelua. Tässä esitelmässä kerron, kuinka luoda ja hallita leaderboardia, lisätä pisteitä, hakea leaderboard-tietoja, sekä käsitellä käyttäjän luomista ja kirjautumista. Lisäksi käymme läpi tietokantatoimintoja, joita PlayFab tarjoaa.

---

### 1. PlayFab:

- **Mikä on PlayFab?**
  - PlayFab on Microsoftin tarjoama pilvipalvelu, joka auttaa pelinkehittäjiä rakentamaan, julkaisemaan ja skaalaamaan pelejään.
  
- **Leaderboardin Luominen PlayFabissa:**
  - Luo PlayFab-tili ja luo uusi peliprojekti.
  - Luo Leaderboard (Statistic) PlayFab-portaalissa määrittämällä sen nimi ja muut asetukset.

---

### 2. Käyttäjän Luominen ja Kirjautuminen:

#### 2.1 Käyttäjän Rekisteröinti:

- Rekisteröi pelaajat luomalla uusi PlayFab-käyttäjätili.
- Käytä `RegisterPlayFabUser` API-kutsua rekisteröintiin.
- Esimerkkikoodi rekisteröintiin:
  ```csharp
  PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
  {
      Username = username,
      Password = password,
      Email = email
  }, result => {
      // Onnistunut rekisteröinti
  }, error => {
      // Virhekäsittely
  });
  ```

#### 2.2 Käyttäjän Kirjautuminen:

- Kirjaudu sisään PlayFab-tilille.
- Käytä `LoginWithPlayFab` API-kutsua kirjautumiseen.
- Esimerkkikoodi kirjautumiseen:
  ```csharp
  PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
  {
      Username = username,
      Password = password
  }, result => {
      // Onnistunut kirjautuminen
  }, error => {
      // Virhekäsittely
  });
  ```

---

### 3. Pisteiden Lisääminen Leaderboardiin:

- Käytä `UpdatePlayerStatistics` -API-funktiota lisätäksesi pisteet leaderboardiin.
- Esimerkkikoodi pisteiden lisäämiseen:
  ```csharp
  PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
  {
      Statistics = new List<StatisticUpdate>
      {
          new StatisticUpdate { StatisticName = "Score", Value = newScore }
      }
  }, result => {
      // Pisteiden päivitys onnistui
  }, error => {
      // Virhekäsittely
  });
  ```

---

### 4. Leaderboardin Tietojen Haku:

- Hae leaderboardin tiedot kutsumalla `GetLeaderboard` -API-funktiota.
- Esimerkkikoodi leaderboardin tietojen hakuun:
  ```csharp
  PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
  {
      StatisticName = "Score",
      StartPosition = 0,
      MaxResultsCount = 10
  }, result => {
      foreach (var item in result.Leaderboard)
      {
          // Käsittely täällä
      }
  }, error => {
      // Virhekäsittely
  });
  ```

---

### 5. Pelaajan Sijoituksen ja Tietojen Haku:

- Hae pelaajan sijoitus ja tiedot leaderboardista.
- Käytä `GetPlayerStatistics` API-funktiota siihen.
- Esimerkkikoodi pelaajan tietojen hakuun:
  ```csharp
  PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(), result => {
      foreach (var eachStat in result.Statistics)
      {
          // Käsittely täällä
      }
  }, error => {
      // Virhekäsittely
  });
  ```

---

### 6. Tietokantatoiminnot PlayFabissa:

#### 6.1 Tietomallin Luominen:

- Voit määrittää tarvitsemasi tietomallin PlayFab-portaalissa.

#### 6.2 Tietojen Tallennus:

##### Mitä Tietoja Voidaan Tallentaa?

- **Pelaajan Profiilitiedot:**
  - Pelaajan nimi, sähköposti, pisteet, saavutukset, ja muut perustiedot.
- **Pelaajan Pelitiedot:**
  - Esimerkiksi hahmon taso, varusteet, aseet, ja muut pelikohtaiset tiedot.
- **Pelin Tiedot:**
  - Pelikohtaiset asetukset, konfiguraatiot, ja muut tiedot.
- **Muut Tiedot:**
  - Voit tallentaa muitakin tietoja tarpeidesi mukaan.
  
Voit tallentaa lähes minkä tahansa tyyppisiä tietoja, mukaan lukien mutta ei rajoittuen merkkijonot, numerot, listat, ja sanakirjat.

- Käytä `UpdateUserData` -kutsua tietojen tallennukseen.
    ```csharp
    PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
    {
        Data = new Dictionary<string, string>
        {
            {"key", "value"}
        }
    }, result => {
        // Onnistunut tallennus
    }, error => {
        // Virhekäsittely
    });
    ```

#### 6.3 Tietojen Haku:

- Hae pelaajien tietoja `GetUserData` -kutsulla.
    ```csharp
    PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result => {
        var value = result.Data["key"].Value;
        // Käsittely täällä
    }, error => {
        // Virhekäsittely
    });
    ```

#### 6.4 Tietojen Päivitys ja Poisto:

- Päivitä tai poista tietoja samalla `UpdateUserData` -kutsulla.


---

### Yhteenveto:

Tämä esitelmä antoi perusteellisen kuvan siitä, miten voit hyödyntää PlayFabia leaderboardien ja käyttäjähallinnan toteuttamiseen Unity 2D -pelissä. Huomioi, että API-kutsut vaativat, että pelaaja on kirjautunut sisään, ja muista aina käsitellä mahdolliset virhetilanteet. Tämä antaa perusnäkymän siihen, miten voit käyttää PlayFab-palvelua tietokantana Unity-peliprojektissasi. Muista tutustua PlayFab-dokumentaatioon tarkempien tietojen ja esimerkkien saamiseksi.



