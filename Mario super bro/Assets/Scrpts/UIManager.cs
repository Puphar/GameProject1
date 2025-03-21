using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Reference to the score text UI element

    private void Start()
    {
        // Initialize UI elements
        UpdateScoreUI(new ScoreData()); // Update the score UI with default ScoreData (or provide a specific ScoreData instance)
        // Initialize other UI elements as needed
    }

    public void UpdateScoreUI(ScoreData scoreData)
    {
        if (scoreText != null && scoreData != null)
        {
            // Update the score text with the score value from the ScoreData object
            scoreText.text = "Score: " + scoreData.scoreValue.ToString();
        }
    }
}