using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System; // Lis‰‰ t‰m‰ k‰ytt‰‰ksesi Action-tyyppi‰

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text countdownTextObject;
    [SerializeField] private TMP_Text scoreTextObject;
    [SerializeField] private TMP_Text highScoreTextObject;

    private const string HighScoreKey = "HighScore";
    private float countdownTime = 3.0f;
    private int score = 0;
    private int highScore = 0;
    public static GameManager Instance;

    private InfiniteParallaxBackground parallax;

    // Lis‰‰ t‰m‰ rivi m‰‰ritell‰ksesi tapahtuman
    public event Action OnCountdownFinished;

    private void Awake()
    {
        SetupSingleton();
        LoadHighScore();
        parallax = FindObjectOfType<InfiniteParallaxBackground>();
    }

    private void Start()
    {
        InitializeGame();
    }

    private void SetupSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeGame()
    {
        Time.timeScale = 1;
        StartCoroutine(CountdownToStart());
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        scoreTextObject.gameObject.SetActive(false);
        Time.timeScale = 0;
        UpdateHighScore();
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        highScoreTextObject.text = $"High Score: {highScore}";
    }

    private void UpdateHighScore()
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt(HighScoreKey, highScore);
        }
        highScoreTextObject.text = $"High Score: {highScore}";
    }

    private IEnumerator CountdownToStart()
    {
        DragonflyController dragonfly = FindObjectOfType<DragonflyController>();
        if (dragonfly != null)
        {
            dragonfly.ToggleRigidbodyMovement(false);
        }

        parallax.enableScrolling = false;

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

        parallax.enableScrolling = true;

        // Lis‰‰ t‰m‰ rivi laukaistaksesi tapahtuman
        OnCountdownFinished?.Invoke();

        StartCoroutine(UpdateScore());
    }

    private IEnumerator UpdateScore()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            score += 1;
            scoreTextObject.text = $"Score: {score}";
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
