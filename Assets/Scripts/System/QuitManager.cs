using UnityEngine;
using UnityEngine.UI;
using TMPro;
// Lis‰‰ PlayFab-kirjastot
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;

public class QuitManager : MonoBehaviour
{
    public static QuitManager Instance { get; private set; }
    public GameObject quitPanelPrefab;
    private GameObject activeQuitPanel;
    private TMP_Text quitMessage;
    private bool isSecondConfirmation = false;

    // Muuttuja peliajan seurantaa varten.
    private float sessionTime = 0.0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleQuitPanel();
        }

        // Seuraa peliaikaa
        sessionTime += Time.deltaTime;
    }

    public void ToggleQuitPanel()
    {
        if (activeQuitPanel == null)
        {
            activeQuitPanel = Instantiate(quitPanelPrefab, transform.position, Quaternion.identity);
            quitMessage = activeQuitPanel.GetComponentInChildren<TMP_Text>();
            Button[] buttons = activeQuitPanel.GetComponentsInChildren<Button>();

            buttons[0].onClick.AddListener(HandleYesButton);
            buttons[1].onClick.AddListener(ToggleQuitPanel);
        }
        else
        {
            Destroy(activeQuitPanel);
            activeQuitPanel = null;
            isSecondConfirmation = false;
        }
    }

    private void HandleYesButton()
    {
        if (!isSecondConfirmation)
        {
            isSecondConfirmation = true;
            if (quitMessage)
                quitMessage.text = "Are you sure?";
        }
        else
        {
            SavePlaytimeToPlayFab();  // Tallenna peliaika ennen poistumista.
            QuitGame();
        }
    }

    public void SavePlaytimeToPlayFab()
    {
        // Tarkistetaan, onko pelaaja online-tilassa ennen PlayFab-kutsun tekemist‰
        if (SecurePlayerPrefs.GetInt("Online") == 1)
        {
            PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
            {
                float totalPlayTime = sessionTime;

                if (result.Data != null && result.Data.ContainsKey("TotalPlayTime"))
                {
                    float previousPlayTime;
                    if (float.TryParse(result.Data["TotalPlayTime"].Value, out previousPlayTime))
                    {
                        totalPlayTime += previousPlayTime;
                    }
                }

                PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
                {
                    Data = new Dictionary<string, string>
                {
                    { "TotalPlayTime", totalPlayTime.ToString() }
                }
                },
                success => Debug.Log("Playtime saved successfully."),
                error => Debug.LogError(error.GenerateErrorReport()));
            },
            error => Debug.LogError(error.GenerateErrorReport()));
        }
        else
        {
            // Jos pelaaja on offline-tilassa, voit jatkaa normaalisti ilman PlayFab-kutsua.
            Debug.Log("Player is offline, playtime will not be saved.");
        }
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
