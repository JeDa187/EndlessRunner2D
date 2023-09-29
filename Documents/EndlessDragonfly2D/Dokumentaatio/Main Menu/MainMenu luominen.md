# Päävalikon luominen Unity 2D -pelissä

Seuraavat askeleet kuvaavat, miten voit luoda päävalikon Unity 2D -pelissä.

## **Aluksi:**

- Loin uuden kohtauksen Unity-projektissani ja nimesin sen "MainMenu".
- Lisäsin kohtaukseen kaksi nappia valitsemalla GameObject-valikosta "UI -> Button". Nimesin nämä napit "Start Game" ja "Quit Game".![[Mainmenucreation.png]]

## **MainMenuController:**

- Loin uuden tyhjän GameObjectin valitsemalla "Create -> Empty" ja nimesin sen "MainMenuController".
- Loin uuden C#-scriptin "Assets"-paneelissa, nimesin sen "MainMenu", ja liitin sen juuri luomaani "MainMenuController"-GameObjectiin.
- Avattuani "MainMenu"-scriptin, kirjoitin seuraavan koodin:

    ```csharp
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class MainMenu : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadScene("GameScene"); // Vaihda tämä pelikohtauksesi nimeen
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
    ```

## **Käytännön soveltaminen:**

- Palasin takaisin Unity-editoriin ja liitin "Start Game" -napin OnClick-tapahtumaan MainMenuController-GameObjectin ja valitsin metodin MainMenu.StartGame.
![[Mainmenucreation (3).png]]
- Tein saman "Quit Game" -napille, mutta valitsin metodin MainMenu.QuitGame. ![[Mainmenucreation (4).png]]

## **Game Over -paneelin muokkaus:**

- Pelikohtauksessani lisäsin "Main Menu" -napin Game Over -paneeliin.
- Liitin "Main Menu" -napin OnClick-tapahtumaan pelini GameManager-scriptin ja lisäsin sinne uuden metodin:

    ```csharp
    public void GoToMainMenu()
    {
        Time.timeScale = 1; // Varmista, että peliaika kulkee normaalisti
        SceneManager.LoadScene("MainMenu");
    }
    ```

- Valitsin "Main Menu" -napin ja liitin sen OnClick-tapahtumaan GameManager-scriptin GoToMainMenu-metodin.
![[Mainmenucreation (2).png]]
## **Scenien järjestäminen:**

- Avattuani Unityn "Build Settings" -ikkunan valitsemalla "File -> Build Settings", raahasin ja pudotin "MainMenu"-kohtauksen ja pelikohtauksen Scenes In Build -paneeliin.
- Tarkistin, että kohtaukset olivat oikeassa järjestyksessä: ensin "MainMenu" ja sitten pelikohtaus.

## **Testaus:**

- Nyt kun testasin peliä, pystyin käyttämään päävalikkoa aloittaakseni pelin tai sulkeakseni sen. Game Over -paneelissa pystyin myös palaamaan päävalikkoon.
- Olin tyytyväinen tuloksiin ja varmistin, että kaikki toimi suunnitellusti.