using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class LoginScene : MonoBehaviour
{
    public TMP_InputField playerNameInputField; // Input field for player name
    public TextMeshProUGUI errorMessage; // Text field for displaying error messages
    public TMP_InputField passwordInputField; // Input field for password

    public void OnContinueButtonClicked()
    {
        string playerName = playerNameInputField.text;
        string password = passwordInputField.text;

        if (playerName.Length != 4)
        {
            errorMessage.text = "Nimen on oltava tasan 4 kirjainta.";
        }
        else if (password.Length < 5)
        {
            errorMessage.text = "Salasanan on oltava vähintään 5 merkkiä pitkä.";
        }
        else
        {
            errorMessage.text = "logging in";
            CheckPlayerName(playerName, password);
        }
    }

    private void CheckPlayerName(string playerName, string password)
    {
        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
        {
            Username = playerName,
            Password = password,
        }, result =>
        {
            // Successful login
            // Update the display name and move to the next scene
            UpdateDisplayName(playerName);
        }, error =>
        {
            if (error.Error == PlayFabErrorCode.AccountNotFound)
            {
                // Account not found
                // Show creating new user message and try to create a new account
                errorMessage.text = "creating new user";
                CreateNewAccount(playerName, password);
            }
            else
            {
                // Other error
                errorMessage.text = error.ErrorMessage;
            }
        });
    }

    private void CreateNewAccount(string playerName, string password)
    {
        PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
        {
            Username = playerName,
            Password = password,
            RequireBothUsernameAndEmail = false
        }, result =>
        {
            // Successful registration
            // Update the display name and move to the next scene
            UpdateDisplayName(playerName);
        }, error =>
        {
            // Error registering
            errorMessage.text = error.ErrorMessage;
        });
    }

    private void UpdateDisplayName(string playerName)
    {
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = playerName,
        }, result =>
        {
            // Successful name update
            // Move to the next scene
            SceneManager.LoadScene("MainMenu");
        }, error =>
        {
            // Error updating the name
            errorMessage.text = error.ErrorMessage;
        });
    }
}
