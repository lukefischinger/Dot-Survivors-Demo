using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] float damage;
    
    ObjectManager objects;

    const int baseSpeed = 140;
    const float experienceProbability = 0.3f;

    float currentHealth;

    Transform myTransform, playerTransform;
    Rigidbody2D myRigidbody;
    Pool enemyPool, experiencePool, damagePool;

    private void Awake() {
        objects = GameObject.Find("RunManager").GetComponent<ObjectManager>();

        myTransform = transform;
        myRigidbody = GetComponent<Rigidbody2D>();
        playerTransform = objects.player.transform;
        enemyPool = objects.enemyPool.GetComponent<Pool>();
        experiencePool = objects.experiencePool.GetComponent<Pool>();
        damagePool = objects.damagePool.GetComponent<Pool>();

        Reset();

    }


    private void FixedUpdate() {
        Move();
    }

    // move towards the player at a constant speed
    private void Move() {
        myRigidbody.velocity = baseSpeed * Time.deltaTime * (playerTransform.position - myTransform.position).normalized;
    }


    public void Damage(float damage, Color color) {

        currentHealth -= damage;

        // display damage
        Damage damageUI = damagePool.GetPooledObject().GetComponent<Damage>();
        damageUI.SetDamage(damage, myTransform.position, color);


        if (currentHealth <= 0)
            Kill(true); 
    }

    // called when health <= 0
    private void Kill(bool canDropExperience) {
        if (canDropExperience && Random.value < experienceProbability) {
            Transform experience = experiencePool.GetPooledObject().transform;
            experience.transform.position = myTransform.position;
        }

        enemyPool.ReturnPooledObject(gameObject);

    }

    // returns the damage inflicted by this enemy on the player
    public float GetDamage() {
        return damage;
    }

    public void Reset() {
        currentHealth = health;
    }


}
