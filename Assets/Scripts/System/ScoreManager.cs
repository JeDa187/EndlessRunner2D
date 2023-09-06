using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : MonoBehaviour
{

    [SerializeField] private TMP_Text scoreTextObject; // Text object to display the player's current score
    [SerializeField] private TMP_Text highScoreTextObject; // Text object to display the highest score
    private int score = 0; // Player's current score
    private int highScore = 0; // Highest score achieved so far
    private const string HighScoreKey = "HighScore"; // Key used to save/load high score with PlayerPrefs

    private DragonflyController dragonflyController;


    private void Start()
    {
        dragonflyController = FindObjectOfType<DragonflyController>();
    }

    public int GetScore()
    {
        return score;
    }
    public TMP_Text GetScoreTextObject()
    {
        return scoreTextObject;
    }
    public void InitializeScore()
    {
        score = 0;
        scoreTextObject.text = $"Score: {score}";
        scoreTextObject.gameObject.SetActive(true);
    }
    public void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt(HighScoreKey, 0); // Load high score from PlayerPrefs or default to 0
        highScoreTextObject.text = $"High Score: {highScore}"; // Update high score text object
    }
    public void UpdateHighScore()
    {
        if (score > highScore)
        {
            highScore = score; // Update high score if player's score is higher
            PlayerPrefs.SetInt(HighScoreKey, highScore); // Save new high score to PlayerPrefs
        }
        highScoreTextObject.text = $"High Score: {highScore}"; // Update high score text object
    }

    public IEnumerator UpdateScore()
    {
        while (true)
        {
            int multiplier = dragonflyController.GetScoreMultiplier();

            // Update every 0.1 seconds when the multiplier is active
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