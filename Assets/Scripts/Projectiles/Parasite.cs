using UnityEngine;

// Added to "host" enemies as children when they are hit by Yellow projectiles
// Inflicts damage over time on the host enemy, and can add additional parasites to nearby enemies
public class Parasite : MonoBehaviour {

    ObjectManager objects;
    Pool parasitePool;
    Weapon weapon;

    SpriteRenderer mySpriteRenderer;
    CircleCollider2D myCollider;
    Transform host, myTransform;
    Enemy hostEnemy;
    

    int spreadNumber;
    float damageMultiplier;
    float tickLength;
    float duration;
    float delay;

    float timeElapsed;
    float tickRemaining;

    const float damageRatioPerTick = 0.1f;

    private void Awake() {
        myTransform = transform;
        objects = GameObject.Find("RunManager").GetComponent<ObjectManager>();
        parasitePool = objects.parasitePool.GetComponent<Pool>();
        weapon = objects.player.GetComponent<Weapon>();
        myCollider = GetComponent<CircleCollider2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Attach(Transform hostTransform) {
        host = hostTransform;
        myTransform.SetParent(host);
        myTransform.localPosition = Vector3.zero;
        hostEnemy = host.GetComponent<Enemy>();
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
    }

    private void FixedUpdate() {
        if(delay > 0) {
            delay -= Time.deltaTime;
            return;
        }

        mySpriteRenderer.enabled = true;
        myCollider.enabled = true;
        UpdateTimeToDestroy();
        UpdateTick();
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
            hostEnemy.Damage(damageMultiplier * damageRatioPerTick * weapon.GetDamage(), objects.parasiteDamageColor);
            tickRemaining += tickLength;
        }
        else {
            tickRemaining -= Time.deltaTime;
        }
    }

    public void Kill() {
        myTransform.SetParent(parasitePool.transform);
        hostEnemy = null;
        parasitePool.ReturnPooledObject(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        ProcessEnemyCollision(collision);
    }

    private void OnTriggerStay2D(Collider2D collision) {
        ProcessEnemyCollision(collision);
    }

    void ProcessEnemyCollision(Collider2D collision) {
        if (spreadNumber == 0) {
            myCollider.enabled = false;
            return;
        }
        if (collision.gameObject.tag == "Enemy" && !collision.GetComponent<Enemy>().HasParasite()) {

            GameObject parasiteObj = parasitePool.GetPooledObject();
            if (parasiteObj == null)
                return;

            //spreadNumber--;
            Parasite parasite = parasiteObj.GetComponent<Parasite>();
            parasite.Attach(collision.transform);
            parasite.SetValues(spreadNumber - 1, damageMultiplier, tickLength, duration, tickLength);

            if (spreadNumber == 0) {
                myCollider.enabled = false;
            }
        }
    }

}
