
### **1. Johdanto**

Tämä dokumentaatio opastaa käyttäjiä luomaan `QuitManager`-järjestelmän Unityssä. Järjestelmä näyttää pelaajalle varmistuspaneelin, kun he yrittävät lopettaa pelin ESC-näppäimellä. Varmistuspaneeli esittää ensin kysymyksen haluaako pelaaja todella lopettaa pelin ja tarjoaa toisen mahdollisuuden vahvistaa valinta, jotta vahingossa tapahtuvat pelin lopetukset voidaan välttää.

---

### **2. QuitManager Singletonin Luominen**

Aloitetaan luomalla `QuitManager`-singleton, joka hallinnoi lopetuspaneelin näyttämistä ja toiminnallisuutta.

**Koodi:**
```csharp
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuitManager : MonoBehaviour
{
    public static QuitManager Instance { get; private set; }

    // ... [Koodi jatkuu] ...
}
```

---

### **3. QuitPanel-prefabin luominen ja TMP-käyttö**

Jatka luomalla itse varmistuspaneeli Unity Editorissa:

1. **Luo Canvas**: Luo uusi `Canvas` nimeltään `QuitCanvas`.
2. **Luo Panel**: Luo `Panel` QuitCanvasin sisälle. Tätä paneelia kutsutaan `QuitPanel`iksi.
3. **Lisää TMP-teksti**: Luo TextMeshPro-teksti QuitPaneliin. Alkutekstiksi asetetaan: "Do you want to exit the game?"
4. **Lisää painikkeet**: Lisää QuitPaneliin kaksi TextMeshPro-painiketta: "Yes" ja "No".

---

### **4. ESC-näppäimen kuuntelu**

Seuraavaksi lisäämme koodin osan, joka kuuntelee, kun pelaaja painaa ESC-näppäintä ja avaa `QuitPanel`-paneelin.

**Koodi:**
```csharp
private void Update()
{
    if (Input.GetKeyDown(KeyCode.Escape))
    {
        ToggleQuitPanel();
    }
}
```

---

### **5. QuitManagerin integrointi peliin**

Seuraavaksi liitämme `QuitManager`-koodin Unity-editorissa:

1. Luo tyhjä GameObject ja nimeä se `QuitManager`iksi.
2. Liitä `QuitManager`-scripti luomaasi `QuitManager` GameObjectiin.
3. Luo `QuitPanel`ista prefab ja raahaa se `QuitManager`-scriptin `quitPanelPrefab`-muuttujaan.

---

### **6. Käyttäjän toisen tason varmistus**

Varmistetaan, että pelaaja saa toisen mahdollisuuden vahvistaa pelin lopetus.

**Koodi:**
```csharp
private bool isSecondConfirmation = false;
public TMP_Text quitMessage;

// ... [Koodi jatkuu] ...

private void HandleYesButton()
{
    if (!isSecondConfirmation)
    {
        isSecondConfirmation = true;
        if (quitMessage)
            quitMessage.text = "Are you sure?";
    }
    else
    {
        QuitGame();
    }
}
```

Kun pelaaja painaa ensimmäisen kerran "Yes"-painiketta, teksti muuttuu ja kysyy: "Are you sure?". Jos pelaaja painaa "Yes" uudelleen, peli suljetaan. Mikäli "No" -painiketta painetaan tässä vaiheessa, paneeli sulkeutuu ja toisen tason varmistus nollautuu.

---

Tämä dokumentaatio auttaa sinua luomaan tehokkaan ja käyttäjäystävällisen QuitManager-järjestelmän Unity-projektiisi. Voit jatkossa muokata ja laajentaa tätä järjestelmää pelisi tarpeiden mukaan.