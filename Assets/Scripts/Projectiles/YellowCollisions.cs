using UnityEngine;

public class YellowCollisions : MonoBehaviour
{
    // upgradeable values
    int spreadNumber;
    float damageMultiplier;
    float tickLength;
    float duration;

    public void Hit(Enemy enemy) {
        if(enemy.parasite.gameObject.activeInHierarchy) {
            return;
        }

        enemy.parasite.gameObject.SetActive(true);
        enemy.parasite.SetValues(spreadNumber - 1, damageMultiplier, tickLength, duration, 0f);

    }

    public void SetValues(int spreadNumber, float damageMultiplier, float tickLength, float duration) {
        this.spreadNumber = spreadNumber;
        this.damageMultiplier = damageMultiplier;
        this.tickLength = tickLength;
        this.duration = duration;
    }
}
