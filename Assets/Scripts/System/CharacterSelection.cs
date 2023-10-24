using UnityEngine;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;
using System.Collections;


public class CharacterSelection : MonoBehaviour
{
    // Singleton Instance
    public static CharacterSelection Instance { get; private set; }

    // Character Details
    [Header("Character Details")]
    private Sprite[] characterSprites; // Array of character sprites
    public int selectedCharacterIndex = -1; // Index of currently selected character
    public bool[] characterLocked = { false, true, true }; // Array indicating if a character is locked
    public PlayerCharacterSO[] characters; // Array of PlayerCharacterSO

    // UI Elements
    [Header("UI Elements")]
    public Button[] characterButtons; // Array of character selection buttons
    public Image[] characterImages; // Images representing the characters
    public TMP_Text[] characterUnlockTexts; // Array of text fields for character unlock status
    public TMP_Text infoText; // Text field for displaying information to the player

    // Visual Appearance
    [Header("Visual Appearance")]
    public Color equippedColor = Color.gray; // Color to indicate a character is equipped
    public Color[] buttonColors; // Array to store default colors for each button

    // Loading Panel
    [Header("Loading Panel")]
    public LoadingPanelManager loadingPanelManager; // Reference to LoadingPanelManager script

    private void Awake()
    {
        selectedCharacterIndex = -1;
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

        // Tarkista onko pelaaja lukittu ja tallenna tulos muuttujaan
        bool isPlayerLocked = SecurePlayerPrefs.GetInt("Online", 1) == 0;

        // Kutsu metodia, joka käsittelee pelaajan lukituksen
        HandlePlayerLockStatus(isPlayerLocked);
    }

    // Metodi käsittelee pelaajan lukituksen
    private void HandlePlayerLockStatus(bool isLocked)
    {
        if (isLocked)
        {
            // Aseta lukitus ja päivitä teksti ja värit
            characterLocked[0] = false;
            characterLocked[1] = true;
            characterLocked[2] = true;
            UpdateCharacterTexts();
            UpdateButtonColors();

            loadingPanelManager.ShowLoadingPanel();
            infoText.text = "Additional characters are unavailable in offline mode. You can only select the default character.";
        }
        else
        {
            loadingPanelManager.ShowLoadingPanel();
            FetchPlayerHighScore();

            // Kutsu uutta metodia, joka käsittelee valitun hahmon
            HandleSelectedCharacter();
        }
    }
    public void SetSelectedCharacterIndex(int index)
    {
        selectedCharacterIndex = index;
    }

    // Switch to the Main Menu scene
    public void GoToMainMenu()
    {
        if (selectedCharacterIndex < 0 || selectedCharacterIndex > 2)
        {
            Debug.LogWarning("Invalid character index. Cannot go to the main menu.");

            // Näytä tilapäinen viesti pelaajalle.
            StartCoroutine(DisplayTemporaryMessage("You must choose a character to continue.", 2f));
            return;
        }

        SceneManager.LoadScene("MainMenu"); // Load the main menu scene
    }

    // Uusi metodi valitun hahmon käsittelylle
    private void HandleSelectedCharacter()
    {
        if (selectedCharacterIndex < 0 || selectedCharacterIndex >= characters.Length)
        {
            infoText.text = "Select your character.";
            return; // Keskeytetään suoritus
        }
        
        string additionalMessage = AdditionalCharactersAvailableMessage();
        // Tässä kohtaa chatgpt muuttanut switchcase methodin sisältä ja tehnyt siitä infotext. 
        //infotext alla kommentoituna. testataan myöhemmin -->
        //infoText.text = $"Character {selectedCharacterIndex + 1} equipped. Press Continue to play. {additionalMessage}";
        switch (selectedCharacterIndex)
        {
            case 0:
                infoText.text = "Default character equipped. Press Continue to play. " + additionalMessage;
                break;
            case 1:
                infoText.text = "Character 2 equipped. Press Continue to play. " + additionalMessage;
                break;
            case 2:
                infoText.text = "Character 3 equipped. Press Continue to play. " + additionalMessage;
                break;
        }
    }


    // Equip a character based on the index provided
    public void EquipCharacter(int index)
    {
        bool isOnline = SecurePlayerPrefs.GetInt("Online", 1) == 1;

        if (index < characters.Length && !characterLocked[index])
        {
            selectedCharacterIndex = index;
            Debug.Log($"Character with index {index} equipped.");
            UpdateButtonColors();
            //updatecharactertext poistettu tai nimi muutettu updatecharacterdisplays
            UpdateCharacterTexts();
            // tässä sama kuin yllä. chatgpt yksinkertaistaa switchcase methodin. testataan myöhemmin.
            //string additionalMessage = AdditionalCharactersAvailableMessage();
            //infoText.text = $"Character {index + 1} equipped. Press Continue to play. {additionalMessage}";

            string additionalMessage = AdditionalCharactersAvailableMessage();
            switch (index)
            {
                case 0:
                    infoText.text = "Default character equipped. Press Continue to play. " + additionalMessage;
                    break;
                case 1:
                    infoText.text = "Character 2 equipped. Press Continue to play. " + additionalMessage;
                    break;
                case 2:
                    infoText.text = "Character 3 equipped. Press Continue to play. " + additionalMessage;
                    break;

            }
        }
        else if (characterLocked[index] && isOnline)
        {
            Debug.Log($"Character with index {index} is locked.");
            StartCoroutine(DisplayTemporaryMessage("This character is still waiting in the shadows, achieve a higher score to unlock.", 2f));
        }
        else if (characterLocked[index] && !isOnline)
        {
            Debug.Log($"Character with index {index} is locked.");
            StartCoroutine(DisplayTemporaryMessage("This character is unavailable in offline mode.", 2f));
        }
        else
        {
            Debug.Log($"Invalid index {index}. Character not equipped.");
        }
    }

    // Message to display additional available characters
    private string AdditionalCharactersAvailableMessage()
    {
        // tätä myös yksinkertaistettu chatgpt toimesta. for loop muutettu foreach loopiksi
        int unlockedCharacters = 0;
        for (int i = 0; i < characterLocked.Length; i++)
        {
            if (!characterLocked[i] && i != selectedCharacterIndex)
                unlockedCharacters++;
        }

        if (unlockedCharacters > 0)
        {
            return "Or select from the available characters.";
        }
        return "";
    }

    // Display a message for a temporary duration
    private IEnumerator DisplayTemporaryMessage(string message, float duration)
    {
        string previousMessage = infoText.text; // Store the current message
        infoText.text = message; // Update the text to the temporary message
        yield return new WaitForSeconds(duration); // Wait for the duration specified
        infoText.text = previousMessage; // Reset back to the previous message
    }


    // Fetch player's high score from the PlayFab service
    //tämä method myös muutettu täysin chatgpt toimesta
    void FetchPlayerHighScore()
    {
        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(),
        result => {
            bool scoreFound = false;

            foreach (var eachStat in result.Statistics)
            {
                if (eachStat.StatisticName == "PlatformScore")
                {
                    SecurePlayerPrefs.SetInt("PlayerScore", eachStat.Value);
                    scoreFound = true;
                    break;
                }
            }

            if (!scoreFound)
            {
                SecurePlayerPrefs.SetInt("PlayerScore", 0);
            }

            UpdateCharacterLocks();
            UpdateCharacterTexts();

            loadingPanelManager.HideLoadingPanel();
        },
        error => {
            Debug.LogError(error.GenerateErrorReport());
            loadingPanelManager.HideLoadingPanel();
        });
    }

    // Lock or unlock characters based on the player's score
    //tämä method myös muutettu täysin chatgpt toimesta
    void UpdateCharacterLocks()
    {
        // Update which characters are locked based on the player's score
        int playerScore = SecurePlayerPrefs.GetInt("PlayerScore", 0);

        characterLocked[0] = false;

        if (playerScore >= 1000)
        {
            characterLocked[1] = false;
        }
        if (playerScore >= 2000)
        {
            characterLocked[2] = false;
        }

        UpdateCharacterDisplays();
    }
    private void UpdateCharactersFromSO()
    {
        if (characters.Length != characterSprites.Length)
        {
            Debug.LogWarning("Characters ScriptableObject array and characterSprites array lengths are not matching.");
            return;
        }

        for (int i = 0; i < characters.Length; i++)
        {
            characterSprites[i] = characters[i].characterPlayableSprite;
        }
    }

    // Päivitetty metodi:
    public void SetSelectedCharacterIndexFromSO(PlayerCharacterSO selectedCharacter)
    {
        int index = System.Array.IndexOf(characters, selectedCharacter);
        if (index >= 0)
        {
            SetSelectedCharacterIndex(index);
        }
        else
        {
            Debug.LogWarning("Selected character is not found in the characters array.");
        }
    }





    // tämä uusi chatgpt generoima methodi mikä helpottaa displayn päivittämistä
    void UpdateCharacterDisplays()
    {
        UpdateButtonColors();
        UpdateCharacterTexts();
    }

    // Update button colors based on character availability and selection
    // tässä myös muutoksia. katso myöhemmin 
    void UpdateButtonColors()
    {
        for (int i = 0; i < characterButtons.Length; i++)
        {
            if (i == selectedCharacterIndex)
            {
                characterButtons[i].image.color = equippedColor;
                characterImages[i].color = Color.white;
            }
            else if (characterLocked[i]) // Check if character is locked
            {
                characterImages[i].color = Color.black; // Set the character image color to black for the silhouette
            }
            else if (i < buttonColors.Length)
            {
                characterImages[i].color = Color.white;
                characterButtons[i].image.color = buttonColors[i];
            }
            else
            {
                characterImages[i].color = Color.white;
                characterButtons[i].image.color = Color.white;
            }
        }
    }
    public (int UnlockedCount, int TotalCount) GetCharacterInfo()
    {
        int unlockedCount = 0;
        int totalCount = characterLocked.Length;  // This will give the total number of characters

        foreach (bool isLocked in characterLocked)
        {
            if (!isLocked) unlockedCount++;
        }

        return (unlockedCount, totalCount);
    }

    // Update text displays on character buttons
    // tässä myös isot muutokset
    void UpdateCharacterTexts()
    {
        bool isOnline = SecurePlayerPrefs.GetInt("Online", 1) == 1;

        for (int i = 0; i < characterButtons.Length; i++)
        {
            TMP_Text buttonText = characterButtons[i].GetComponentInChildren<TMP_Text>(true);
            if (buttonText)
            {
                if (i == selectedCharacterIndex)
                {
                    buttonText.text = "Equipped";
                }
                else if (!characterLocked[i])
                {
                    // Jos olemme offline ja hahmo on indexissä 0, näytetään "Equip"
                    buttonText.text = (!isOnline && i == 0) ? "Equip" : (isOnline ? "Equip" : "Locked");
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