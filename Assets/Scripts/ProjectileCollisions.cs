using UnityEngine;

public class ProjectileCollisions : MonoBehaviour {

    ObjectManager objects;
    Pool explosionPool;

    // set by weapon upon firing
    float hitCount;

    Enemy lastEnemy;
    Projectile projectile;

    bool isRedActive, isBlueActive, isYellowActive;
    int explosionChainNumber;

    bool isBasicActive = true;

    private void Awake() {
        objects = GameObject.Find("RunManager").GetComponent<ObjectManager>();
        explosionPool = objects.explosionPool.GetComponent<Pool>();

        projectile = GetComponent<Projectile>();
    }

    public void SetProperties(float hitCount, bool isBasicActive = true, bool isRedActive = true, bool isBlueActive = true, bool isYellowActive = true, int explosionChainNumber = 0) {
        // set basic properties
        this.hitCount = hitCount;
        this.isBasicActive = isBasicActive;

        this.isRedActive = isRedActive;
        this.isBlueActive = isBlueActive;
        this.isYellowActive = isYellowActive;

        this.explosionChainNumber = explosionChainNumber;

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
        if (isBasicActive && lastEnemy.gameObject.activeInHierarchy)
            lastEnemy.Damage(Weapon.GetDamage(), "White");

        // call Hit method in RedCollisions component
        if (isRedActive && lastEnemy.gameObject.activeInHierarchy)
            RedCollisions.Hit(lastEnemy, explosionPool, explosionChainNumber);

        // call Hit method in YellowCollisions component
        if (isYellowActive && lastEnemy.gameObject.activeInHierarchy)
            YellowCollisions.Hit(lastEnemy);

        // call Hit method in BlueCollisions component
        if (isBlueActive && lastEnemy.gameObject.activeInHierarchy)
            BlueCollisions.Hit(lastEnemy);

        if (hitCount <= 0) {
            projectile.Kill();
        }
    }
}
