using UnityEngine;

public class ProjectileCollisions : MonoBehaviour {

    ObjectManager objects;

    // set by weapon upon firing
    float hitCount;

    protected float coolDownRemaining = 0;
    protected float coolDown = 0.4f;

    Weapon weapon;
    Enemy lastEnemy;
    Projectile projectile;
    
    RedCollisions red;
    YellowCollisions yellow;

    bool isBasicActive = true;

    private void Awake() {
        objects = GameObject.Find("RunManager").GetComponent<ObjectManager>();

        coolDownRemaining = 0;
        weapon = objects.player.GetComponent<Weapon>();
        projectile = GetComponent<Projectile>();
        red = GetComponent<RedCollisions>();
        yellow = GetComponent<YellowCollisions>();
    }


    private void FixedUpdate() {
        coolDownRemaining -= Time.deltaTime;
    }


    public void SetProperties(float hitCount, bool isBasicActive = true) {
        this.hitCount = hitCount;
        
        // set red properties if active
        if(red != null && weapon.isRedActive) {
            red.SetValues(weapon.activeRedDamageMultiplier, weapon.activeRedCriticalChance, weapon.activeRedExplosionSize, weapon.activeRedChainNumber);
        }

        if(yellow != null && weapon.isYellowActive) {
            yellow.SetValues(weapon.activeYellowSpreadNumber, weapon.activeYellowDamageMultiplier, weapon.activeYellowTickLength, weapon.activeYellowDuration);
        }

        this.isBasicActive = isBasicActive;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        HitEnemy(collision);
    }

    protected void HitEnemy(Collider2D collision) {
        if (hitCount <= 0 || collision.gameObject.tag != "Enemy") {
            return;
        }

        coolDownRemaining = coolDown;
        hitCount--;

        // apply base damage to the collided enemy
        lastEnemy = collision.gameObject.GetComponent<Enemy>();
        if(isBasicActive && lastEnemy.gameObject.activeInHierarchy)
            lastEnemy.Damage(weapon.GetDamage(), objects.basicDamageColor);

        // call Hit method in RedCollisions component
        if(red != null && weapon.isRedActive && lastEnemy.gameObject.activeInHierarchy)
            red.Hit(lastEnemy);
        
        // call Hit method in YellowCollisions component, if no parasite present
        if(yellow != null && weapon.isYellowActive && lastEnemy.gameObject.activeInHierarchy && !lastEnemy.HasParasite())
            yellow.Hit(lastEnemy);

        if (hitCount <= 0) {
            projectile.Kill();
        }

    }







}
