# ONGELMANRATKAISU

## Ongelman Kuvaus

Projektin aikana havaittiin ongelma: pelaajien nimet eivät ilmestyneet online tulostaulukkoon, vaikka heidän pisteensä näkyivät oikein. Tulostaulukon odotettiin näyttävän sekä pelaajien nimet että pisteet, mutta vain pisteet näkyivät, nimet puuttuivat.

## Ongelman Syynä

Ongelman juuri löytyi PlayFabista. PlayFabissa pelaajien `DisplayName` on eri asia kuin pelaajien `PlayFabId` tai `CustomId`. Koodissani yritin näyttää pelaajien nimet ja pisteet käyttäen `DisplayName`-kenttää. Jos `DisplayName`-kenttää ei ole erikseen määritetty, se voi jäädä tyhjäksi, mikä selitti miksi vain pisteet näkyivät.

## Ongelmanratkaisu

Ongelma ratkaistiin asettamalla pelaajien `DisplayName` arvoksi heidän `CustomId` arvonsa heidän kirjautuessaan sisään. Tämä muutos tehtiin `LoginScene`-luokassa seuraavalla koodilla:

```csharp
private void CheckPlayerName(string playerName) 
{ 
    PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest 
    { 
        CustomId = playerName, 
        CreateAccount = true, 
    }, result => 
    { 
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest 
        { 
            DisplayName = playerName, 
        }, nameResult => 
        { 
            PlayerPrefs.SetString("PlayerName", playerName); 
            SceneManager.LoadScene("MainMenu"); 
        }, nameError => 
        { 
            errorMessage.text = nameError.ErrorMessage; 
        }); 
    }, error => 
    { 
        errorMessage.text = error.ErrorMessage; 
    }); 
} 

public void OnContinueButtonClicked() 
{ 
    string playerName = playerNameInputField.text; 
    if (playerName.Length != 4) 
    { 
        errorMessage.text = "Nimen on oltava tasan 4 kirjainta."; 
    } 
    else 
    { 
        CheckPlayerName(playerName); 
    } 
} 
```

## 1: `UpdateUserTitleDisplayName`-metodin Kutsu

```csharp
PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest 
{ 
    DisplayName = playerName, // Tämä rivi on keskeinen: täällä asetetaan DisplayName.
}, ...
```

### 1.Selitys:

Tässä koodinpätkässä `UpdateUserTitleDisplayName`-metodia kutsutaan asettamaan pelaajan `DisplayName` samaan arvoon kuin heidän `CustomId`. Tämä on keskeinen osa ongelman ratkaisua, sillä jos `DisplayName`-kenttää ei ole asetettu, se jää tyhjäksi ja näin ollen pelaajien nimet eivät näy tulostaulukossa. Asettamalla `DisplayName` arvo tässä kohtaa varmistetaan, että pelaajan nimi näkyy tulostaulukossa heidän pisteidensä kanssa.

## 2: `CheckPlayerName`-Metodi

```csharp
private void CheckPlayerName(string playerName) 
{ 
    ...
    PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest 
    { 
        CustomId = playerName, 
        CreateAccount = true, 
    }, result => 
    { 
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest 
        { 
            DisplayName = playerName, 
        }, ...
    ...
}
```

### 2. Selitys:

`CheckPlayerName`-metodi on vastuussa pelaajan kirjautumisesta ja heidän `DisplayName`-kenttänsä asettamisesta. Ensiksi, metodi kirjaa pelaajan sisään käyttäen `LoginWithCustomID`-metodia, jossa `CustomId` asetetaan pelaajanimen mukaiseksi. Tämän jälkeen kutsutaan `UpdateUserTitleDisplayName`-metodia asettamaan pelaajan `DisplayName` samaan arvoon kuin `CustomId`. Tämä varmistaa, että `DisplayName` on asetettu oikein, ja että pelaajan nimi näkyy tulostaulukossa.

Yhdessä nämä koodinpätkät muodostavat ratkaisun ongelmaan, jossa pelaajien nimet eivät näy tulostaulukossa, vaikka heidän pisteensä näkyvät. Ratkaisu on yksinkertaisesti varmistaa, että `DisplayName`-kenttä on asetettu oikein, kun pelaaja kirjautuu sisään.
## Lopputulos

Tämän muutoksen myötä pelaajien nimet näkyvät oikein tulostaulukossa, kuten myös heidän pistemääränsä. Ratkaisu varmistaa, että pelaajien `DisplayName` asetetaan oikein heidän kirjautuessaan, jotta se näkyy oikein tulostaulukossa.



**HUOM!**  

Seuraavaksi on ratkaistava ongelma, että useampi henkilö ei voi pelata saman nimimerkin alaisuudessa.