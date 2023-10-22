using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class LoginScene : MonoBehaviour
{
    // Muuttuja, joka kertoo onko automaattinen kirjautuminen jo yritetty
    private bool autoLoginAttempted = false;

    public TMP_InputField playerNameInputField; // Input field for player name
    public TextMeshProUGUI errorMessage; // Text field for displaying error messages
    public TMP_InputField passwordInputField; // Input field for password
    public Toggle rememberMeToggle; // Toggle for "Remember Me" function

    private void Start()
    {
        // Tarkistetaan, onko "Remember Me" -toiminto aktivoitu ja yritet‰‰n automaattista kirjautumista vain kerran
        if (SecurePlayerPrefs.GetInt("RememberMe", 0) == 1 && !autoLoginAttempted)
        {
            playerNameInputField.text = SecurePlayerPrefs.GetString("PlayerName", "");
            passwordInputField.text = SecurePlayerPrefs.GetString("PlayerPassword", "");
            autoLoginAttempted = true; // Merkit‰‰n automaattinen kirjautuminen yritetyksi
            OnContinueButtonClicked();
        }
    }

    public void OnContinueButtonClicked()
    {
        string playerName = playerNameInputField.text;
        string password = passwordInputField.text;
        SecurePlayerPrefs.SetInt("Online", 1); // 1 for online

        if (rememberMeToggle.isOn) // Save login info if "Remember Me" is checked
        {
            SecurePlayerPrefs.SetString("PlayerName", playerName);
            SecurePlayerPrefs.SetString("PlayerPassword", password);
            SecurePlayerPrefs.SetInt("RememberMe", 1);
        }
        else
        {
            ClearSavedLoginInfo();
            SecurePlayerPrefs.SetInt("RememberMe", 0);
        }

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
            SecurePlayerPrefs.SetString("PlayerName", playerName); // Tallenna kirjautuneen k‰ytt‰j‰n nimi.
            CheckPlayerName(playerName, password);
        }
    }

    public void OnPlayOfflineButtonClicked()
    {
        SecurePlayerPrefs.SetInt("Online", 0); // 0 for offline
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
            errorMessage.text = "Logging in to existing account";
            Invoke("UpdateDisplayNameAndMoveToNextScene", 3.0f);
        }, error =>
        {
            if (error.HttpCode == 0)
            {
                errorMessage.text = "Network error. Please check your connection.";
                return;
            }

            switch (error.Error)
            {
                case PlayFabErrorCode.AccountNotFound:
                    errorMessage.text = "Creating a new user";
                    Invoke("CreateNewAccountAndMoveToNextScene", 3.0f);
                    break;
                case PlayFabErrorCode.InvalidUsernameOrPassword:
                    errorMessage.text = "Invalid username or password. Please try again.";
                    break;
                case PlayFabErrorCode.AccountBanned:
                    errorMessage.text = "This account is banned.";
                    break;
                case PlayFabErrorCode.AccountDeleted:
                    errorMessage.text = "This account has been deleted.";
                    break;
                default:
                    errorMessage.text = error.ErrorMessage;
                    break;
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
            SaveAccountCreationDate();
            UpdateDisplayName(playerName);
        }, error =>
        {
            if (error.HttpCode == 0)
            {
                errorMessage.text = "Network error. Please check your connection.";
                return;
            }

            switch (error.Error)
            {
                case PlayFabErrorCode.NameNotAvailable:
                    errorMessage.text = "Username is already taken. Please choose a different one.";
                    break;
                // Add more specific error cases if needed
                default:
                    errorMessage.text = error.ErrorMessage;
                    break;
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

    private void SaveAccountCreationDate()
    {
        string currentDate = DateTime.UtcNow.Date.ToString("yyyy-MM-dd");

        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { "AccountCreationDate", currentDate }
            }
        },
        result =>
        {
            // This is the response handler for a successful save.
            Debug.Log("Account creation date saved successfully: " + currentDate);
        },
        error =>
        {
            // This is the error handler if something goes wrong.
            Debug.LogError("Failed to save account creation date. Error: " + error.ErrorMessage);
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

    public void ClearSavedLoginInfo()
    {
        SecurePlayerPrefs.DeleteKey("PlayerName");
        SecurePlayerPrefs.DeleteKey("PlayerPassword");
    }
}
