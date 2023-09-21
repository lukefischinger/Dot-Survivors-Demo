using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] float health;
    [SerializeField] float damage;

    ObjectManager objects;

    const int baseSpeed = 140;
    const float experienceProbability = 0.3f;
    const float criticalDamageSizeMultiplier = 2f;
    const float slowMass = 1000f;

    float currentHealth;
    float speedModifier = 1f;

    Transform myTransform, playerTransform;
    Rigidbody2D myRigidbody;
    Pool enemyPool, experiencePool, damagePool;

    public Parasite parasite;
    public Chill chill;

    private void Awake() {
        objects = GameObject.Find("RunManager").GetComponent<ObjectManager>();

        myTransform = transform;
        myRigidbody = GetComponent<Rigidbody2D>();
        playerTransform = objects.player.transform;
        enemyPool = objects.enemyPool.GetComponent<Pool>();
        experiencePool = objects.experiencePool.GetComponent<Pool>();
        damagePool = objects.damagePool.GetComponent<Pool>();

        parasite = GetComponentInChildren<Parasite>();
        chill = GetComponentInChildren<Chill>();
    }

    private void Start() {
        Reset();
    }

    private void FixedUpdate() {
        Move();
    }

    // move towards the player at a constant speed
    private void Move() {
        myRigidbody.velocity = speedModifier * baseSpeed * Time.deltaTime * (playerTransform.position - myTransform.position).normalized;
    }


    public void Damage(float damage, Color color, bool isCritical = false) {

        currentHealth -= damage;

        // display damage
        GameObject damageUI = damagePool.GetPooledObject();
        if (damageUI != null)
            damageUI.GetComponent<Damage>().SetDamage(damage, myTransform.position, color, isCritical ? criticalDamageSizeMultiplier : 1f);


        if (currentHealth <= 0)
            Kill(true);
    }

    // called when health <= 0
    private void Kill(bool canDropExperience) {
        if (canDropExperience && Random.value < experienceProbability) {
            GameObject experience = experiencePool.GetPooledObject();
            if (experience != null)
                experience.transform.position = myTransform.position;
        }
        KillParasite();
        enemyPool.ReturnPooledObject(gameObject);

    }

    // returns the damage inflicted by this enemy on the player
    public float GetDamage() {
        return damage;
    }

    public void Reset() {
        currentHealth = health;
        KillParasite();
        RemoveChill();
    }

    public bool HasParasite() {
        return parasite.gameObject.activeInHierarchy;
    }

    public bool HasChill() {
        return chill.gameObject.activeInHierarchy;
    }

    void KillParasite() {
        parasite.Kill();
        parasite.gameObject.SetActive(false);
    }

    void RemoveChill() {
        chill.Kill();
        chill.gameObject.SetActive(false);
    }

    public void SetSpeed(float speedModifier) {
        this.speedModifier = speedModifier;
        if (speedModifier <= 1)
            myRigidbody.mass = slowMass;
        else myRigidbody.mass = 1f;
    }

}
