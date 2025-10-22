using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class ScoreEffect : MonoBehaviour
{
    [SerializeField]
    float upSpeed = 1f;

    [SerializeField]
    float lifeTime = 1f; 

    float aliveTimer = 0f;

    TMP_Text tmpText;

    void Awake()
    {
        tmpText = GetComponent<TMP_Text>();
    }

    public void SetScore(int score)
    {
        if (tmpText != null)
            tmpText.text = score.ToString();
    }

    void Update()
    {
        aliveTimer += Time.deltaTime;
        if (aliveTimer >= lifeTime)
        {
            Destroy(gameObject);
            return;
        }

        transform.position += Vector3.up * upSpeed * Time.deltaTime;
    }
}
