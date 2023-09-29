# LEADERBOARD 

Tässä dokumentaatiossa käsittelen työvaiheita ja perusteluja, jotka liittyvät Unity-pelin kehittämiseen käyttäen PlayFab-palvelua. PlayFab on pilvipohjainen backend-palvelu, joka tarjoaa erilaisia ominaisuuksia, kuten pelaajien hallinnan, tilastot, tulostaulukot ja paljon muuta. Kehittämässäni pelissä käytin PlayFabia pelaajien kirjautumiseen, pisteytykseen ja tulostaulukon hallintaan. 

## Asennus ja Konfiguraatio 

### PlayFab SDK:n Asennus 

Ensimmäinen askel oli asentaa PlayFab SDK Unity-projektiini. SDK:n voi ladata Unity Asset Storesta tai GitHubista. SDK sisältää kaikki tarvittavat kirjastot ja työkalut PlayFab-palvelun käyttämiseen Unityssa. 

### PlayFab-tilin ja Pelin Luominen 

Seuraava askel oli luoda tili PlayFab-palveluun ja luoda uusi peli PlayFab-konsolissa. Tämä prosessi on suoraviivainen ja hyvin dokumentoitu PlayFabin verkkosivuilla. 

### API-avaimen Määrittäminen 

PlayFab-konsolissa luodun pelin jälkeen, määritin API-avaimen Unity-projektissani. API-avain on välttämätön, jotta pelini voi kommunikoida PlayFab-palvelun kanssa. 


# Pelaajan Kirjautuminen 

## LoginScene-skriptin Muokkaus 

Seuraava askel oli muokata LoginScene-skriptiäni tarkistamaan 4-kirjaiminen pelaajan nimi ja onko se jo käytössä. Tätä varten käytin PlayFab API:a tarkistaakseni, onko nimi jo olemassa vai ei. 

```csharp
using UnityEngine; 
using UnityEngine.SceneManagement; 
using TMPro; 
using PlayFab; 
using PlayFab.ClientModels; 

public class LoginScene : MonoBehaviour 
{ 
    public TMP_InputField playerNameInputField; 
    public TextMeshProUGUI errorMessage; 

    public void OnContinueButtonClicked() 
    { 
        string playerName = playerNameInputField.text; 
        if (playerName.Length < 4) 
        { 
            errorMessage.text = "Nimen on oltava vähintään 4 kirjainta."; 
        } 
        else 
        { 
            CheckPlayerName(playerName); 
        } 
    } 

    void CheckPlayerName(string playerName) 
    { 
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest { Username = playerName },  
        result => { 
            errorMessage.text = "Nimi on jo olemassa."; 
        }, 
        error => { 
            PlayerPrefs.SetString("PlayerName", playerName); 
            SceneManager.LoadScene("MainMenu"); 
        }); 
    } 
}
```

Tässä koodissa `OnContinueButtonClicked`-metodia kutsutaan, kun pelaaja painaa "Jatka"-painiketta. Metodi tarkistaa, onko syötetty nimi vähintään 4 merkkiä pitkä. Jos se on, se kutsuu `CheckPlayerName`-metodia, joka tarkistaa PlayFabista, onko nimi jo olemassa. Jos nimi on olemassa, näytetään virheviesti. Jos nimi ei ole olemassa, se tallennetaan PlayerPrefs-iin ja siirrytään päävalikkoon.

# Pisteytys ja Tulostaulukko 

## GameManager-skriptin Muokkaus 

GameManager-skriptiä muokattiin lähettämään pelaajan pisteet tulostaulukkoon, kun peli päättyy. 

```csharp
public void GameOver() 
{ 
    gameOverPanel.SetActive(true); 
    scoreTextObject.gameObject.SetActive(false); 
    Time.timeScale = 0; 
    UpdateHighScore(); 

    SendScoreToLeaderboard(score); 
} 

void SendScoreToLeaderboard(int playerScore) 
{ 
    string playerName = PlayerPrefs.GetString("PlayerName"); 
    PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest 
    { 
        Statistics = new List<StatisticUpdate> { 
            new StatisticUpdate { StatisticName = "LeaderboardName", Value = playerScore }, 
        } 
    }, 
    result => { Debug.Log("Pistetilasto päivitetty."); }, 
    error => { Debug.LogError("Pistetilastoa ei voitu päivittää: " + error.ErrorMessage); }); 
}
```

Tässä koodissa `GameOver`-metodia kutsutaan, kun pelaaja kuolee. Metodi asettaa `gameOverPanel`-objektin aktiiviseksi, piilottaa `scoreTextObject`-objektin, pysäyttää pelin ajan, päivittää korkeimman pistemäärän ja lähettää pelaajan pistemäärän tulostaulukkoon. `SendScoreToLeaderboard`-metodi lähettää pelaajan pistemäärän PlayFab-tulostaulukkoon käyttäen pelaajan nimeä ja pistemäärää.

# Tulostaulukon Näyttäminen 

## LeaderboardScene-skriptin Muokkaus 

`LeaderboardScene`-skriptiäni muokattiin hakemaan tulostaulukko PlayFabista ja näyttämään se pelaajalle. 

```csharp
using UnityEngine; 
using UnityEngine.SceneManagement; 
using TMPro; 
using PlayFab; 
using PlayFab.ClientModels; 

public class LeaderboardScene : MonoBehaviour 
{ 
    public Transform entryContainer; 
    public Transform entryTemplate; 
    List<LeaderboardEntry> leaderboardEntriesList; 
    List<Transform> leaderboardEntryTransformList; 

    void Awake() 
    { 
        entryTemplate.gameObject.SetActive(false); 
        GetLeaderboard(); 
    } 

    void GetLeaderboard() 
    { 
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest 
        { 
            StatisticName = "LeaderboardName", 
            StartPosition = 0, 
            MaxResultsCount = 10 
        }, 
        result => { CreateLeaderboardUI(result.Leaderboard); }, 
        error => { Debug.LogError("Virhe haettaessa tulostaulukkoa: " + error.ErrorMessage); }); 
    } 

    void CreateLeaderboardUI(List<PlayerLeaderboardEntry> leaderboard) 
    { 
        leaderboardEntriesList = new List<LeaderboardEntry>(); 

        foreach (PlayerLeaderboardEntry entry in leaderboard) 
        { 
            LeaderboardEntry leaderboardEntry = new LeaderboardEntry { playerName = entry.DisplayName, playerScore = entry.StatValue }; 
            leaderboardEntriesList.Add(leaderboardEntry); 
        } 

        leaderboardEntryTransformList = new List<Transform>(); 
        foreach (LeaderboardEntry leaderboardEntry in leaderboardEntriesList) 
        { 
            Transform entryTransform = Instantiate(entryTemplate, entryContainer); 
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>(); 
            entryRectTransform.anchoredPosition = new Vector2(0, -100 * leaderboardEntryTransformList.Count); 
            entryTransform.gameObject.SetActive(true); 

            entryTransform.Find("PositionText").GetComponent<TMP_Text>().text = (leaderboardEntryTransformList.Count + 1).ToString(); 
            entryTransform.Find("NameText").GetComponent<TMP_Text>().text = leaderboardEntry.playerName; 
            entryTransform.Find("ScoreText").GetComponent<TMP_Text>().text = leaderboardEntry.playerScore.ToString(); 

            leaderboardEntryTransformList.Add(entryTransform); 
        } 
    } 
} 

[System.Serializable] 
public class LeaderboardEntry 
{ 
    public string playerName; 
    public int playerScore; 
}
```

Tässä koodissa `GetLeaderboard`-metodia kutsutaan, kun `LeaderboardScene`-kohtaus ladataan. Metodi hakee tulostaulukon PlayFabista ja kutsuu `CreateLeaderboardUI`-metodia luomaan tulostaulukon käyttöliittymän. `CreateLeaderboardUI`-metodi luo `LeaderboardEntry`-objekteja jokaiselle tulostaulukon merkinnälle ja lisää ne `leaderboardEntriesList`-listaan. Sitten se luo `Transform`-objekteja jokaiselle `LeaderboardEntry`-objektille ja lisää ne `leaderboardEntryTransformList`-listaan. Lopuksi se asettaa jokaisen `Transform`-objektin arvot ja aktivoi ne.

# Lopuksi 

Tämä dokumentaatio käsittelee Unity-pelin kehitystyövaiheita ja perusteluja käyttäen PlayFab-palvelua. Käytin PlayFabia pelaajien kirjautumiseen, pisteytykseen ja tulostaulukon hallintaan. Koodini käsittelee erilaisia tilanteita, kuten virheitä PlayFab API -kutsuissa, pelaajien nimen tarkistamista ja tulostaulukon näyttämistä. Tämä dokumentaatio auttaa ymmärtämään, miten PlayFabia voidaan käyttää Unity-pelien kehittämisessä. 

