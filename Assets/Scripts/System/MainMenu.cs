using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI; // Required for UI elements


public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI playerStatusText;
    public TextMeshProUGUI errorMessage;  // Text field for displaying error messages
    public Button accountManagerButton;  // Reference to the Account Manager button
    public Color dimmedButtonColor = new Color(0.7f, 0.7f, 0.7f); // dimmed color for the button                                                                  // Dimmed color for the button
    private Color originalButtonColor; // Store the original color of the button

    private void Start()
    {
        // Get the original color of the button
        originalButtonColor = accountManagerButton.image.color;

        if (IsOnline())
        {
            playerStatusText.text = SecurePlayerPrefs.GetString("PlayerName", "Unknown");
            accountManagerButton.image.color = originalButtonColor; // Set the button to its original color
        }
        else
        {
            playerStatusText.text = "Offline Mode";
            accountManagerButton.image.color = dimmedButtonColor; // Dim the button color
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OpenLeaderboard()
    {
        SceneManager.LoadScene("Leaderboard");
    }

    public void OpenAccountManager()
    {
        if (IsOnline())
        {
            SceneManager.LoadScene("AccountManager");
        }
        else
        {
            errorMessage.text = "User Statistics is not available in Offline Mode.";
        }
    }

    public void Logout()
    {
        if (SecurePlayerPrefs.GetInt("PrivacyPolicyAccepted", 0) == 0)
        {
            SceneManager.LoadScene("PrivacyPolicyScene");
        }
        else
        {
            SecurePlayerPrefs.DeleteKey("Online");
            SecurePlayerPrefs.DeleteKey("RememberMe"); 
            SceneManager.LoadScene("LoginScene");
        }
    }

    public void QuitGame()
    {
        // Call QuitManager's ToggleQuitPanel method to display the exit window
        if (QuitManager.Instance != null)
        {
            QuitManager.Instance.ToggleQuitPanel();
        }
        else
        {
            Debug.LogWarning("QuitManager.Instance not found.");
        }
    }

    public void CharacterSelection()
    {
        // Tuhotaan instanssi jos se on olemassa
        CharacterSelection instance = FindObjectOfType<CharacterSelection>();
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }

        // Ladataan skene
        SceneManager.LoadScene("CharacterSelection");
    }

    private bool IsOnline()
    {
        return SecurePlayerPrefs.GetInt("Online") == 1;
    }
}
