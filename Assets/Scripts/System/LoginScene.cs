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
        PlayerPrefs.SetInt("Online", 1); // 1 for online

        if (playerName.Length != 4)
        {
            errorMessage.text = "The name must contain 4 characters.";
        }
        else if (password.Length < 5)
        {
            errorMessage.text = "The password must be at least 5 characters long.";
        }
        else
        {
            PlayerPrefs.SetString("PlayerName", playerName); // Tallenna kirjautuneen käyttäjän nimi.
            CheckPlayerName(playerName, password);
        }
    }
    public void OnPlayOfflineButtonClicked()
    {
        PlayerPrefs.SetInt("Online", 0); // 0 for offline
        errorMessage.text = "Starting game in offline mode";
        Invoke("LoadMainMenuScene", 3.0f);
    }

    private void LoadMainMenuScene()
    {
        SceneManager.LoadScene("CharacterSelection");
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
            // Show logging in message
            errorMessage.text = "Logging in to existing account";
            // Wait for 3 seconds, then update the display name and move to the next scene
            Invoke("UpdateDisplayNameAndMoveToNextScene", 3.0f);
        }, error =>
        {
            if (error.Error == PlayFabErrorCode.AccountNotFound)
            {
                // Account not found
                // Show creating new user message and try to create a new account
                errorMessage.text = "Creating new user";
                Invoke("CreateNewAccountAndMoveToNextScene", 3.0f);
            }
            else
            {
                // Other error
                errorMessage.text = error.ErrorMessage;
            }
        });
    }

    private void UpdateDisplayNameAndMoveToNextScene()
    {
        string playerName = playerNameInputField.text;
        UpdateDisplayName(playerName);
    }

    private void CreateNewAccountAndMoveToNextScene()
    {
        string playerName = playerNameInputField.text;
        string password = passwordInputField.text;
        CreateNewAccount(playerName, password);
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
            SceneManager.LoadScene("CharacterSelection");
        }, error =>
        {
            // Error updating the name
            errorMessage.text = error.ErrorMessage;
        });
    }
}