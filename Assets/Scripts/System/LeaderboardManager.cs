using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    public TMP_Text[] playerNameTexts; // Array of text fields for player names
    public TMP_Text[] playerScoreTexts; // Array of text fields for player scores
    public Button backButton; // Back button

    // Method called when the script is initialized
    void Start()
    {
        if (PlayerPrefs.GetInt("Online") == 1)
        {
            GetLeaderboard();
        }
        else
        {
            // show a message to the player that the leaderboard is not available in offline mode
            for (int i = 0; i < playerNameTexts.Length; i++)
            {
                playerNameTexts[i].text = "";
                playerScoreTexts[i].text = "";
            }
            playerNameTexts[0].text = "Leaderboard is not available in offline mode.";
        }
    }


    // Method to get the leaderboard data
    void GetLeaderboard()
    {
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
        {
            StatisticName = "PlatformScore",
            StartPosition = 0,
            MaxResultsCount = 10
        },
        result => {
            // Update the leaderboard text fields with the received results
            for (int i = 0; i < result.Leaderboard.Count; i++)
            {
                playerNameTexts[i].text = result.Leaderboard[i].DisplayName;
                playerScoreTexts[i].text = result.Leaderboard[i].StatValue.ToString();
            }
        },
        error => { Debug.LogError(error.GenerateErrorReport()); });
    }

    // Method called when the "Back" button is clicked
    public void OnBackButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
