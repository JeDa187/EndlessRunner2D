# Lisääminen Offline Pelitilan Napit ja Toiminnot

## Johdanto:

Tämä opas opastaa lisäämään offline pelitilan, napit ja toiminnot Unity-peliin. Tavoitteena on mahdollistaa pelaajien pelata peliä offline-tilassa ja varmistaa, että peli ei yritä käyttää PlayFab-palvelua offline-tilassa.

## Työvaiheet:

### Lisää "Pelaa offline" -nappi LoginScene-kohtaukseen:

1. **Napin Lisääminen:**
   - Lisäsin uuden UI-napin LoginScene-kohtaukseen Unity-editorissa.
2. **Metodin Linkitys:**
   - Linkitin napin uuteen metodiin LoginScene-skriptissä, joka lataa suoraan päävalikon:

```csharp
public void OnPlayOfflineButtonClicked() 
{ 
    SceneManager.LoadScene("MainMenu"); 
}
```

### Tallenna pelaajan tila:

1. **Pelaajan Tilan Tallennus:**
   - Tallensin pelaajan tilan (online/offline) globaaliin muuttujaan, PlayerPrefs:

```csharp
public void OnContinueButtonClicked() 
{ 
    PlayerPrefs.SetInt("Online", 1); // 1 for online 
}

public void OnPlayOfflineButtonClicked() 
{ 
    PlayerPrefs.SetInt("Online", 0); // 0 for offline 
    SceneManager.LoadScene("MainMenu"); 
}
```

### Tarkista pelaajan tila ennen PlayFab-toimintojen käyttöä:

1. **GameManager-luokassa:**

```csharp
public void GameOver() 
{ 
    if (PlayerPrefs.GetInt("Online") == 1) 
    { 
        SendScoreToLeaderboard(scoreManager.GetScore()); 
    } 
}
```

2. **LeaderboardManager-luokassa:**

```csharp
void Start() 
{ 
    if (PlayerPrefs.GetInt("Online") == 1) 
    { 
        GetLeaderboard(); 
    } 
    else 
    { 
        // Näytä viesti pelaajalle
        for (int i = 0; i < playerNameTexts.Length; i++) 
        { 
            playerNameTexts[i].text = ""; 
            playerScoreTexts[i].text = ""; 
        } 
        playerNameTexts[0].text = "Leaderboard is not available in offline mode."; 
    } 
}
```

## Lopputulos:

Nyt pelaaja voi valita "Pelaa offline" -napin, jolloin peli asettaa pelaajan tilan offline-tilaan ja ohittaa kaikki PlayFab-kirjautumiset ja rekisteröinnit. Jos pelaaja yrittää käyttää pistetaulukkoa offline-tilassa, peli näyttää viestin "Leaderboard is not available in offline mode".

## Lisätään "Kirjaudu ulos" -toiminto:

### Luo "Kirjaudu ulos" -nappi:

1. **Uusi Nappi:**
   - Lisäsin uuden UI-napin päävalikkoon Unity-editorissa.
2. **Metodin Linkitys:**
   - Linkitin napin uuteen metodiin MainMenu-skriptissä:

```csharp
public void OnLogoutButtonClicked() 
{ 
    PlayerPrefs.DeleteKey("Online"); 
    SceneManager.LoadScene("LoginScene"); 
}
```

## Yhteenveto:

Nyt, pelaaja voi kirjautua ulos pelistä ja vaihtaa pelin tilan offline-tilaan käyttämällä "Kirjaudu ulos" -nappia. Tämä toiminto yhdessä "Pelaa offline" -toiminnon kanssa mahdollistaa pelaajien pelaamisen joko online- tai offline-tilassa ja varmistaa, että peli ei yritä käyttää PlayFabin palveluja offline-tilassa.