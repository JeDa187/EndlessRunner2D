using UnityEngine;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;

public class CharacterSelection : MonoBehaviour
{
    public static CharacterSelection Instance { get; private set; }

    public Sprite[] characterSprites;
    public int selectedCharacterIndex = 0;
    public bool[] characterLocked = { false, true, true };

    public Button[] characterButtons;
    public Color equippedColor = Color.gray;

    public GameObject loadingPanel; // Latauspaneeli

    private void Awake()
    {
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
        if (PlayerPrefs.GetInt("Online", 1) == 0) // Jos offline-tila
        {
            // Lukitse kaikki hahmot paitsi ensimmäinen
            characterLocked[0] = false;
            characterLocked[1] = true;
            characterLocked[2] = true;
            UpdateButtonTexts();
            UpdateButtonColors();
            HideLoadingPanel(); // Piilota latauspaneeli offline-tilassa
        }
        else
        {
            ShowLoadingPanel();
            FetchPlayerHighScore();
        }
    }

    public void GoToMainMenu()
    {
        Debug.Log("GoToMainMenu called.");
        SceneManager.LoadScene("MainMenu");
    }

    public void EquipCharacter(int index)
    {
        if (index < characterSprites.Length && !characterLocked[index])
        {
            selectedCharacterIndex = index;
            Debug.Log($"Character with index {index} equipped.");
            UpdateButtonColors();
            UpdateButtonTexts();
        }
        else if (characterLocked[index])
        {
            Debug.Log($"Character with index {index} is locked.");
        }
        else
        {
            Debug.Log($"Invalid index {index}. Character not equipped.");
        }
    }

    void FetchPlayerHighScore()
    {
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
            UpdateButtonTexts();
            HideLoadingPanel(); // Piilota latauspaneeli kun tiedot on haettu
        },
        error => {
            Debug.LogError(error.GenerateErrorReport());
            HideLoadingPanel(); // Piilota latauspaneeli virhetilanteessa
        });
    }

    void UpdateCharacterLocks()
    {
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

        UpdateButtonTexts();
        UpdateButtonColors();
    }

    void UpdateButtonColors()
    {
        for (int i = 0; i < characterButtons.Length; i++)
        {
            if (i == selectedCharacterIndex)
            {
                characterButtons[i].image.color = equippedColor;
            }
            else
            {
                characterButtons[i].image.color = Color.white;
            }
        }
    }

    void UpdateButtonTexts()
    {
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
    }

    void ShowLoadingPanel()
    {
        loadingPanel.SetActive(true);
    }

    void HideLoadingPanel()
    {
        loadingPanel.SetActive(false);
    }
}
