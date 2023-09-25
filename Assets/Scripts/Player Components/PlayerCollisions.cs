using UnityEngine;

// player component
// Manages player collisions, e.g. with enemies and experience
public class PlayerCollisions : MonoBehaviour {

    [SerializeField] GameObject damagePrefab;
    ObjectManager objects;

    float hitCooldownRemaining,
          hitCooldown = 0.3f;
    HealthManager health;
    Transform canvasTransform;
    Pool experiencePool, damagePool;


    private void Awake() {
        objects = GameObject.Find("RunManager").GetComponent<ObjectManager>();

        health = GetComponent<HealthManager>();
        canvasTransform = objects.canvas.transform;
        experiencePool = objects.experiencePool.GetComponent<Pool>();
        damagePool = objects.damagePool.GetComponent<Pool>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        ProcessEnemyHit(collision);
        ProcessExperience(collision);
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (hitCooldownRemaining <= 0)
            ProcessEnemyHit(collision);

    }

    void ProcessEnemyHit(Collider2D collision) {
        if (collision.gameObject.tag == "Enemy") {
            hitCooldownRemaining = hitCooldown;

            float damage = collision.GetComponent<Enemy>().GetDamage();
            health.Damage(damage, "Red");


        }
    }

    void ProcessExperience(Collider2D collision) {
        if (collision.gameObject.tag == "Experience") {
            GetComponent<ExperienceManager>().AddExperience(collision.gameObject.GetComponent<Experience>().experienceAmount);
            experiencePool.ReturnPooledObject(collision.gameObject);
        }
    }

    private void Update() {
        hitCooldownRemaining -= Time.deltaTime;
    }
}
