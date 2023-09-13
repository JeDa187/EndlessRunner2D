using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class PrivacyPolicyManager : MonoBehaviour
{
    // Variables for the "Accept" and "Decline" buttons.
    public Button acceptButton;
    public Button declineButton;
    public TMP_Text privacyPolicyText; // Reference to the TextMeshPro text.

    private void Start()
    {
        // Attach Click events to the buttons in the script.
        acceptButton.onClick.AddListener(OnAcceptButtonClicked);
        declineButton.onClick.AddListener(OnDeclineButtonClicked);
    }

    private void Update()
    {
        // Check for a mouse click (relevant for browser-based gameplay)
        if (Input.GetMouseButtonDown(0))
        {
            CheckForLink(Input.mousePosition);
        }

        // Check for a touchscreen tap (relevant for mobile/touchscreen gameplay)
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            CheckForLink(Input.GetTouch(0).position);
        }
    }

    private void CheckForLink(Vector2 position)
    {
        // Find a link at the given position (either mouse click or touch tap position)
        var linkIndex = TMP_TextUtilities.FindIntersectingLink(privacyPolicyText, position, null);

        // If a link is found at the position, handle the link click
        if (linkIndex != -1)
        {
            var linkInfo = privacyPolicyText.textInfo.linkInfo[linkIndex];
            OnLinkClicked(linkInfo);
        }
    }

    // This function is called when a link is clicked.
    public void OnLinkClicked(TMP_LinkInfo linkInfo)
    {
        if (linkInfo.GetLinkID() == "PlayFabTerms")
        {
            Application.OpenURL("https://playfab.com/terms/");
        }
    }

    // This function is called when the "Accept" button is clicked.
    public void OnAcceptButtonClicked()
    {
        // Save a value indicating that the user has accepted the terms.
        PlayerPrefs.SetInt("PrivacyPolicyAccepted", 1);
        // Load the main menu or the next scene.
        SceneManager.LoadScene("LoginScene");
    }

    // This function is called when the "Decline" button is clicked.
    public void OnDeclineButtonClicked()
    {
        // Save a value indicating that the user has declined the terms.
        PlayerPrefs.SetInt("PrivacyPolicyAccepted", 0);

        // Set the player's preference value "Online" to 0 (offline).
        PlayerPrefs.SetInt("Online", 0);

        // Save changes in the PlayerPrefs class.
        PlayerPrefs.Save();

        // Load the "CharacterSelection" scene.
        SceneManager.LoadScene("CharacterSelection");
    }
}
