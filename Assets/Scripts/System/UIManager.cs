using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    //[SerializeField] Sprite icon;
    [SerializeField] TMP_Text collectedItemsText;
    [SerializeField] Button pauseButton;
    [SerializeField] GameObject pauseMenuCanvas;
    [SerializeField] GameObject pauseMenuPanel;
    [SerializeField] GameObject settingsMenuPanel;
    [SerializeField] GameObject MainMenuSecurePanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Aseta Instance arvoksi this (UIManager)
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateCollectedItemsText(); // P‰ivit‰ tekstikentt‰ alussa
                                    // Varmista, ett‰ pauseMenu ja uiPauseButton ovat asetettu inspectorissa
        if (pauseMenuPanel == null)
        {
            Debug.LogError("PauseMenu GameObject ei ole asetettu PauseButtonControllerissa!");
        }

        if (pauseButton == null)
        {
            Debug.LogError("UIPauseButton ei ole asetettu PauseButtonControllerissa!");
        }
        else
        {
            // Kuuntele OnClick-tapahtumaa UI-painikkeelle
            pauseButton.onClick.AddListener(TogglePauseMenu);
        }
    }

    void Update()
    {
        // Tarkista, onko Pause-painiketta painettu n‰pp‰imistˆlt‰ (Q)
        if (Input.GetButtonDown("Pause"))
        {
            TogglePauseMenu(); // Kutsu TogglePauseMenu-metodia, kun painetaan pause-painiketta n‰pp‰imistˆlt‰
        }
    }

    //Voit kutsua t‰t‰ metodia esimerkiksi aina kun ker‰ttyj‰ objekteja p‰ivitet‰‰n
    public void UpdateUI()
    {
        // Tarkista, ett‰ InventoryManagerin instanssi on olemassa
        if (InventoryManager.Instance != null)
        {
            // P‰ivit‰ ker‰ttyjen objektien teksti
            UpdateCollectedItemsText();
        }
    }

    private void UpdateCollectedItemsText()
    {
        List<ItemSO> collectedItems = InventoryManager.Instance.GetCollectedItems();

        StringBuilder sb = new("Collected Items:\n");
        foreach (ItemSO item in collectedItems)
        {
            sb.Append(item.collectableName).Append("\n");
        }

        collectedItemsText.text = sb.ToString();
    }
    void TogglePauseMenu()
    {
        // K‰‰nn‰ pauseMenu-panelin tila p‰‰lle/pois p‰‰lt‰
        pauseMenuPanel.SetActive(!pauseMenuPanel.activeSelf);

        // Pys‰yt‰ aika, jos pauseMenu on p‰‰ll‰, jatka, jos se on pois p‰‰lt‰
        Time.timeScale = (pauseMenuPanel.activeSelf) ? 0 : 1;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1; // Resume the game
        pauseMenuCanvas.SetActive(false); // Hide the pause menu
    }
    public void ShowSettingsMenu()
    {
        settingsMenuPanel.SetActive(true);
        pauseMenuPanel.SetActive(false);
    }
    public void ShowMainMenuOption()
    {
        MainMenuSecurePanel.SetActive(true);
        pauseMenuPanel.SetActive(false);
        
    }
    public void ReturnToPauseMenu()
    {
        settingsMenuPanel.SetActive(false);
        MainMenuSecurePanel.SetActive(false);
        pauseMenuPanel.SetActive(true);
    }
}
