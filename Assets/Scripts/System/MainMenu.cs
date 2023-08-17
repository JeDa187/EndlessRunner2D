using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Function called when "Start Game" button is pressed
    public void StartGame()
    {
        // Load the game scene named "GameScene"
        SceneManager.LoadScene("GameScene");
    }

    // Function called when "Quit Game" button is pressed
    public void QuitGame()
    {
        // Quit the application
        Application.Quit();
    }
}
