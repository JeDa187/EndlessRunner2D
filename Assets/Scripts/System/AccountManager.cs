using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class AccountManager : MonoBehaviour
{
    [Header("Pelaajan tiedot")]
    public TMP_Text playerNameText;
    public TMP_Text highscoreText;
    public TMP_Text accountCreationDateText;
    public TMP_Text totalPlayTimeText;

    [Header("Varoituspaneeli")]
    public GameObject warningPanel;
    public TMP_Text warningText;
    public Button confirmButton;
    public Button cancelButton;

    [Header("Tilin poisto-ohjeet")]
    public TMP_Text accountDeletionInstructionsText;
    public GameObject accountDeletionInstructionsPanel;
    public Button openAccountDeletionInstructionsButton;
    public Button closeAccountDeletionInstructionsButton;

    [Header("Muut")]
    private QuitManager quitManager;
    public Button MainMenu;
    private string playerName;

    private void Start()
    {
        quitManager = QuitManager.Instance;

        if (quitManager == null)
        {
            Debug.LogError("QuitManager instance not found.");
            return;
        }

        quitManager.SavePlaytimeToPlayFab();

        playerName = SecurePlayerPrefs.GetString("PlayerName");
        FetchPlayerInfo();
    }

    void FetchPlayerInfo()
    {
        playerNameText.text = "" + playerName;

        // Fetch player statistics
        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(),
            result =>
            {
                bool hasHighscore = false; // Assume player hasn't achieved a highscore by default

                foreach (var eachStat in result.Statistics)
                {
                    if (eachStat.StatisticName == "PlatformScore" && eachStat.Value > 0)
                    {
                        highscoreText.text = "Highscore: " + eachStat.Value;
                        hasHighscore = true; // Player has a highscore
                        break; // Exit the loop as we found the highscore
                    }
                }

                // If the player hasn't achieved a highscore yet
                if (!hasHighscore)
                {
                    highscoreText.text = "No highscore yet! Play now and set your record!";
                }
            },
            error =>
            {
                Debug.LogError(error.GenerateErrorReport());
                ShowWarning("Failed to fetch highscore data.");
            }
        );

        // Get the stored PlayFabId from PlayerPrefs 
        string playFabId = SecurePlayerPrefs.GetString("PlayFabId");

        // Fetch player user data
        PlayFabClientAPI.GetUserData(new GetUserDataRequest { PlayFabId = playFabId },
            result =>
            {
                if (result.Data.ContainsKey("AccountCreationDate"))
                {
                    accountCreationDateText.text = "Account Created On: " + result.Data["AccountCreationDate"].Value;
                }

                if (result.Data != null && result.Data.ContainsKey("TotalPlayTime"))
                {
                    float retrievedPlayTime;
                    if (float.TryParse(result.Data["TotalPlayTime"].Value, out retrievedPlayTime) && retrievedPlayTime > 0)
                    {
                        int hours = Mathf.FloorToInt(retrievedPlayTime / 3600);
                        int minutes = Mathf.FloorToInt((retrievedPlayTime % 3600) / 60);
                        int seconds = Mathf.FloorToInt(retrievedPlayTime % 60);

                        totalPlayTimeText.text = "Total Play Time: " + hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
                    }
                    else
                    {
                        totalPlayTimeText.text = "You haven't played yet! Dive in and enjoy the game!";
                    }
                }
                else
                {
                    totalPlayTimeText.text = "You haven't played yet! Dive in and enjoy the game!";
                }
            },
            error =>
            {
                Debug.LogError(error.GenerateErrorReport());
                ShowWarning("Failed to fetch user data.");
            }
        );

    }

    public void ShowAccountDeletionInstructions()
    {
        string instructions = "To delete your account:\n\n" +
                              "1. Send an email to [support@email.com] with the subject 'Account Deletion Request'.\n" +
                              "2. In the email body, provide your username and the reason for the deletion request.\n" +
                              "3. Our support team will get back to you within 48 hours.\n\n" +
                              "Please note: Deleting your account is irreversible, and all data associated with your account will be permanently removed.";

        accountDeletionInstructionsText.text = instructions;
        accountDeletionInstructionsPanel.SetActive(true);
    }

    // Metodi paneelin näyttämiseksi:
   

    public void ShowWarning(string message)
    {
        warningText.text = message;
        warningPanel.SetActive(true);
    }

    public void HideAccountDeletionInstructionsPanel()
    {
        accountDeletionInstructionsPanel.SetActive(false);
    }

    public void HideWarning()
    {
        warningPanel.SetActive(false);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Load the main menu scene
    }
}

