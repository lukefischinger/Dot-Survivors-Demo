using UnityEngine;

public class ProjectileCollisions : MonoBehaviour {

    ObjectManager objects;

    // set by weapon upon firing
    float hitCount;

    Weapon weapon;
    Enemy lastEnemy;
    Projectile projectile;
    
    RedCollisions red;
    YellowCollisions yellow;
    BlueCollisions blue;

    bool isBasicActive = true;

    private void Awake() {
        objects = GameObject.Find("RunManager").GetComponent<ObjectManager>();

        weapon = objects.player.GetComponent<Weapon>();
        projectile = GetComponent<Projectile>();
        red = GetComponent<RedCollisions>();
        yellow = GetComponent<YellowCollisions>();
        blue = GetComponent<BlueCollisions>();
    }

    public void SetProperties(float hitCount, bool isBasicActive = true) {
        // set basic properties
        this.hitCount = hitCount;
        this.isBasicActive = isBasicActive;

        // set red properties if active
        if (red != null && weapon.isRedActive) {
            red.SetValues(weapon.redDamageMultiplier, weapon.redCriticalChance, weapon.redExplosionSize, weapon.redChainNumber);
        }

        // set yellow properties if active
        if(yellow != null && weapon.isYellowActive) {
            yellow.SetValues(weapon.yellowSpreadNumber, weapon.yellowDamageMultiplier, weapon.yellowTickLength, weapon.yellowDuration);
        }

        // set blue properties if active
        if(blue != null && weapon.isBlueActive) {
            blue.SetValues(weapon.blueDamage, weapon.blueSpeedModifier, weapon.blueDuration, weapon.blueDamageDelay, weapon.isBlueCountTrigger);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        HitEnemy(collision);
    }

    protected void HitEnemy(Collider2D collision) {
        if (hitCount <= 0 || collision.gameObject.tag != "Enemy") {
            return;
        }

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

        // call Hit method in BlueCollisions component, if no chill present
        if(blue != null && weapon.isBlueActive && lastEnemy.gameObject.activeInHierarchy && !lastEnemy.HasChill())
            blue.Hit(lastEnemy);

        if (hitCount <= 0) {
            projectile.Kill();
        }

    }







}
