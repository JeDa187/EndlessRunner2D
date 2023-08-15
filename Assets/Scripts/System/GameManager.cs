using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;           // Panel displayed when the game is over
    [SerializeField] private TMP_Text countdownTextObject;       // Text object for the countdown

    private float countdownTime = 3.0f;                          // Time for the countdown before the game starts
    public static GameManager Instance;                          // Singleton instance of the GameManager

    private void Awake()
    {
        SetupSingleton();                                        // Setup the Singleton instance
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
        Time.timeScale = 0;                                      // Pause the game time
    }

    public void RestartGame()
    {
        Time.timeScale = 1;                                      // Resume game time
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  // Reload the current scene
    }

    private IEnumerator CountdownToStart()
    {
        DragonflyController dragonfly = FindObjectOfType<DragonflyController>();  // Find the Dragonfly object

        dragonfly?.ToggleRigidbodyMovement(false);              // Disable dragonfly movement

        float currentCountdown = countdownTime;                 // Initialize countdown

        while (currentCountdown > 0)
        {
            countdownTextObject.text = currentCountdown.ToString("0");
            yield return new WaitForSeconds(1.0f);
            currentCountdown--;
        }

        countdownTextObject.text = "";                          // Clear countdown text

        if (dragonfly != null)
        {
            dragonfly.ResetDragonfly();                         // Reset the dragonfly position and state
            dragonfly.ToggleRigidbodyMovement(true);            // Enable dragonfly movement
        }
    }
}
