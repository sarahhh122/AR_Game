using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public int scoreMultiplier = 1;
    public int CurrentScore { get; private set; } = 0;

    public void AddPoint()
    {
        CurrentScore += 1 * scoreMultiplier;
        scoreText.text = "Score: " + CurrentScore;
    }
}