using TMPro; // Namespace for TextMesh Pro, an improved text rendering package
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System; // Add this to use the Action type
using PlayFab.ClientModels;
using PlayFab;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel; // UI panel displayed when game is over
    [SerializeField] private TMP_Text countdownTextObject; // Text object to display the countdown before game starts
    [SerializeField] private TMP_Text scoreTextObject; // Text object to display the player's current score
    [SerializeField] private TMP_Text highScoreTextObject; // Text object to display the highest score
    [SerializeField] private GameObject pauseMenuCanvas; // UI panel displayed when game is paused
   


    private const string HighScoreKey = "HighScore"; // Key used to save/load high score with PlayerPrefs
    private float countdownTime = 3.0f; // Duration of countdown before game starts
    private int score = 0; // Player's current score
    private int highScore = 0; // Highest score achieved so far
    public static GameManager Instance; // Singleton instance of the GameManager

    private InfiniteParallaxBackground parallax; // Reference to the parallax scrolling background

    // Event triggered when the countdown is finished
    public event Action OnCountdownFinished;

    private void Awake()
    {
        SetupSingleton(); // Set up singleton instance
        LoadHighScore(); // Load the high score from PlayerPrefs
        parallax = FindObjectOfType<InfiniteParallaxBackground>(); // Get the parallax background script
       
    }

    private void Start()
    {
        InitializeGame(); // Initialize the game
    }

    private void SetupSingleton()
    {
        if (Instance == null)
        {
            Instance = this; // If no instance, set this instance as the singleton
        }
        else
        {
            Destroy(gameObject); // Destroy this instance if another one already exists
        }
    }
    
    // Tämä ennen scriptin muutosta

    //private void InitializeGame()
    //{
    //    Time.timeScale = 1; // Reset time scale to normal speed
    //    StartCoroutine(CountdownToStart()); // Start the countdown before the game starts
    //}

    private void InitializeGame()
    {
        Time.timeScale = 1; // Reset time scale to normal speed
        CountdownToStart(); // Start the countdown before the game starts
    }   
    public void ResumeGame()
    {
        Time.timeScale = 1; // Resume the game
        pauseMenuCanvas.SetActive(false); // Hide the pause menu
    }
    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        scoreTextObject.gameObject.SetActive(false);
        Time.timeScale = 0;
        UpdateHighScore();

        // Lähetä pisteet tulostaululle
        SendScoreToLeaderboard(score);
    }
    public void RestartGame()
    {
        Time.timeScale = 1; // Reset time scale to normal speed
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
    }

    void SendScoreToLeaderboard(int playerScore)
    {
        string playerName = PlayerPrefs.GetString("PlayerName");
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate> {
            new StatisticUpdate { StatisticName = "PlatformScore", Value = playerScore },
        }
        },
        result => { Debug.Log("Pelaajatilastot päivitetty"); },
        error => { Debug.LogError(error.GenerateErrorReport()); });
    }

    private void CountdownToStart()
    {
        DisableDragonflyAndParallax();
        StartCoroutine(DoCountdown());
    }

    private void DisableDragonflyAndParallax()
    {
        DragonflyController dragonfly = FindObjectOfType<DragonflyController>();
        if (dragonfly != null)
        {
            dragonfly.ToggleRigidbodyMovement(false);
            dragonfly.ResetDragonfly();
        }

        parallax.enableScrolling = false;
    }

    private void EnableDragonflyAndParallax()
    {
        DragonflyController dragonfly = FindObjectOfType<DragonflyController>();
        if (dragonfly != null)
        {
            dragonfly.ToggleRigidbodyMovement(true);
        }

        parallax.enableScrolling = true;
    }

    private IEnumerator DoCountdown()
    {
        float currentCountdown = countdownTime;
        while (currentCountdown > 0)
        {
            countdownTextObject.text = currentCountdown.ToString("0");
            yield return new WaitForSeconds(1.0f);
            currentCountdown--;
        }

        countdownTextObject.text = "";

        InitializeScore();

        EnableDragonflyAndParallax();

        NotifyCountdownFinished();

        StartCoroutine(UpdateScore());
    } 
    private void NotifyCountdownFinished()
    {
        OnCountdownFinished?.Invoke();
    }
    private void InitializeScore()
    {
        score = 0;
        scoreTextObject.text = $"Score: {score}";
        scoreTextObject.gameObject.SetActive(true);
    }
    private void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt(HighScoreKey, 0); // Load high score from PlayerPrefs or default to 0
        highScoreTextObject.text = $"High Score: {highScore}"; // Update high score text object
    }

    private void UpdateHighScore()
    {
        if (score > highScore)
        {
            highScore = score; // Update high score if player's score is higher
            PlayerPrefs.SetInt(HighScoreKey, highScore); // Save new high score to PlayerPrefs
        }
        highScoreTextObject.text = $"High Score: {highScore}"; // Update high score text object
    }
    

    private IEnumerator UpdateScore()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            score += 1; // Increase score every 0.5 seconds
            scoreTextObject.text = $"Score: {score}"; // Update score text object
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1; // Reset time scale to normal speed
        SceneManager.LoadScene("MainMenu"); // Load the main menu scene
    }
}

/* Alla Osa vanhasta Koodista minkä jaoin osiin jotta on helpommin luettava ja hallitava */


//private IEnumerator CountdownToStart()
//{
//    DragonflyController dragonfly = FindObjectOfType<DragonflyController>(); // Get the dragonfly controller script
//    if (dragonfly != null)
//    {
//        dragonfly.ToggleRigidbodyMovement(false); // Disable dragonfly movement during countdown
//    }

//    parallax.enableScrolling = false; // Disable parallax scrolling during countdown

//    float currentCountdown = countdownTime;
//    while (currentCountdown > 0)
//    {
//        countdownTextObject.text = currentCountdown.ToString("0"); // Update countdown text
//        yield return new WaitForSeconds(1.0f);
//        currentCountdown--;
//    }

//    countdownTextObject.text = ""; // Clear countdown text

//    score = 0;
//    scoreTextObject.text = $"Score: {score}"; // Reset and display the score
//    scoreTextObject.gameObject.SetActive(true);

//    if (dragonfly != null)
//    {
//        dragonfly.ResetDragonfly(); // Reset dragonfly position and state
//        dragonfly.ToggleRigidbodyMovement(true); // Enable dragonfly movement
//    }

//    parallax.enableScrolling = true; // Enable parallax scrolling

//    // Trigger event to notify other scripts that countdown has finished
//    OnCountdownFinished?.Invoke();

//    StartCoroutine(UpdateScore()); // Start updating the score
//}
