using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class LifeBar : MonoBehaviour
{
    private Slider slide_;
    private float ratio_;
    private bool isDeadTriggered = false;

    private void Awake()
    {
        slide_ = GetComponent<Slider>();
    }

    /// <summary>
    /// Update HP ratio (0.0 ~ 1.0)
    /// </summary>
    public void SetGaugeRatio(float ratio)
    {
        ratio_ = Mathf.Clamp01(ratio);
        slide_.value = ratio_;

        // Trigger death only once
        if (!isDeadTriggered && ratio_ <= 0f)
        {
            isDeadTriggered = true;

            // Safe check
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnPlayerDeath();
            }
            else
            {
                Debug.LogWarning("GameManager.Instance is NULL!");
            }
        }
    }

    public float GetRatio()
    {
        return ratio_;
    }
}
