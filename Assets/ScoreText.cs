using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    private int score_;
    private TMP_Text scoreText_;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score_ = 0;
        scoreText_=GetComponent<TMP_Text>();
    }
    public void SetScore(int score) {
        score_ = score;
        UpdateScoreText();
    
    }
    private void UpdateScoreText() {
        scoreText_.text = $"SCORE:{score_:00000000}";
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
