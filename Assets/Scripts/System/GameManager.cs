using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text countdownTextObject;

    private float countdownTime = 3.0f;
    public static GameManager Instance;

    private void Awake()
    {
        SetupSingleton();
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
        Time.timeScale = 1; // Ensure game time is normalized
        StartCoroutine(CountdownToStart());
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator CountdownToStart()
    {
        DragonflyController dragonfly = FindObjectOfType<DragonflyController>();

        dragonfly?.ToggleRigidbodyMovement(false);

        float currentCountdown = countdownTime;

        while (currentCountdown > 0)
        {
            countdownTextObject.text = currentCountdown.ToString("0");
            yield return new WaitForSeconds(1.0f);
            currentCountdown--;
        }

        countdownTextObject.text = "";

        if (dragonfly != null)
        {
            dragonfly.ResetDragonfly();
            dragonfly.ToggleRigidbodyMovement(true);
        }
    }
}
