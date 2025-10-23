using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem.Composites;

public class Missile : MonoBehaviour
{
    [SerializeField] private Explosion explosionPrefab_;
    [SerializeField] private float speed_;
    private Vector3 velocity;
    private GameObject reticle_;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void SetUp(GameObject reticle)
    {
        reticle_ = reticle;
        if (reticle.transform.position != transform.position)
        {
            Vector3 direction = (reticle.transform.position - transform.position).normalized;
            velocity = direction * speed_;
            //Œü‚«‚ÌŒvŽZ
            LookAtReticle();
        }
        else
        {
            velocity = Vector3.zero;
        }

    }
    private void SetupVelocity()
    {
        Vector3 direction = (reticle_.transform.position - transform.position);
        Assert.IsTrue(direction != Vector3.zero);
        direction = direction.normalized;
        velocity = direction * speed_;

    }
    private void LookAtReticle() {
    float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        angle -= 90.0f;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

    }
    private void Explosion() { 
    Instantiate(explosionPrefab_, transform.position, Quaternion.identity);
        Destroy(reticle_);
        Destroy(gameObject);





    }
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        float distanceSqr = Vector3.SqrMagnitude(reticle_.transform.position - transform.position);
        Vector3 velocityDeltaTime = velocity * Time.deltaTime;
        float velocityDistanceSqr = Vector3.SqrMagnitude(velocityDeltaTime);
        if (distanceSqr >= velocityDistanceSqr)
        {
            transform.position += velocityDeltaTime;
            return;

        }
        transform.position = reticle_.transform.position;
        Explosion();
    }
}
