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

    [Header("Loading Panel Reference")]
    [SerializeField] private LoadingPanelManager loadingPanelManager;  // Viittaus LoadingPanelManager-olioon

    void Start()
    {
        loadingPanelManager.ShowLoadingPanel(); // Näytä loading panel

        if (SecurePlayerPrefs.GetInt("Online") == 1)
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

            loadingPanelManager.HideLoadingPanel(); // Piilota loading panel
        }
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

            loadingPanelManager.HideLoadingPanel(); // Piilota loading panel kun tiedot on haettu
        },
        error => {
            Debug.LogError(error.GenerateErrorReport());
            loadingPanelManager.HideLoadingPanel(); // Piilota loading panel, jos virhe tapahtuu
        });
    }

    public void OnBackButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
