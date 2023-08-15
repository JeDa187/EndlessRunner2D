using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;  // Singleton rakenne
    public TMP_Text countdownTextObject; // Drag your TextMeshPro UI object here from the inspector
    private float countdownTime = 3.0f;

    public GameObject gameOverPanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        gameOverPanel.SetActive(false);
        StartCoroutine(CountdownToStart());
    }

    private IEnumerator CountdownToStart()
    {
        float currentCountdown = countdownTime;

        while (currentCountdown > 0)
        {
            countdownTextObject.text = currentCountdown.ToString("0");
            yield return new WaitForSeconds(1.0f);
            currentCountdown--;
        }

        countdownTextObject.text = ""; // Clear the countdown text
        DragonflyController dragonfly = Object.FindFirstObjectByType<DragonflyController>();
        if (dragonfly != null)
        {
            dragonfly.ResetDragonfly();
        }
    }
}
