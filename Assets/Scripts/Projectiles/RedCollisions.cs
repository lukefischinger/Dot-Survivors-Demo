using UnityEngine;

public class RedCollisions {

    public static void Hit(Enemy enemy, Pool explosionPool, int explosionChainNumber = 0) {
        if (explosionChainNumber > 0 && Random.value < Weapon.explosionChance) {
            CreateExplosion(enemy, explosionPool, explosionChainNumber - 1);
        }

        float critical = CriticalMultiplier();
        enemy.Damage(
            Weapon.redDamageMultiplier * critical * Weapon.GetDamage(), 
            "Red", 
            critical > 1f
        );
        
        if (Weapon.redDamageTriggersBlue && enemy.HasChill()) {
            enemy.chill.AddTrigger();
        }
    }

    public static void CreateExplosion(Enemy enemy, Pool explosionPool, int explosionChainNumber = 0) {
        GameObject explosion = explosionPool.GetPooledObject();
        if (explosion == null) {
            return;
        }

        explosion.GetComponent<ProjectileCircle>().Reset(enemy.transform.position, Weapon.redExplosionSize);
        explosion.GetComponent<ProjectileCollisions>().SetProperties(
            1000,                           // hit count (effectively unlimited for explosions)
            false,                          // basic active
            true,                           // red active
            Weapon.explosionsAddYellow,     // yellow active
            Weapon.explosionsAddBlue,        // blue active
            explosionChainNumber
        );

    }

    public static float CriticalMultiplier() {
        return (Random.value < Weapon.redCriticalChance) ? Weapon.criticalMultiplier : 1f;
    }
}



