using UnityEngine;

public class Enemy : MonoBehaviour {


    ObjectManager objects;

    const int baseSpeed = 2;
    const float experienceProbability = 0.3f;
    const float criticalDamageSizeMultiplier = 2f;
    const float slowMass = 1000f;
    const float offscreenTimeToReset = 8f;

    // variables set by the enemy generator upon spawn
    float health;
    float damage;
    bool elite;
    int experienceAmount;

    float difficultySpeedMultiplier = 1f;
    float speedModifier = 1f;
    float randomSpeedMultiplier;
    float offscreenTimeRemaining = 8f;

    Transform myTransform, playerTransform;
    Rigidbody2D myRigidbody;
    Pool enemyPool, experiencePool, damagePool;
    SpriteRenderer mySpriteRenderer;
    EnemyGenerator enemyGenerator;

    public Parasite parasite;
    public Chill chill;

    private void Awake() {
        objects = GameObject.Find("RunManager").GetComponent<ObjectManager>();
        enemyGenerator = objects.enemyGenerator.GetComponent<EnemyGenerator>();

        myTransform = transform;
        myRigidbody = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();

        playerTransform = objects.player.transform;
        enemyPool = objects.enemyPool.GetComponent<Pool>();
        experiencePool = objects.experiencePool.GetComponent<Pool>();
        damagePool = objects.damagePool.GetComponent<Pool>();

        parasite = GetComponentInChildren<Parasite>();
        chill = GetComponentInChildren<Chill>();

        randomSpeedMultiplier = 0.8f + Random.value * 0.4f; // randomize enemy movement speed up/down wtihin 20% to avoid gridlock
    }

   
    private void Update() {
        UpdateOffScreen();
        Move();
    }

    // move towards the player at a constant speed
    private void Move() {
        Vector2 playerDifference = playerTransform.position - myTransform.position;
        if (playerDifference.magnitude < 0.2f)
            myRigidbody.velocity = Vector2.zero;
        else myRigidbody.velocity = difficultySpeedMultiplier * speedModifier * baseSpeed * randomSpeedMultiplier * playerDifference.normalized;
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
            if (experience != null) {
                experience.transform.position = myTransform.position;
                experience.GetComponent<Experience>().SetProperties(experienceAmount);
            }
        }
        if (canDropExperience)
            objects.runInformation.enemiesKilled++;

        if (elite)
            SetAllExperienceToDisregardDistance();

        enemyPool.ReturnPooledObject(gameObject);

    }

    // returns the damage inflicted by this enemy on the player
    public float GetDamage() {
        return damage;
    }

    public void ResetEnemy(float health, float damage, Sprite sprite, bool elite = false, int experienceAmount = 1, float difficultySpeedMultiplier = 1) {
        this.health = health;
        this.damage = damage;
        mySpriteRenderer.sprite = sprite;
        this.elite = elite;
        this.experienceAmount = experienceAmount;
        this.difficultySpeedMultiplier = difficultySpeedMultiplier;

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

    void SetAllExperienceToDisregardDistance() {
        for (int i = 0; i < experiencePool.pool.Count; i++) {
            Experience exp = experiencePool.pool[i].GetComponent<Experience>();
            if (exp.gameObject.activeInHierarchy) {
                exp.disregardDistance = true;
            }
        }
    }

    // move the enemy back to the edge of the screen if it lingers too long offscreen
    void UpdateOffScreen() {
        if (mySpriteRenderer.isVisible) {
            offscreenTimeRemaining = offscreenTimeToReset;
            return;
        }

        if (offscreenTimeRemaining <= 0)
            myTransform.position = enemyGenerator.GetRandomSpawnLocation();
        else offscreenTimeRemaining -= Time.deltaTime;
    }

}
