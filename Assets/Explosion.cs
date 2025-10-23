using UnityEngine;

public class Explosion : MonoBehaviour
{

    private float maxLifeTime_= 1.0f;
    private float time_= 0.0f;
    public int chainNum = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        time_ = maxLifeTime_;

    }

    // Update is called once per frame
    void Update()
    {
        time_-= Time.deltaTime;
        if (time_ > 0) {
            return;
        }
        Destroy(gameObject);

    }
}
