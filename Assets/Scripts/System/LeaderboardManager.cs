using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    public TMP_Text[] playerNameTexts;
    public TMP_Text[] playerScoreTexts;
    public Button backButton;
   

    void Start()
    {
        GetLeaderboard();
    }

    void GetLeaderboard()
    {
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
        {
            StatisticName = "PlatformScore",
            StartPosition = 0,
            MaxResultsCount = 10
        },
        result => {
            for (int i = 0; i < result.Leaderboard.Count; i++)
            {
                playerNameTexts[i].text = result.Leaderboard[i].DisplayName;
                playerScoreTexts[i].text = result.Leaderboard[i].StatValue.ToString();
            }
        },
        error => { Debug.LogError(error.GenerateErrorReport()); });
    }

    public void OnBackButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
