using UnityEngine;

public class ProjectileCollisions : MonoBehaviour
{

    [SerializeField] GameObject damagePrefab;

    // set by weapon upon firing
    float hitCount;

    protected float coolDownRemaining = 0;
    protected float coolDown = 0.4f;

    Weapon weapon;
    GameObject lastEnemy;
    Transform canvasTransform;

    private void Awake()
    {
        coolDownRemaining = 0;
        weapon = GameObject.FindGameObjectWithTag("Player").GetComponent<Weapon>();
        canvasTransform = GameObject.Find("Canvas").transform;
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
            return;
        }

        coolDownRemaining = coolDown;
        hitCount--;

        // apply damage to the collided enemy
        lastEnemy = collision.gameObject;
        float damage = weapon.GetDamage();
        lastEnemy.GetComponent<Enemy>().Damage(damage);

        // display damage
        Damage damageUI = Instantiate(damagePrefab).GetComponent<Damage>();
        damageUI.transform.SetParent(canvasTransform, false);
        damageUI.SetDamage(damage, lastEnemy.transform.position, Color.white);


        if (hitCount <= 0)
        {
            Destroy(gameObject);
        }

    }

    

  
  


}
