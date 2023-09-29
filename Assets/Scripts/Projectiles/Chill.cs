using UnityEngine;

// lives as a child of each enemy (the "host")
// activated when the enemy is hit by a blue collision, chill is activated with the appropriate values
// chill slows enemies and inflicts damage after a few seconds' delay
public class Chill : MonoBehaviour {
    ObjectManager objects;
    Pool explosionPool;

    CircleCollider2D myCollider;
    Transform host, myTransform;
    Enemy hostEnemy;
    Animator myAnimator;
    bool animationStarted = false;


    // blue collision values
    float damage;
    float speedModifier;
    float damageDelay;
    bool isCountTrigger;

    const float multiHitTick = 0.1f;

    // timer variables
    float durationRemaining;
    float damageDelayRemaining;

    // variables related to how many times the chill can damage,
    // and how many hits (triggerCount) are subject to the shorter "multiHitTick" time interval instead of the "damageDelay" interval
    int triggerCount = 0;
    bool isMultiHitActive = false;
    bool canHit = false;


    private void Awake() {
        myTransform = transform;
        host = myTransform.parent;
        hostEnemy = host.GetComponent<Enemy>();

        objects = GameObject.Find("RunManager").GetComponent<ObjectManager>();
        explosionPool = objects.explosionPool.GetComponent<Pool>();

        myCollider = GetComponent<CircleCollider2D>();
        myAnimator = GetComponent<Animator>();

    }

    private void Update() {
        UpdateAnimation();
    }

    private void FixedUpdate() {
        UpdateDuration();
        UpdateDamage();
        UpdateTriggerCount();

    }

    // kill the chill if it is time; otherwise, update the remaining chill duration
    void UpdateDuration() {
        if (durationRemaining <= 0)
            Kill();
        else durationRemaining -= Time.deltaTime;
    }

    // inflict damage if it is time; otherwise, update the damage delay timer
    void UpdateDamage() {
        if (!canHit)
            return;


        if (damageDelayRemaining <= 0) {
            hostEnemy.Damage(damage * Weapon.RandomDamageMultiplier(), "Blue");
            if (Weapon.blueDamageTriggersExplosion && Random.value < Weapon.explosionChance)
                RedCollisions.CreateExplosion(hostEnemy, explosionPool);

            if (!isMultiHitActive) {
                canHit = false;
                return;
            }
            else if (triggerCount > 0) {
                damageDelayRemaining += multiHitTick;
                triggerCount--;
            }
            else {
                damageDelayRemaining += damageDelay;
            }
        }
        else {
            damageDelayRemaining -= Time.deltaTime;
        }

    }


    void UpdateAnimation() {
        if (!animationStarted) {
            myAnimator.enabled = true;
            float offset = (Time.time - Mathf.Floor(Time.time));
            myAnimator.Play("Chill", 0, offset);
            animationStarted = true;
        }
    }

    public void SetValues(float damage, float speedModifier, float duration, float damageDelay, bool isCountTrigger, bool isMultiHitActive) {
        this.damage = damage;
        this.speedModifier = speedModifier;
        this.damageDelay = damageDelay;
        this.isCountTrigger = isCountTrigger;
        this.isMultiHitActive = isMultiHitActive;

        SetHostSpeed();

        triggerCount = 0;
        durationRemaining = duration;
        damageDelayRemaining = damageDelay;
        canHit = damage > 0;

        Weapon.blueCount++;


        myAnimator.enabled = false;
        animationStarted = false;


    }

    public void Refresh(float damage, float speedModifier, float duration, float damageDelay, bool isCountTrigger, bool isMultiHitActive) {
        this.damage = Mathf.Max(this.damage, damage);
        this.speedModifier = Mathf.Max(this.speedModifier, speedModifier);
        this.damageDelay = Mathf.Min(this.damageDelay, damageDelay);
        this.isCountTrigger = isCountTrigger || this.isCountTrigger;
        this.isMultiHitActive = isMultiHitActive || this.isMultiHitActive;
        
        canHit = damage > 0;
        durationRemaining = Mathf.Max(durationRemaining, duration);
        if(damageDelayRemaining < 0)
            damageDelayRemaining = damageDelay;
        Weapon.blueCount++;
    }

    void SetHostSpeed() {
        hostEnemy.SetSpeed(speedModifier);
    }

    // deactivate & reset host speed to normal
    public void Kill() {
        hostEnemy.SetSpeed(1f);
        gameObject.SetActive(false);
    }

    void UpdateTriggerCount() {
        if (isCountTrigger)
            triggerCount += Weapon.blueTriggers;
        // if trigger comes into effect, reduce the damage delay remaining
        if (triggerCount > 0 && damageDelayRemaining > multiHitTick) {
            damageDelayRemaining = multiHitTick - (damageDelay - damageDelayRemaining); ;
        }
    }

    public void SpreadChill(Enemy enemy) {
        if (durationRemaining < 0.5f)
            return;

        if (enemy.HasChill()) {
            enemy.chill.Refresh(damage, speedModifier, durationRemaining, damageDelay, isCountTrigger, isMultiHitActive);
            return;
        }

        enemy.chill.gameObject.SetActive(true);
        enemy.chill.SetValues(damage, speedModifier, durationRemaining, damageDelay, isCountTrigger, isMultiHitActive);
    }

    public void AddTrigger() {
        triggerCount++;
    }

}
