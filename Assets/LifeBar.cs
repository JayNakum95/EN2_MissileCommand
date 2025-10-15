using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class LifeBar : MonoBehaviour
{
    private float ratio_;
    private Slider slide_;
    private void Awake() { slide_ = GetComponent<Slider>(); }
    public void SetGaugeRatio(float ratio) { ratio_ = Mathf.Clamp01(ratio);
    slide_.value = ratio_; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
