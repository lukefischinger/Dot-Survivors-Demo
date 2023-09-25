using UnityEngine;

// lives as a child of each enemy (the "host")
// activated when the enemy is hit by a Yellow projectile
// Inflicts damage over time on the host enemy, and can add additional parasites to nearby enemies
public class Parasite : MonoBehaviour {


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

    const float damageRatioPerTick = 0.2f;

    bool areComponentsEnabled = false;

    private void Awake() {
        myTransform = transform;
        host = myTransform.parent;
        hostEnemy = host.GetComponent<Enemy>();

        myCollider = GetComponent<CircleCollider2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
    }


    private void Update() {
        UpdateDelay();
    }

    private void OnEnable() {
        timeElapsed = 0;
        myCollider.enabled = false;
        mySpriteRenderer.enabled = false;
        myAnimator.enabled = false;
        areComponentsEnabled = false;
    }

    public void SetValues(int spreadNumber, float damageMultiplier, float tickLength, float duration, float delay) {
        this.spreadNumber = spreadNumber;
        this.damageMultiplier = damageMultiplier;
        this.tickLength = tickLength;
        this.duration = duration;
        this.delay = delay;
    }

    public void Refresh(int spreadNumber, float damageMultiplier, float tickLength, float duration) {
        this.spreadNumber = Mathf.Max(spreadNumber, this.spreadNumber);
        this.damageMultiplier = Mathf.Max(damageMultiplier, this.damageMultiplier);
        this.tickLength = Mathf.Min(tickLength, this.tickLength);
        this.duration = Mathf.Max(duration, this.duration);

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

        float critical = Weapon.yellowCanHitCritically && (Random.value < Weapon.redCriticalChance) ? Weapon.criticalMultiplier : 1f;
        if (tickRemaining <= 0) {
            hostEnemy.Damage(critical * damageMultiplier * damageRatioPerTick * Weapon.GetDamage(), "Yellow", critical > 1f);
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
    public void ProcessEnemyCollision(Collider2D collision) {
        if (spreadNumber == 0) {
            myCollider.enabled = false;
            return;
        }
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if(enemy.HasParasite()) {
            enemy.parasite.Refresh(spreadNumber, damageMultiplier, tickLength, duration);
        } else {
            enemy.parasite.gameObject.SetActive(true);
            enemy.parasite.SetValues(spreadNumber, damageMultiplier, tickLength, duration, tickLength);
        }

        // spread blue if available
        if (Weapon.blueSpreadsWithYellow)
           hostEnemy.chill.SpreadChill(enemy);
            
    }

}
