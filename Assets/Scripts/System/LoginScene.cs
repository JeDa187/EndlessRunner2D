using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class LoginScene : MonoBehaviour
{
    public TMP_InputField playerNameInputField;
    public TextMeshProUGUI errorMessage;

    public void OnContinueButtonClicked()
    {
        string playerName = playerNameInputField.text;
        if (playerName.Length != 4)
        {
            errorMessage.text = "Nimen on oltava tasan 4 kirjainta.";
        }
        else
        {
            // Kirjaudu PlayFab-palveluun ja tarkista pelaajan nimi
            CheckPlayerName(playerName);
        }
    }



    private void CheckPlayerName(string playerName)
    {
        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest
        {
            CustomId = playerName,
            CreateAccount = true,
        }, result =>
        {
            // Onnistunut kirjautuminen
            // Aseta DisplayName samaksi kuin CustomId
            PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = playerName,
            }, nameResult =>
            {
                // Onnistunut nimen päivitys
                // Siirry seuraavaan kohtaukseen
                SceneManager.LoadScene("MainMenu");
            }, nameError =>
            {
                // Virhe nimeä päivitettäessä
                errorMessage.text = nameError.ErrorMessage;
            });
        }, error =>
        {
            // Virhe kirjautuessa
            errorMessage.text = error.ErrorMessage;
        });
    }

}
