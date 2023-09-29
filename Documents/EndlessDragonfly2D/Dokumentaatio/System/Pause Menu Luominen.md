
# Taukomenun Lisääminen Peliin

Taukomenun lisääminen peliin on tärkeää pelaajille, jotta he voivat keskeyttää pelin ja palata myöhemmin takaisin peliin. Seuraavassa on esitetty vaiheet taukomenun lisäämiseksi.

## 1. **Taukomenupaneelin Luominen**

   ### a) Luo uusi "Canvas" GameObject (PauseMenuCanvas).
   
   ### b) Luo "Panel" GameObject Canvasin alapuolelle (PauseMenuPanel).
   
   ### c) Luo kaksi uutta "Button" GameObjectia PauseMenuPanelin alapuolelle ja nimeä ne "ContinueButton" ja "MainMenuButton".
   
   ### d) Aseta nappien tekstiksi "Continue" ja "Main Menu".

## 2. **PauseButtonin Luominen**

   ### a) Luo uusi "Button" GameObject Canvasin alapuolelle.
   
   ### b) Nimeä se "PauseButton" ja aseta tekstiksi "Pause".
   
   ### c) Luo uusi C#-skripti "PauseButtonController".

   ```csharp
   using UnityEngine;
   using UnityEngine.UI;

   public class PauseButtonController : MonoBehaviour
   {
       [SerializeField] private Button pauseButton;
       [SerializeField] private GameObject pauseMenuPanel;

       private void Start()
       {
           pauseButton.onClick.AddListener(ShowPauseMenu);
       }

       private void ShowPauseMenu()
       {
           Time.timeScale = 0f; // Pause the game
           pauseMenuPanel.SetActive(true); // Show the pause menu
       }
   }
   ```

   ### d) Liitä tämä skripti "PauseButton" GameObjectiin.
   
   ### e) Aseta "PauseButton"-muuttujaan viite juuri luotuun nappiin ja "PauseMenuPanel"-muuttujaan viite taukomenupaneeliin.

## 3. **GameManager-Skriptin Muokkaus**

   ### a) Lisää seuraavat GameManager-skriptiin:

   ```csharp
   [SerializeField] private GameObject pauseMenuPanel; // UI panel displayed when game is paused

   public void ResumeGame()
   {
       Time.timeScale = 1; // Resume the game
       pauseMenuPanel.SetActive(false); // Hide the pause menu
   }
   ```

   ### b) Aseta "PauseMenuPanel"-muuttujaan viite taukomenupaneeliin.

## 4. **Napit Yhdistetään Toimintoihin**

   1. **"ContinueButton":**
      - Valitse "ContinueButton".
      - Lisää "On Click()" -listalle uusi toiminto.
      - Aseta kohde olemaan GameManager.
      - Aseta toiminnoksi "ResumeGame".

   2. **"MainMenuButton":**
      - Valitse "MainMenuButton".
      - Lisää "On Click()" -listalle uusi toiminto.
      - Aseta kohde olemaan GameManager.
      - Aseta toiminnoksi "GoToMainMenu".

## **Lopputulos**

Kun pelaaja painaa "Pause"-nappia, peli keskeytyy ja näyttöön tulee taukomenupaneeli. Pelaaja voi joko jatkaa peliä painamalla "Continue" tai palata päävalikkoon painamalla "Main Menu".
