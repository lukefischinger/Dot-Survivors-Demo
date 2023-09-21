using UnityEngine;

public class BlueCollisions : MonoBehaviour
{
    // upgradeable values
    float damage;
    float speedModifier;
    float duration;
    float damageDelay;
    bool isCountTrigger;

    public void Hit(Enemy enemy) { 
        if(enemy.chill.gameObject.activeInHierarchy) {
            return;
        }

        enemy.chill.gameObject.SetActive(true);
        enemy.chill.SetValues(damage, speedModifier, duration, damageDelay, isCountTrigger, true);
    }

    public void SetValues(float damage, float speedModifier, float duration, float damageDelay, bool isCountTrigger) {
        this.damage = damage;
        this.speedModifier = speedModifier;
        this.duration = duration;
        this.damageDelay = damageDelay;
        this.isCountTrigger = isCountTrigger;
    }
}
