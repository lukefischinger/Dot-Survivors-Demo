using UnityEngine;

public class ProjectileCollisions : MonoBehaviour {

    ObjectManager objects;
    UIManager uiManager;
    Pool explosionPool;

    // set by weapon upon firing
    float hitCount;

    Enemy lastEnemy;
    Projectile projectile;
    SpriteRenderer mySpriteRenderer;

    bool isBasicActive = true,
        isRedActive = false, 
        isBlueActive = false, 
        isYellowActive = false;
    int explosionChainNumber;
    const float alpha = 0.6f;
   
    private void Awake() {
        objects = GameObject.Find("RunManager").GetComponent<ObjectManager>();
        uiManager = objects.GetComponent<UIManager>();
        explosionPool = objects.explosionPool.GetComponent<Pool>();

        mySpriteRenderer = GetComponent<SpriteRenderer>();
        projectile = GetComponent<Projectile>();
    }

    public void SetProperties(float hitCount, bool isBasicActive = true, bool isRedActive = true, bool isBlueActive = true, bool isYellowActive = true, int explosionChainNumber = 0) {
        this.hitCount = hitCount;
        this.isBasicActive = isBasicActive;

        this.isRedActive = isRedActive;
        this.isBlueActive = isBlueActive;
        this.isYellowActive = isYellowActive;
        SetSpriteColor();


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


    void SetSpriteColor() {
        if (!isBasicActive) return; // if not the main projectile, don't change the color

        int colorVal = (isRedActive ? 2 : 1) *
                       (isBlueActive ? 3 : 1) *
                       (isYellowActive ? 5 : 1);
        string colorName;

        switch(colorVal) {
            case 1: return;
            case 2: colorName = "Red"; break;
            case 3: colorName = "Blue"; break;
            case 5: colorName = "Yellow"; break;
            case 6: colorName = "Purple"; break;
            case 10: colorName = "Orange"; break;
            case 15: colorName = "Green"; break;
            default: return;
        }

        Color color = uiManager.colors[uiManager.colorNames.IndexOf(colorName)];
        color.a = alpha;
        mySpriteRenderer.color = color;
    }
    
}
