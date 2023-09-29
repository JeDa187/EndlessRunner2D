## **Viiveen Lisääminen**

Halusin integroida 3 sekunnin viiveen prosessiin, antaen pelaajalle riittävästi aikaa lukea viestin kirjautumisen tai rekisteröitymisen yhteydessä.

### _Invoke-Metodin Käyttö:_

Käytin Unityn `Invoke`-metodia tämän viiveen toteuttamiseen. `Invoke` on `MonoBehaviour`-luokan metodi, joka sallii metodin suorittamisen viiveellä. Se ottaa kaksi parametria: metodin nimen merkkijonona ja viiveen sekunteina, jonka jälkeen metodi suoritetaan. 

```csharp
Invoke("MethodName", 3.0f);
```

Tämä koodi kutsuu `MethodName`-metodia 3 sekunnin kuluttua.

### _UpdateDisplayNameAndMoveToNextScene-Metodi:_

Loin `UpdateDisplayNameAndMoveToNextScene`-metodin, joka noutaa pelaajan nimen `playerNameInputField`-kentästä ja kutsuu sen jälkeen `UpdateDisplayName`-metodia.

```csharp
private void UpdateDisplayNameAndMoveToNextScene()
{
    string playerName = playerNameInputField.text;
    UpdateDisplayName(playerName);
}
```

Tätä metodia kutsutaan `Invoke`-metodin kautta `CheckPlayerName`-metodissa, kun kirjautuminen onnistuu.

### _CreateNewAccountAndMoveToNextScene-Metodi:_

Loimme myös `CreateNewAccountAndMoveToNextScene`-metodin, joka hakee pelaajan nimen ja salasanan `playerNameInputField` ja `passwordInputField` -kentistä ja kutsuu sen jälkeen `CreateNewAccount`-metodia.

```csharp
private void CreateNewAccountAndMoveToNextScene()
{
    string playerName = playerNameInputField.text;
    string password = passwordInputField.text;
    CreateNewAccount(playerName, password);
}
```

Tätä metodia kutsutaan myös `Invoke`-metodin kautta `CheckPlayerName`-metodissa, mikäli tiliä ei löydy ja uusi tili täytyy luoda.

### **Lopputulos:**

Näiden muutosten myötä "Logging in" ja "Creating new user" -viestit näytetään 3 sekunnin ajan ennen seuraavan toiminnon suorittamista, parantaen käyttäjäkokemusta tarjoamalla visuaalista palautetta ja antamalla pelaajalle aikaa prosessoida näytetty viesti.