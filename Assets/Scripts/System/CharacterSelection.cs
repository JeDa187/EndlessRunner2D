using UnityEngine;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;

public class CharacterSelection : MonoBehaviour
{
    public static CharacterSelection Instance { get; private set; }

    public Sprite[] characterSprites; // Array of character sprites
    public int selectedCharacterIndex = 0; // Index of currently selected character
    public bool[] characterLocked = { false, true, true }; // Array indicating if a character is locked

    public Button[] characterButtons; // Array of character selection buttons
    public Color equippedColor = Color.gray; // Color to indicate a character is equipped

    public LoadingPanelManager loadingPanelManager; // Referenssi LoadingPanelManager-skriptiin

    public Color[] buttonColors; // Array to store default colors for each button

    public TMP_Text[] characterUnlockTexts; // Array of text fields for character unlock status

    public TMP_Text infoText; // Text field for displaying information to the player
    public Image[] characterImages;





    private void Awake()
    {
        // Singleton pattern to ensure only one instance of CharacterSelection exists
        if (Instance == null || Instance.gameObject.scene != this.gameObject.scene)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (this != Instance)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Start()
    {
        // Check if the game is in offline mode
        if (PlayerPrefs.GetInt("Online", 1) == 0)
        {
            // Lock all characters except the first one
            characterLocked[0] = false;
            characterLocked[1] = true;
            characterLocked[2] = true;
            UpdateCharacterTexts();
            UpdateButtonColors();
            loadingPanelManager.HideLoadingPanel();

            // Update info text
            infoText.text = "Additional characters are unavailable in offline mode, Press 'Continue' to play.";
        }
        else
        {
            infoText.text = "Select a character to play.";
            loadingPanelManager.ShowLoadingPanel(); // Kutsu LoadingPanelManagerin metodia
            FetchPlayerHighScore(); // Fetch the player's high score
        }
    }


    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Load the main menu scene
    }

    public void EquipCharacter(int index)
    {
        // Equip the character if it's not locked
        if (index < characterSprites.Length && !characterLocked[index])
        {
            selectedCharacterIndex = index;
            Debug.Log($"Character with index {index} equipped.");
            UpdateButtonColors();
            UpdateCharacterTexts();

            infoText.text = "Character equipped. Press 'Continue' to play."; 
        }
        else if (characterLocked[index])
        {
            Debug.Log($"Character with index {index} is locked.");
            infoText.text = "This character is still waiting in the shadows, Achieve a higher score to unlock.";
        }
        else
        {
            Debug.Log($"Invalid index {index}. Character not equipped.");
        }
    }

    void FetchPlayerHighScore()
    {
        // Fetch the player's high score from PlayFab
        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(),
        result => {
            bool scoreFound = false;

            foreach (var eachStat in result.Statistics)
            {
                if (eachStat.StatisticName == "PlatformScore")
                {
                    PlayerPrefs.SetInt("PlayerScore", eachStat.Value);
                    scoreFound = true;
                    break;
                }
            }

            if (!scoreFound)
            {
                PlayerPrefs.SetInt("PlayerScore", 0);
            }

            UpdateCharacterLocks();
            UpdateCharacterTexts();
            loadingPanelManager.HideLoadingPanel(); // Hide the loading panel once data is fetched
        },
        error => {
            Debug.LogError(error.GenerateErrorReport());
            loadingPanelManager.HideLoadingPanel(); // Hide the loading panel in case of an error
        });
    }

    void UpdateCharacterLocks()
    {
        // Update which characters are locked based on the player's score
        int playerScore = PlayerPrefs.GetInt("PlayerScore", 0);

        characterLocked[0] = false;

        if (playerScore >= 1000)
        {
            characterLocked[1] = false;
        }
        if (playerScore >= 2000)
        {
            characterLocked[2] = false;
        }

        UpdateCharacterTexts();
        UpdateButtonColors();
    }

    void UpdateButtonColors()
    {
        // Update the color of the character buttons and their images
        for (int i = 0; i < characterButtons.Length; i++)
        {
            if (i == selectedCharacterIndex)
            {
                characterButtons[i].image.color = equippedColor;
                characterImages[i].color = Color.white; // restore the image's original color
            }
            else if (characterLocked[i]) // Check if character is locked
            {
                characterImages[i].color = Color.black; // Set the character image color to black for the silhouette
            }
            else if (i < buttonColors.Length) // Ensure we don't go out of bounds
            {
                characterImages[i].color = Color.white; // restore the image's original color
                characterButtons[i].image.color = buttonColors[i];
            }
            else
            {
                characterImages[i].color = Color.white; // restore the image's original color
                characterButtons[i].image.color = Color.white;
            }
        }
    }

    void UpdateCharacterTexts()
    {
        bool isOnline = PlayerPrefs.GetInt("Online", 1) == 1;

        for (int i = 0; i < characterButtons.Length; i++)
        {
            TMP_Text buttonText = characterButtons[i].GetComponentInChildren<TMP_Text>(true);
            if (buttonText)
            {
                if (i == selectedCharacterIndex)
                {
                    buttonText.text = "Equipped";
                }
                else if (!characterLocked[i] && isOnline)
                {
                    buttonText.text = "Equip";
                }
                else
                {
                    buttonText.text = "Locked";
                }
            }
            else
            {
                Debug.LogWarning($"Button at index {i} does not have a TMP_Text child component.");
            }
        }

        // Päivitä unlock-tekstit hahmojen kuvien alla
        if (isOnline)
        {
            characterUnlockTexts[0].text = "Unlocked";
            characterUnlockTexts[1].text = characterLocked[1] ? "Score 1000 points to unlock" : "Unlocked";
            characterUnlockTexts[2].text = characterLocked[2] ? "Score 2000 points to unlock" : "Unlocked";
        }
        else
        {
            characterUnlockTexts[0].text = "Unlocked";
            characterUnlockTexts[1].text = "Not available in offline mode";
            characterUnlockTexts[2].text = "Not available in offline mode";
        }
    }
}