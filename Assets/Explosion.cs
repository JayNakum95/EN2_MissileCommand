using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Explosion : MonoBehaviour
{
    public enum Mode { TransformScale, ColliderRadius }

    [SerializeField] Mode mode = Mode.TransformScale;
    [SerializeField] float growTime = 0.1f;
    [SerializeField] float holdTime = 0.2f;
    [SerializeField] float fadeTime = 0.3f;

    // 🔹 added back so Meteor.cs can access it
    public int chainNum = 0;

    SpriteRenderer sr;
    CircleCollider2D circle;
    Color baseColor;
    Vector3 baseScale;
    float baseRadius;
    float timer;
    float totalLife;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        circle = GetComponent<CircleCollider2D>();

        if (circle != null)
            circle.isTrigger = true;

        // remember prefab settings
        baseColor = sr != null ? sr.color : Color.white;
        baseScale = transform.localScale;
        baseRadius = circle != null ? circle.radius : 0.5f;

        totalLife = growTime + holdTime + fadeTime;
    }

    void Update()
    {
        timer += Time.deltaTime;
        float t = timer / totalLife;

        // scale or radius animation
        if (mode == Mode.TransformScale)
        {
            float k = GetAnimValue();
            transform.localScale = baseScale * k;
        }
        else if (circle != null)
        {
            float k = GetAnimValue();
            circle.radius = baseRadius * k;
        }

        // fade color
        if (sr != null)
        {
            Color c = baseColor;
            c.a = Mathf.Lerp(baseColor.a, 0, Mathf.Clamp01((timer - holdTime) / fadeTime));
            sr.color = c;
        }

        if (timer >= totalLife)
            Destroy(gameObject);
    }

    float GetAnimValue()
    {
        if (timer < growTime)
            return Mathf.Lerp(0.1f, 1f, timer / growTime); // grow
        else if (timer < growTime + holdTime)
            return 1f; // hold
        else
            return Mathf.Lerp(1f, 0.1f, (timer - growTime - holdTime) / fadeTime); // shrink
    }
}
