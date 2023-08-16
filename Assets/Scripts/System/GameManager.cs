using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;           // Panel displayed when the game is over
    [SerializeField] private TMP_Text countdownTextObject;       // Text object for the countdown
    [SerializeField] private TMP_Text scoreTextObject;           // Text object for the score
    [SerializeField] private TMP_Text highScoreTextObject;       // Text object for the high score

    private const string HighScoreKey = "HighScore";             // Key for PlayerPrefs

    private float countdownTime = 3.0f;                          // Time for the countdown before the game starts
    private int score = 0;                                       // Player's score
    private int highScore = 0;                                   // High score
    public static GameManager Instance;                          // Singleton instance of the GameManager

    private InfiniteParallaxBackground parallax;   // Add a reference to the new InfiniteParallaxBackground class

    private void Awake()
    {
        SetupSingleton();
        LoadHighScore();
        parallax = FindObjectOfType<InfiniteParallaxBackground>();  // Find the InfiniteParallaxBackground object
    }

    private void Start()
    {
        InitializeGame();                                        // Setup the game at the start
    }

    private void SetupSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);                                 // Destroy the game object if another instance exists
        }
    }

    private void InitializeGame()
    {
        Time.timeScale = 1;                                      // Ensure game time is running normally
        StartCoroutine(CountdownToStart());                      // Start the countdown before the game begins
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);                           // Display the Game Over panel
        scoreTextObject.gameObject.SetActive(false);             // Hide the score text
        Time.timeScale = 0;                                      // Pause the game time

        UpdateHighScore();                                       // Update the high score if necessary
    }

    public void RestartGame()
    {
        Time.timeScale = 1;                                      // Resume game time
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  // Reload the current scene
    }

    private void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt(HighScoreKey, 0);         // Load the high score from PlayerPrefs
        highScoreTextObject.text = $"High Score: {highScore}";   // Display the high score
    }

    private void UpdateHighScore()
    {
        if (score > highScore)                                   // Check if the score is greater than the high score
        {
            highScore = score;                                   // Update the high score
            PlayerPrefs.SetInt(HighScoreKey, highScore);         // Save the high score to PlayerPrefs
        }
        highScoreTextObject.text = $"High Score: {highScore}";   // Display the high score
    }

    private IEnumerator CountdownToStart()
    {
        DragonflyController dragonfly = FindObjectOfType<DragonflyController>();
        if (dragonfly != null)
        {
            dragonfly.ToggleRigidbodyMovement(false);
        }

        // Update CountdownToStart() method to use the new InfiniteParallaxBackground class
        parallax.enableScrolling = false; // Disable parallax scrolling

        float currentCountdown = countdownTime;
        while (currentCountdown > 0)
        {
            countdownTextObject.text = currentCountdown.ToString("0");
            yield return new WaitForSeconds(1.0f);
            currentCountdown--;
        }

        countdownTextObject.text = "";

        score = 0;
        scoreTextObject.text = $"Score: {score}";
        scoreTextObject.gameObject.SetActive(true);

        if (dragonfly != null)
        {
            dragonfly.ResetDragonfly();
            dragonfly.ToggleRigidbodyMovement(true);
        }

        // Use the new InfiniteParallaxBackground class to enable parallax scrolling
        parallax.enableScrolling = true;

        StartCoroutine(UpdateScore());
    }




    private IEnumerator UpdateScore()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);              // Wait for 0.5 seconds
            score += 1;                                         // Increase the score by 1
            scoreTextObject.text = $"Score: {score}";           // Update the score text
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

}