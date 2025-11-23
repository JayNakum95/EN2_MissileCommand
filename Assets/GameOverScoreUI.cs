using UnityEngine;
using TMPro;

public class GameOverScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    void Start()
    {
        int lastScore = PlayerPrefs.GetInt("LastScore", 0);
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        scoreText.text = "Score : " + lastScore + "\nHigh Score : " + highScore;

    }
}
