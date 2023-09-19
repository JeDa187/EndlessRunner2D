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
    private ScoreManager scoreManager;
    private InfiniteParallaxBackground parallax; // Reference to the parallax scrolling background  
    [SerializeField] GameObject gameOverPanel; // UI panel displayed when game is over
    [SerializeField] GameObject pauseButton;
    [SerializeField] TMP_Text countdownTextObject; // Text object to display the countdown before game starts
    private float countdownTime = 3.0f; // Duration of countdown before game starts
    public static GameManager Instance; // Singleton instance of the GameManager
    public event Action OnCountdownFinished; // Event triggered when the countdown is finished

    public SpriteRenderer playerSpriteRenderer; // Renderer, joka n‰ytt‰‰ pelaajan hahmon.


    private void Awake()
    {
        parallax = FindObjectOfType<InfiniteParallaxBackground>(); // Get the parallax background script
        scoreManager = GetComponent<ScoreManager>();
        SetupSingleton(); // Set up singleton instance
        scoreManager.LoadHighScore(); // Load the high score from PlayerPrefs

        if (scoreManager != null)
        {
            scoreManager.LoadHighScore(); // Load the high score from PlayerPrefs
        }
        

    }
    private void Start()
    {
        InitializeGame(); // Initialize the game

        // Aseta valittu hahmo peliss‰.
        if (CharacterSelection.Instance != null)
        {
            playerSpriteRenderer.sprite = CharacterSelection.Instance.characterSprites[CharacterSelection.Instance.selectedCharacterIndex];
            Debug.Log($"Set player sprite to: {playerSpriteRenderer.sprite.name}");
        }
        else
        {
            Debug.LogError("CharacterSelection.Instance is null.");
        }
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
    private void InitializeGame()
    {
        Time.timeScale = 1; // Reset time scale to normal speed
        CountdownToStart(); // Start the countdown before the game starts
    }
    public void GameOver()
    {
        pauseButton.SetActive(false);
        gameOverPanel.SetActive(true);
        scoreManager.GetScoreTextObject().gameObject.SetActive(false);
        Time.timeScale = 0;
        scoreManager.UpdateHighScore();

        if (SecurePlayerPrefs.GetInt("Online") == 1)
        {
            SendScoreToLeaderboard(scoreManager.GetScore()); 
        }      
    }
    public void RestartGame()
    {
        Time.timeScale = 1; // Reset time scale to normal speed
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1; // Reset time scale to normal speed
        SceneManager.LoadScene("MainMenu"); // Load the main menu scene
    }
    void SendScoreToLeaderboard(int playerScore)
    {
        string playerName = SecurePlayerPrefs.GetString("PlayerName");
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate> {
            new StatisticUpdate { StatisticName = "PlatformScore", Value = playerScore },
        }
        },
        result => { Debug.Log("Pelaajatilastot p‰ivitetty"); },
        error => { Debug.LogError(error.GenerateErrorReport()); });
    }
    private void CountdownToStart()
    {
        DisableDragonflyAndParallax();
        StartCoroutine(DoCountdown());
    }
    
    private IEnumerator DoCountdown()
    {
        yield return StartCoroutine(StartCountdown(countdownTime));
        scoreManager.InitializeScore();

        EnableDragonflyAndParallax();
        NotifyCountdownFinished();
        StartCoroutine(scoreManager.UpdateScore());
    }
    private void NotifyCountdownFinished()
    {
        OnCountdownFinished?.Invoke();
    }
    
    private IEnumerator StartCountdown(float duration)
    {
        float currentCountdown = duration;
        while (currentCountdown > 0)
        {
            countdownTextObject.text = currentCountdown.ToString("0");
            yield return new WaitForSeconds(1.0f);
            currentCountdown--;
        }

        countdownTextObject.text = "";
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
}
