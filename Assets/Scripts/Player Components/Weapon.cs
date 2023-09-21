using System.Collections.Generic;
using UnityEngine;

// player component
// creates projectiles of a specified type at a specified interval
public class Weapon : MonoBehaviour {
    float damage = 10;

    [SerializeField] GameObject projectilePrefab;

    ProjectileCircle projectile;
    ProjectileCollisions projectileCollisions;
    float timeLastFired;
    float damageMultiplier = 1f;

    // basic upgradeable weapon attributes
    public float rate = 1.5f;
    public float hitCount = 100;

    // upgradeable red values
    public bool isRedActive = false;
    public float redCriticalChance;
    public float redDamageMultiplier;
    public float redExplosionSize;
    public int redChainNumber = 0;

    // upgradeable yellow values
    public bool isYellowActive = false;
    public int yellowSpreadNumber;
    public float yellowDamageMultiplier;
    public float yellowTickLength;
    public float yellowDuration;

    // upgradeable blue values
    public bool isBlueActive = false;
    public float blueDamage;
    public float blueSpeedModifier;
    public float blueDuration;
    public float blueDamageDelay;
    public bool isBlueCountTrigger;

    public int blueCount = 0;
    public int blueTriggers = 0;

    const int chillsPerTrigger = 10;

    private void Awake() {
        isYellowActive = false;
        isRedActive = false;

        projectile = Instantiate(projectilePrefab).GetComponent<ProjectileCircle>();
        projectileCollisions = projectile.GetComponent<ProjectileCollisions>();
    }

    void FixedUpdate() {
        blueTriggers = blueCount / chillsPerTrigger;
        blueCount -= blueTriggers * 10;
    }

    private void Update() {
        Fire();
    }

    // fire projectiles at set interval
    private void Fire() {
        if (Time.time > timeLastFired + rate) {
            Vector3 startPosition = transform.position;

            projectile.Reset(startPosition);
            projectileCollisions.SetProperties(hitCount);
            projectile.gameObject.SetActive(true);

            timeLastFired = Time.time;
        }
    }

    public float RandomDamageMultiplier() {
        return Random.Range(0.8f, 1.2f) * damageMultiplier;
    }

    // returns damage done by the player to an enemy
    public float GetDamage() {
        return damage * RandomDamageMultiplier();
    }

    public void SetDamageMultiplier(float value) {
        damageMultiplier = value;
    }

    public void SetRateSpeedAndHitCount(List<float> attributes) {
        rate = attributes[0];
        hitCount = attributes[1];
    }

    


}
