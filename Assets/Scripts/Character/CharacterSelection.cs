using UnityEngine;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;

public class CharacterSelection : MonoBehaviour
{
    public static CharacterSelection Instance { get; private set; }

    public Sprite[] characterSprites; // Hahmojen spritet.
    public int selectedCharacterIndex = 0; // Oletushahmo.
    public bool[] characterLocked = { false, true, true }; // Oletuksena ensimmäinen hahmo on avoinna ja muut kaksi ovat lukittuja

    public Button[] characterButtons; // Hahmojen napit.
    public Color equippedColor = Color.gray; // Väri, joka näytetään, kun hahmo on varustettu.

    private void Awake()
    {
        // Tarkista, onko olemassa oleva instanssi samassa kohtauksessa
        if (Instance == null || Instance.gameObject.scene != this.gameObject.scene)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Jos on olemassa toinen instanssi samassa kohtauksessa, tuhoa tämä instanssi
            if (this != Instance)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Start()
    {
        FetchPlayerHighScore();

    }

    public void GoToMainMenu()
    {
        Debug.Log("GoToMainMenu called.");
        SceneManager.LoadScene("MainMenu"); // Load the main menu scene
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
                    PlayerPrefs.SetInt("PlayerScore", eachStat.Value); // Tallenna pelaajan pisteet PlayerPrefsiin
                    scoreFound = true;
                    break; // Lopeta loop, kun pistetulos on löydetty
                }
            }

            // Jos pistetulosta ei löydy, aseta pisteet nollaksi
            if (!scoreFound)
            {
                PlayerPrefs.SetInt("PlayerScore", 0);
            }

            UpdateCharacterLocks(); // Päivitä hahmojen lukitusstatukset
            UpdateButtonTexts(); // Päivitä nappien tekstit

        },
        error => { Debug.LogError(error.GenerateErrorReport()); });
    }


    void UpdateCharacterLocks()
    {
        int playerScore = PlayerPrefs.GetInt("PlayerScore", 0); // Hae pelaajan pisteet PlayerPrefsistä

        characterLocked[0] = false; // Aseta ensimmäinen hahmo aina avoimeksi

        if (playerScore >= 1000)
        {
            characterLocked[1] = false; // Avaa toinen hahmo
        }
        if (playerScore >= 2000)
        {
            characterLocked[2] = false; // Avaa kolmas hahmo
        }

        UpdateButtonTexts(); // Päivitä nappien tekstit tässä
        UpdateButtonColors(); // Päivitä nappien värit
    }



    void UpdateButtonColors()
    {
        for (int i = 0; i < characterButtons.Length; i++)
        {
            if (i == selectedCharacterIndex)
            {
                characterButtons[i].image.color = equippedColor; // Aseta varustettu väri
            }
            else
            {
                characterButtons[i].image.color = Color.white; // Aseta oletusväri
            }
        }
    }

    void UpdateButtonTexts()
    {
        for (int i = 0; i < characterButtons.Length; i++)
        {
            TMP_Text buttonText = characterButtons[i].GetComponentInChildren<TMP_Text>(true); // Etsi TMP_Text-komponentti myös ei-aktiivisista lapsista
            if (buttonText)
            {
                if (i == selectedCharacterIndex)
                {
                    buttonText.text = "Equipped";
                    Debug.Log($"Button {i} set to 'Equipped'");
                }
                else if (!characterLocked[i])
                {
                    buttonText.text = "Equip";
                    Debug.Log($"Button {i} set to 'Equip'");
                }
                else
                {
                    buttonText.text = "Locked";
                    Debug.Log($"Button {i} set to 'Locked'");
                }
            }
            else
            {
                Debug.LogWarning($"Button at index {i} does not have a TMP_Text child component.");
            }
        }
    }

}
