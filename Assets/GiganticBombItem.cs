using UnityEngine;

public class GiganticBombItem : ItemBase
{
    [SerializeField] Explosion giganticExplosionfab_;

    public override void Get()
    {
        Instantiate(giganticExplosionfab_, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }


}
