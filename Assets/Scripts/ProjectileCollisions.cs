using UnityEngine;

public class ProjectileCollisions : MonoBehaviour
{

    // set by weapon upon firing
    float hitCount;

    protected float coolDownRemaining = 0;
    protected float coolDown = 0.4f;

    Weapon weapon;
    GameObject lastEnemy;

    private void Awake()
    {
        coolDownRemaining = 0;
        weapon = GameObject.FindGameObjectWithTag("Player").GetComponent<Weapon>();
    }


    private void Update()
    {
        coolDownRemaining -= Time.deltaTime;
    }


    public void SetProperties(float hitCount)
    {
        this.hitCount = hitCount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != lastEnemy)
        {
            HitEnemy(collision);
            Debug.Log("Trigger");

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (coolDownRemaining <= 0)
        {
             HitEnemy(collision);
        }
    }

    protected void HitEnemy(Collider2D collision)
    {
        if(hitCount <= 0 || collision.gameObject.tag != "Enemy")
        {
            Debug.Log(hitCount);
            return;
        }

        coolDownRemaining = coolDown;
        hitCount--;

        lastEnemy = collision.gameObject;
        lastEnemy.GetComponent<Enemy>().Damage(weapon.GetDamage());
        Debug.Log(collision.gameObject.tag);

        if (hitCount <= 0)
        {
            Destroy(gameObject);
        }

    }

    

  
  


}
