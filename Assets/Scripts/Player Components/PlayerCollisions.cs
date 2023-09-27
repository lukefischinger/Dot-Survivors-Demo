using UnityEngine;

// player component
// Manages player collisions, e.g. with enemies and experience
public class PlayerCollisions : MonoBehaviour {

    [SerializeField] GameObject damagePrefab;
    ObjectManager objects;

    float hitCooldownRemaining,
          hitCooldown = 0.3f;
    HealthManager health;
    Pool experiencePool, damagePool;
    SpriteRenderer spriteRenderer;
    Transform myTransform;


    private void Awake() {
        objects = GameObject.Find("RunManager").GetComponent<ObjectManager>();
        experiencePool = objects.experiencePool.GetComponent<Pool>();
        damagePool = objects.damagePool.GetComponent<Pool>();
        myTransform = transform;

        health = GetComponent<HealthManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
            float experience = collision.gameObject.GetComponent<Experience>().experienceAmount;

            GetComponent<ExperienceManager>().AddExperience((int)experience);
            experiencePool.ReturnPooledObject(collision.gameObject);
            
            GameObject experienceUI = damagePool.GetPooledObject();
            if (experienceUI != null)
                experienceUI.GetComponent<Damage>().SetDamage(experience, myTransform.position + 1.4f * Vector3.up, "Experience", experience >= 10 ? 2f : 1f);
        }
    }

    private void Update() {
        hitCooldownRemaining -= Time.deltaTime;
        if (hitCooldownRemaining > 0) {
            spriteRenderer.color = Color.red;
        }
        else {
            spriteRenderer.color = Color.white;
        }
    }
}
