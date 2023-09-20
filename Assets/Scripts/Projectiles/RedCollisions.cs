using UnityEngine;

public class RedCollisions : MonoBehaviour {

    ObjectManager objects;
    Weapon weapon;
    Pool explosionPool;

    // upgradeable values
    float damageMultiplier;
    float criticalChance;
    int explosionChainNumber;
    float explosionSize;

    // constants
    const float explosionChance = 0.10f;
    const float criticalMultiplier = 2f;
    const float explosionMultiplier = 0.5f / 0.3f;

    private void Awake() {
        objects = GameObject.Find("RunManager").GetComponent<ObjectManager>();
        weapon = objects.player.GetComponent<Weapon>();
        explosionPool = objects.explosionPool.GetComponent<Pool>();
    }

    public void Hit(Enemy enemy) {
        if (explosionChainNumber > 0 && Random.value < explosionChance) {
            CreateExplosion(enemy);
        }

        float critical = (Random.value < criticalChance) ? criticalMultiplier : 1f;
        enemy.Damage(damageMultiplier * critical * weapon.GetDamage(), objects.redDamageColor, critical > 1f);
    }

    public void CreateExplosion(Enemy enemy) {
        GameObject explosion = explosionPool.GetPooledObject();
        if (explosion == null) {
            return;
        }

        explosion.GetComponent<ProjectileCircle>().Reset(enemy.transform.position, explosionSize);
        
        explosion.GetComponent<ProjectileCollisions>().SetProperties(1000, false);
        explosion.GetComponent<RedCollisions>().SetValues(damageMultiplier * explosionMultiplier, criticalChance, explosionSize, explosionChainNumber - 1);
    }

    public void SetValues(float damageMultiplier, float criticalChance, float explosionSize, int explosionChainNumber) {
        this.damageMultiplier = damageMultiplier;
        this.criticalChance = criticalChance;
        this.explosionSize = explosionSize;
        this.explosionChainNumber = explosionChainNumber;
    }

}



