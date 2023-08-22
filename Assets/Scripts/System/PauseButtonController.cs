using UnityEngine;
using UnityEngine.UI;

public class PauseButtonController : MonoBehaviour
{
    [SerializeField] private Button pauseButton;
    [SerializeField] private GameObject pauseMenuPanel;

    private void Start()
    {
        pauseButton.onClick.AddListener(ShowPauseMenu);
    }

    private void ShowPauseMenu()
    {
        Time.timeScale = 0f; // Pause the game
        pauseMenuPanel.SetActive(true); // Show the pause menu
    }
}
