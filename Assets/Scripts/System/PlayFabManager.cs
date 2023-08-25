using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayFabManager : MonoBehaviour
{
    public TMP_Text leaderboardText;

    private void Start()
    {
        Login();
    }

    // L�het� pistem��r� PlayFabille.
    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate { StatisticName = "PlatformScore", Value = score }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    public void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier, // K�yt� laitteen yksil�llist� tunnusta kirjautumiseen.
            CreateAccount = true // Luo tili, jos sellaista ei ole.
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Successfully logged in to PlayFab");
    }

    // Kutsutaan, kun pistem��r�n p�ivitys onnistuu.
    private void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successful leaderboard send");
    }

    // Hae leaderboard-tulokset PlayFabista.
    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "PlatformScore",
            StartPosition = 0,
            MaxResultsCount = 10
        };

        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    // Kutsutaan, kun leaderboard-tulokset on haettu onnistuneesti.
    private void OnLeaderboardGet(GetLeaderboardResult result)
    {
        leaderboardText.text = ""; // Tyhjenn� teksti.

        foreach (var item in result.Leaderboard)
        {
            leaderboardText.text += item.Position + " " + item.DisplayName + " " + item.StatValue + "\n";
        }
    }

    // Kutsutaan, jos PlayFab-pyynt� ep�onnistuu.
    private void OnError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }
}