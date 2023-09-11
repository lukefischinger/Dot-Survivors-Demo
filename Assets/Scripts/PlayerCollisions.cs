using UnityEngine;


// player component
// Manages player collisions, e.g. with enemies
public class PlayerCollisions : MonoBehaviour
{

    [SerializeField] GameObject damagePrefab;

    float hitCooldownRemaining, 
          hitCooldown = 0.3f;
    HealthManager health;
    Transform canvasTransform;


    private void Awake() {
        health = GetComponent<HealthManager>();
        canvasTransform = GameObject.Find("Canvas").transform;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        ProcessEnemyHit(collision);
    }
    void OnCollisionStay2D(Collision2D collision) {
        if (hitCooldownRemaining <= 0)
            ProcessEnemyHit(collision);
    }

    void ProcessEnemyHit(Collision2D collision) {
        if (collision.gameObject.tag == "Enemy") {
            hitCooldownRemaining = hitCooldown;
            
            float damage = collision.collider.GetComponent<Enemy>().GetDamage();
            health.Damage(damage);
            
            // display damage
            Damage damageUI = Instantiate(damagePrefab).GetComponent<Damage>();
            damageUI.transform.SetParent(canvasTransform, false);
            damageUI.SetDamage(damage, collision.transform.position, Color.red);
        }
    }

    private void Update() {
        hitCooldownRemaining -= Time.deltaTime;
    }
}
