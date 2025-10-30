using UnityEngine;

public class ClusterBombItem : ItemBase
{
    [SerializeField]
    private Explosion explosionFab_;
    bool isGet = false;
    private float explosionEmmitionTimer_ = 0f; // start emitting immediately (or set >0 for delay)
    private float explosionInterval_ = 0.2f;
    private float explosionTimer_ = 3f; // give the cluster some lifetime
    private Renderer renderer_;
    
    public override void Get()
    {
        if (TryGetComponent(out renderer_))
        {
            renderer_.enabled = false;
        }
        collider_.enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
        isGet = true;
    }

    protected override void Update()
    {
       if (!isGet)
       {
           base.Update();
           return;
       }

       // Emit explosions first so they happen before this object is destroyed
       UpdateClusterExplosion();

       explosionTimer_ -= Time.deltaTime;
       if (explosionTimer_ <= 0f)
       { 
           Destroy(gameObject);
       }
    }

    private void UpdateClusterExplosion()
    {
        if (explosionFab_ == null) return; // guard against missing prefab

        explosionEmmitionTimer_ -= Time.deltaTime;
        if (explosionEmmitionTimer_ > 0f)
        {
            return;
        }
        float randomWidth = 2f;
        Vector3 offset = new Vector3(Random.Range(-randomWidth, randomWidth), Random.Range(-randomWidth, randomWidth), 0f);
        Instantiate(explosionFab_, transform.position + offset, Quaternion.identity);
        explosionEmmitionTimer_ += explosionInterval_;
    }
}
