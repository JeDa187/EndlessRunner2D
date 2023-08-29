using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class LoginScene : MonoBehaviour
{
    public TMP_InputField playerNameInputField; // Input field for player name
    public TextMeshProUGUI errorMessage; // Text field for displaying error messages

    // Method called when the "Continue" button is clicked
    public void OnContinueButtonClicked()
    {
        string playerName = playerNameInputField.text;
        // Check if the player's name is exactly 4 characters long
        if (playerName.Length != 4)
        {
            errorMessage.text = "Nimen on oltava tasan 4 kirjainta.";
        }
        else
        {
            // If the name is 4 characters long, log in to PlayFab service and check the player's name
            CheckPlayerName(playerName);
        }
    }

    // Method to check the player's name and log in to PlayFab
    private void CheckPlayerName(string playerName)
    {
        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest
        {
            CustomId = playerName,
            CreateAccount = true,
        }, result =>
        {
            // Successful login
            // Set the DisplayName to be the same as the CustomId
            PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = playerName,
            }, nameResult =>
            {
                // Successful name update
                // Move to the next scene
                SceneManager.LoadScene("MainMenu");
            }, nameError =>
            {
                // Error updating the name
                errorMessage.text = nameError.ErrorMessage;
            });
        }, error =>
        {
            // Error logging in
            errorMessage.text = error.ErrorMessage;
        });
    }
}
