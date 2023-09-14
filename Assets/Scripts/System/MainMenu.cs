using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI playerStatusText; // Viittaus uuteen UI-elementtiin.
   
    private void Start()
    {
        // P‰ivit‰ teksti riippuen siit‰, onko k‰ytt‰j‰ kirjautunut sis‰‰n vai ei.
        if (PlayerPrefs.GetInt("Online") == 1)
        {
          
            //Kirjautuneen k‰ytt‰j‰n nimi on tallennettu PlayerPrefsiin.
            string playerName = PlayerPrefs.GetString("PlayerName", "Unknown");
            playerStatusText.text = playerName;
        }
        else
        {
        
            playerStatusText.text = "Offline Mode";
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
    public void Logout()
    {
        // Jos ehtoja EI ole hyv‰ksytty
        if (PlayerPrefs.GetInt("PrivacyPolicyAccepted", 0) == 0)
        {
            SceneManager.LoadScene("PrivacyPolicyScene");
        }
        // Jos ehtoja on hyv‰ksytty, riippumatta siit‰ onko k‰ytt‰j‰ online vai offline, siirryt‰‰n LoginSceneen
        else
        {
            PlayerPrefs.DeleteKey("Online"); // Poistetaan online-tila
            SceneManager.LoadScene("LoginScene");
        }
    }


    public void QuitGame()
    {
        Application.Quit();
    }
    public void CharacterSelection()
    {
        SceneManager.LoadScene("CharacterSelection");
    }
}
