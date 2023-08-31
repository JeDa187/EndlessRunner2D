using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OpenLeaderboard()
    {
        SceneManager.LoadScene("Leaderboard");
    }
    public void Logout()
    {
        PlayerPrefs.DeleteKey("Online");
        SceneManager.LoadScene("LoginScene");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
