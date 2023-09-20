using UnityEngine;

public class YellowCollisions : MonoBehaviour
{

    ObjectManager objects;
    Pool parasitePool;

    // upgradeable values
    int spreadNumber;
    float damageMultiplier;
    float tickLength;
    float duration;

    private void Awake() {
        objects = GameObject.Find("RunManager").GetComponent<ObjectManager>();
        parasitePool = objects.parasitePool.GetComponent<Pool>();
    }

    public void Hit(Enemy enemy) {
        GameObject parasiteObj = parasitePool.GetPooledObject();
        if(parasiteObj == null)
            return;

        Parasite parasite = parasiteObj.GetComponent<Parasite>();
        parasite.Attach(enemy.transform);
        parasite.SetValues(spreadNumber - 1, damageMultiplier, tickLength, duration, 0f);

    }

    public void SetValues(int spreadNumber, float damageMultiplier, float tickLength, float duration) {
        this.spreadNumber = spreadNumber;
        this.damageMultiplier = damageMultiplier;
        this.tickLength = tickLength;
        this.duration = duration;
    }
}
