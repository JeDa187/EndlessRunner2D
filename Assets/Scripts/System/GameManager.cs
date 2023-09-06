using TMPro; // Namespace for TextMesh Pro, an enhanced text rendering package
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System; // Namespace for handling system-based operations such as events
using PlayFab.ClientModels; // For integrating with PlayFab's client models
using PlayFab; // Namespace for PlayFab SDK
using System.Collections.Generic; // For using data structures like List

public class GameManager : MonoBehaviour
{
    private ScoreManager scoreManager; // Manager that handles score functionalities
    private InfiniteParallaxBackground parallax; // Reference to the parallax scrolling background

    [SerializeField] private GameObject gameOverPanel; // UI panel displayed when game is over
    [SerializeField] private TMP_Text countdownTextObject; // Text object to display the countdown before the game starts

    private float countdownTime = 3.0f; // Duration of the countdown before the game starts
    public static GameManager Instance; // Singleton instance of the GameManager

    // Event triggered when the countdown is finished
    public event Action OnCountdownFinished;

    private void Awake()
    {
        parallax = FindObjectOfType<InfiniteParallaxBackground>(); // Find and assign the parallax background script
        scoreManager = GetComponent<ScoreManager>(); // Get the ScoreManager attached to this GameObject
        SetupSingleton(); // Configure the singleton instance
        scoreManager.LoadHighScore(); // Retrieve the high score from storage (e.g., PlayerPrefs)
    }

    private void Start()
    {
        InitializeGame(); // Setup and prepare the game for play
    }

    private void SetupSingleton()
    {
        if (Instance == null)
        {
            Instance = this; // If there's no instance yet, set this as the singleton instance
        }
        else
        {
            Destroy(gameObject); // Remove this GameObject if another instance already exists
        }
    }

    private void InitializeGame()
    {
        Time.timeScale = 1; // Reset the game's time scale to normal speed
        CountdownToStart(); // Begin the countdown sequence before game commencement
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true); // Show the game over UI
        scoreManager.GetScoreTextObject().gameObject.SetActive(false); // Hide the score display
        Time.timeScale = 0; // Pause the game by setting the time scale to zero
        scoreManager.UpdateHighScore(); // Check and update the high score if necessary

        if (PlayerPrefs.GetInt("Online") == 1)
        {
            SendScoreToLeaderboard(scoreManager.GetScore()); // Send score to online leaderboard if online mode is enabled
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1; // Reset game's time scale to normal speed
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene to restart the game
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1; // Reset game's time scale to normal speed
        SceneManager.LoadScene("MainMenu"); // Switch to the main menu scene
    }

    void SendScoreToLeaderboard(int playerScore)
    {
        string playerName = PlayerPrefs.GetString("PlayerName");
        // Update the player's score on PlayFab's leaderboard
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate> {
            new StatisticUpdate { StatisticName = "PlatformScore", Value = playerScore },
        }
        },
        result => { Debug.Log("Player stats updated"); }, // Log success
        error => { Debug.LogError(error.GenerateErrorReport()); }); // Log any errors
    }

    private void CountdownToStart()
    {
        DisableDragonflyAndParallax(); // Ensure the dragonfly and parallax are not active during countdown
        StartCoroutine(DoCountdown()); // Begin the countdown coroutine
    }

    private IEnumerator DoCountdown()
    {
        yield return StartCoroutine(StartCountdown(countdownTime));
        scoreManager.InitializeScore(); // Set up the score for the game session

        EnableDragonflyAndParallax(); // Activate the dragonfly and parallax after the countdown
        NotifyCountdownFinished(); // Notify subscribers that the countdown has concluded
        StartCoroutine(scoreManager.UpdateScore()); // Start the score updating process
    }

    private void NotifyCountdownFinished()
    {
        OnCountdownFinished?.Invoke(); // Trigger the OnCountdownFinished event if there are any subscribers
    }

    private IEnumerator StartCountdown(float duration)
    {
        float currentCountdown = duration;
        // For each second of the countdown duration
        while (currentCountdown > 0)
        {
            countdownTextObject.text = currentCountdown.ToString("0");
            yield return new WaitForSeconds(1.0f); // Wait for a second
            currentCountdown--;
        }

        countdownTextObject.text = ""; // Clear the countdown text
    }

    private void DisableDragonflyAndParallax()
    {
        DragonflyController dragonfly = FindObjectOfType<DragonflyController>(); // Get the dragonfly controller
        if (dragonfly != null)
        {
            dragonfly.ToggleRigidbodyMovement(false); // Deactivate dragonfly's movement
            dragonfly.ResetDragonfly(); // Reset the dragonfly's state
        }

        parallax.enableScrolling = false; // Turn off parallax scrolling
    }

    private void EnableDragonflyAndParallax()
    {
        DragonflyController dragonfly = FindObjectOfType<DragonflyController>();
        if (dragonfly != null)
        {
            dragonfly.ToggleRigidbodyMovement(true); // Activate dragonfly's movement
        }

        parallax.enableScrolling = true; // Turn on parallax scrolling
    }
}
