using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : MonoBehaviour
{
    // Serialized fields for assigning in the editor
    [SerializeField] private TMP_Text scoreTextObject; // UI element for displaying the player's current score.
    [SerializeField] private TMP_Text highScoreTextObject; // UI element for displaying the highest score ever achieved.

    private int score = 0; // Keeps track of the current in-game score.
    private int highScore = 0; // Keeps track of the highest score achieved across all play sessions.

    // Key for PlayerPrefs to store and retrieve the high score.
    private const string HighScoreKey = "HighScore";

    // Reference to the DragonflyController which provides the score multiplier.
    private DragonflyController dragonflyController;

    private void Start()
    {
        // Get the DragonflyController component at the start of the game.
        dragonflyController = FindObjectOfType<DragonflyController>();
    }

    // Method to get the current score.
    public int GetScore()
    {
        return score;
    }

    // Method to get the text object displaying the score (useful for other scripts).
    public TMP_Text GetScoreTextObject()
    {
        return scoreTextObject;
    }

    // Initializes the score at the start of a new game or round.
    public void InitializeScore()
    {
        score = 0;
        scoreTextObject.text = $"Score: {score}";
        scoreTextObject.gameObject.SetActive(true); // Ensures the score is visible.
    }

    // Load the highest score ever achieved from PlayerPrefs.
    public void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt(HighScoreKey, 0); // Fetches the high score or defaults to 0 if not found.
        highScoreTextObject.text = $"High Score: {highScore}";
    }

    // Updates the high score if the current score surpasses it.
    public void UpdateHighScore()
    {
        if (score > highScore)
        {
            highScore = score; // Sets the new high score.
            PlayerPrefs.SetInt(HighScoreKey, highScore); // Persists the new high score.
        }
        highScoreTextObject.text = $"High Score: {highScore}";
    }

    // Coroutine to continually update the score based on a multiplier from the DragonflyController.
    public IEnumerator UpdateScore()
    {
        while (true) // Runs indefinitely, be cautious of infinite loops.
        {
            int multiplier = dragonflyController.GetScoreMultiplier();

            // Determines the frequency of score updates based on the multiplier.
            // If multiplier is greater than 1, updates faster.
            float updateTime = (multiplier > 1) ? 0.4f : 0.5f;

            for (int i = 0; i < multiplier; i++)
            {
                score += 1;
                scoreTextObject.text = $"Score: {score}";
                yield return new WaitForSeconds(updateTime / multiplier);
            }
        }
    }
}
