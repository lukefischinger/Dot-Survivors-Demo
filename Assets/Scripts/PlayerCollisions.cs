using UnityEngine;


// player component
// Manages player collisions, e.g. with enemies
public class PlayerCollisions : MonoBehaviour
{

    float hitCooldownRemaining, 
          hitCooldown = 0.3f;
    HealthManager health;


    private void Start() {
        health = GetComponent<HealthManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        ProcessEnemyHit(collision.collider);
    }
    void OnCollisionStay2D(Collision2D collision) {
        if (hitCooldownRemaining <= 0)
            ProcessEnemyHit(collision.collider);
    }

    void ProcessEnemyHit(Collider2D collision) {
        if (collision.gameObject.tag == "Enemy") {
            hitCooldownRemaining = hitCooldown;
            health.Damage(collision.GetComponent<Enemy>().GetDamage());
        }
    }
}
