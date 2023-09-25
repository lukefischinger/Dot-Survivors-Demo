using UnityEngine;

public class Enemy : MonoBehaviour {
    
    
    ObjectManager objects;

    const int baseSpeed = 140;
    const float experienceProbability = 0.3f;
    const float criticalDamageSizeMultiplier = 2f;
    const float slowMass = 1000f;

    float health;
    float damage;

    float speedModifier = 1f;

    Transform myTransform, playerTransform;
    Rigidbody2D myRigidbody;
    Pool enemyPool, experiencePool, damagePool;
    SpriteRenderer mySpriteRenderer;

    public Parasite parasite;
    public Chill chill;

    private void Awake() {
        objects = GameObject.Find("RunManager").GetComponent<ObjectManager>();

        myTransform = transform;
        myRigidbody = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        
        playerTransform = objects.player.transform;
        enemyPool = objects.enemyPool.GetComponent<Pool>();
        experiencePool = objects.experiencePool.GetComponent<Pool>();
        damagePool = objects.damagePool.GetComponent<Pool>();

        parasite = GetComponentInChildren<Parasite>();
        chill = GetComponentInChildren<Chill>();
    }

    private void FixedUpdate() {
        Move();
    }

    // move towards the player at a constant speed
    private void Move() {
        myRigidbody.velocity = speedModifier * baseSpeed * Time.deltaTime * (playerTransform.position - myTransform.position).normalized;
    }


    public void Damage(float damage, string colorName = "White", bool isCritical = false) {

        health -= damage;

        // display damage
        GameObject damageUI = damagePool.GetPooledObject();
        if (damageUI != null)
            damageUI.GetComponent<Damage>().SetDamage(damage, myTransform.position, colorName, isCritical ? criticalDamageSizeMultiplier : 1f);

        objects.runInformation.IncrementDamage(damage, colorName);

        if (health <= 0)
            Kill(true);
    }

    // called when health <= 0
    private void Kill(bool canDropExperience) {
        if (canDropExperience && Random.value < experienceProbability) {
            GameObject experience = experiencePool.GetPooledObject();
            if (experience != null)
                experience.transform.position = myTransform.position;
        }
        if (canDropExperience)
            objects.runInformation.enemiesKilled++;

        KillParasite();
        enemyPool.ReturnPooledObject(gameObject);

    }

    // returns the damage inflicted by this enemy on the player
    public float GetDamage() {
        return damage;
    }

    public void ResetEnemy(float health, float damage, Sprite sprite) {
        this.health = health;
        this.damage = damage;
        mySpriteRenderer.sprite = sprite;

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
