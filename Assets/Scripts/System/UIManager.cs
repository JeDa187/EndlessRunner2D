using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{

    //private AbilityManager abilityManager;

    public static UIManager Instance;
    [SerializeField] Sprite icon;
    [SerializeField] TMP_Text collectedItemsText;
    [SerializeField] private Button pauseButton;
    [SerializeField] private GameObject pauseMenuCanvas;
    [SerializeField] GameObject settingsMenuPanel;

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
        pauseButton.onClick.AddListener(ShowPauseMenu);
    }

    //Voit kutsua t‰t‰ metodia esimerkiksi aina kun ker‰ttyj‰ objekteja p‰ivitet‰‰n
    public void UpdateUI()
    {
        UpdateCollectedItemsText();
        // Voit lis‰t‰ muita p‰ivityksi‰ tarpeesi mukaan.
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
    public void ShowPauseMenu()
    {
        Time.timeScale = 0f; // Pause the game
        pauseMenuCanvas.SetActive(true); // Show the pause menu
    }
    public void ResumeGame()
    {
        Time.timeScale = 1; // Resume the game
        pauseMenuCanvas.SetActive(false); // Hide the pause menu
    }
    public void ShowSettingsMenu()
    {
        settingsMenuPanel.SetActive(true);
    }


}
