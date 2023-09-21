using UnityEngine;

// lives as a child of each enemy (the "host")
// activated when the enemy is hit by a Yellow projectile
// Inflicts damage over time on the host enemy, and can add additional parasites to nearby enemies
public class Parasite : MonoBehaviour {

    ObjectManager objects;
    Weapon weapon;

    SpriteRenderer mySpriteRenderer;
    CircleCollider2D myCollider;
    Transform host, myTransform;
    Enemy hostEnemy;
    Animator myAnimator;

    int spreadNumber;
    float damageMultiplier;
    float tickLength;
    float duration;
    float delay;

    float timeElapsed;
    float tickRemaining;

    const float damageRatioPerTick = 0.1f;

    bool areComponentsEnabled = false;

    private void Awake() {
        myTransform = transform;
        host = myTransform.parent;
        hostEnemy = host.GetComponent<Enemy>();

        objects = GameObject.Find("RunManager").GetComponent<ObjectManager>();
        weapon = objects.player.GetComponent<Weapon>();
        myCollider = GetComponent<CircleCollider2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
    }


    private void Update() {
        UpdateDelay();
    }

    public void SetValues(int spreadNumber, float damageMultiplier, float tickLength, float duration, float delay) {
        this.spreadNumber = spreadNumber;
        this.damageMultiplier = damageMultiplier;
        this.tickLength = tickLength;
        this.duration = duration;
        this.delay = delay;

        timeElapsed = 0;
        myCollider.enabled = false;
        mySpriteRenderer.enabled = false;
        myAnimator.enabled = false;
        areComponentsEnabled = false;
    }

    private void FixedUpdate() {
        if(delay > 0) {
            delay -= Time.deltaTime;
            return;
        }

        

        UpdateTimeToDestroy();
        UpdateTick();
    }

    void UpdateDelay() {
        if (delay > 0) {
            delay -= Time.deltaTime;
        }
        else if (!areComponentsEnabled) {
            mySpriteRenderer.enabled = true;
            myCollider.enabled = true;
            myAnimator.enabled = true;

            float offset = (Time.time - Mathf.Floor(Time.time));
            myAnimator.Play("Enemy Effect", 0, offset);
            areComponentsEnabled = true;
        }
    }

    void UpdateTimeToDestroy() {
        timeElapsed += Time.fixedDeltaTime;
        if (timeElapsed > duration) {
            Kill();
        }
    }

    void UpdateTick() {
        if (hostEnemy == null) {
            Kill();
            return;
        }

        if (tickRemaining <= 0) {
            hostEnemy.Damage(damageMultiplier * damageRatioPerTick * weapon.GetDamage(), objects.yellowDamageColor);
            tickRemaining += tickLength;
        }
        else {
            tickRemaining -= Time.deltaTime;
        }
    }

    public void Kill() {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        ProcessEnemyCollision(collision);
    }

    private void OnTriggerStay2D(Collider2D collision) {
        ProcessEnemyCollision(collision);
    }

    // no need to check if the collision is with an enemy,
    // as the projectile layer only has physics interactions with the enemy layer
    void ProcessEnemyCollision(Collider2D collision) {
        if (spreadNumber == 0) {
            myCollider.enabled = false;
            return;
        }

        Parasite enemyParasite = collision.GetComponent<Enemy>().parasite;
        if (!enemyParasite.gameObject.activeInHierarchy) {
            enemyParasite.gameObject.SetActive(true);
            enemyParasite.SetValues(spreadNumber - 1, damageMultiplier, tickLength, duration, tickLength);

            if (spreadNumber == 0) {
                myCollider.enabled = false;
            }
        }
    }

}
