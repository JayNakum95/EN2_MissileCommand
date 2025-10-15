using JetBrains.Annotations;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float fallSpeedMin_ = 1.0f;
    [SerializeField] private float fallSpeedMax_ = 3.0f;
    private Explosion explosionPrefab_;
    private BoxCollider2D groundCollider_;
    // Change the type of rb_ from Rigidbody to Rigidbody2D
    private Rigidbody2D rb_;
    private GameManager gameManager_;
    void Start()
    {
        rb_ = GetComponent<Rigidbody2D>();
        SetUpVelocity();
    }
    public void Setup(BoxCollider2D ground, GameManager gameManager, Explosion explosionPrefab)
    {
        groundCollider_ = ground;
        gameManager_ = gameManager;
        explosionPrefab_ = explosionPrefab;
    }
    private void SetUpVelocity()
    {

        float left = groundCollider_.bounds.center.x - groundCollider_.bounds.size.x / 2;
        float right = groundCollider_.bounds.center.x + groundCollider_.bounds.size.x / 2;
        float top = groundCollider_.bounds.center.y + groundCollider_.bounds.size.y / 2;
        float bottom = groundCollider_.bounds.center.y - groundCollider_.bounds.size.y / 2;

        float targetX=Mathf.Lerp(left,right,Random.Range(0.0f,1.0f));
        Vector3 target = new Vector3(targetX, top, 0);
        Vector3 direction = (target - transform.position).normalized;
        float fallSpeed = Random.Range(fallSpeedMin_, fallSpeedMax_);
        rb_.linearVelocity = direction * fallSpeed;



    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Explosion")) { 
        Explosion();

        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            Fall();
        }



        }
    private void Explosion() {
    gameManager_.AddScore(100);
        Instantiate(explosionPrefab_, transform.position, Quaternion.identity);
        Destroy(gameObject);





    }
    private void Fall() {
        gameManager_.Damage(1);
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
