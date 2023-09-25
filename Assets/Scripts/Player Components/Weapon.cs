using System.Collections.Generic;
using UnityEngine;

// player component
// creates projectiles of a specified type at a specified interval
public class Weapon : MonoBehaviour {

    [SerializeField] GameObject projectilePrefab;

    ProjectileCircle projectile;
    ProjectileCollisions projectileCollisions;
    static float timeLastFired;
    static float damageMultiplier = 1f;

    // constants
    public const float criticalMultiplier = 2f;
    public const float damage = 10;
    public const int chillsPerTrigger = 10;
    public static float explosionChance = 0.15f;

    // basic upgradeable weapon attributes
    public static float rate = 1.5f;
    public static float hitCount = 100;

    // upgradeable red values
    public static bool isRedActive = false;
    public static float redCriticalChance;
    public static float redDamageMultiplier;
    public static float redExplosionSize;
    public static int redChainNumber = 0;

    // upgradeable yellow values
    public static bool isYellowActive = false;
    public static int yellowSpreadNumber;
    public static float yellowDamageMultiplier;
    public static float yellowTickLength;
    public static float yellowDuration;

    // upgradeable blue values
    public static bool isBlueActive = false;
    public static float blueDamage;
    public static float blueSpeedModifier;
    public static float blueDuration;
    public static float blueDamageDelay;
    public static bool isBlueCountTrigger;

    public static int blueCount = 0;
    public static int blueTriggers = 0;
    
    // upgradeable green values
    public static bool blueSpreadsWithYellow = false;
    public static bool yellowUsesBlueDuration = false;
    public static bool isBlueMultiHitActive = false;

    // upgradeable orange values
    public static bool yellowCanHitCritically = false;
    public static bool explosionsAddYellow = false;

    // upgradeable purple values
    public static bool explosionsAddBlue = false;
    public static bool redDamageTriggersBlue = false;
    public static bool blueDamageTriggersExplosion = false;

    private void Awake() {
        isYellowActive = false;
        isRedActive = false;

        projectile = Instantiate(projectilePrefab).GetComponent<ProjectileCircle>();
        projectileCollisions = projectile.GetComponent<ProjectileCollisions>();
    }

    void FixedUpdate() {
        blueTriggers = blueCount / chillsPerTrigger;
        blueCount -= blueTriggers * chillsPerTrigger;
    }

    private void Update() {
        Fire();
    }

    // fire projectiles at set interval
    private void Fire() {
        if (Time.time > timeLastFired + rate) {
            Vector3 startPosition = transform.position;

            projectile.Reset(startPosition);
            projectileCollisions.SetProperties(hitCount, true, isRedActive, isBlueActive, isYellowActive, redChainNumber);
            projectile.gameObject.SetActive(true);

            timeLastFired = Time.time;
        }
    }

    public static float RandomDamageMultiplier() {
        return Random.Range(0.8f, 1.2f) * damageMultiplier;
    }

    // returns damage done by the player to an enemy
    public static float GetDamage() {
        return damage * RandomDamageMultiplier();
    }

    public static void SetDamageMultiplier(float value) {
        damageMultiplier = value;
    }



}
