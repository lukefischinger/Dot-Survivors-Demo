using UnityEngine;

// player component
// creates projectiles of a specified type at a specified interval
public class Weapon : MonoBehaviour
{ 
    float rate = 1.5f;
    float damage = 40;
    float speed;
    float hitCount = 100;
    
    [SerializeField] GameObject projectilePrefab;
    
    float timeLastFired;
    float damageMultiplier = 1f;
    
    void Update()
    {
        Fire();
    }

    
    // fire projectiles
    private void Fire()
    {
        if(Time.time > timeLastFired + rate)
        {
            Vector3 startPosition = transform.position;
            Projectile projectile = Instantiate(projectilePrefab).GetComponent<Projectile>();
            ProjectileCollisions projectileCollisions = projectile.GetComponent<ProjectileCollisions>();
            projectile.SetProperties(speed, startPosition, Vector2.up);
            projectileCollisions.SetProperties(hitCount);

            timeLastFired = Time.time;
        }
    }

    private float RandomDamageMultiplier()
    {
        return Random.Range(0.8f, 1.2f); 
    }

    // returns damage done by the player to an enemy
    public float GetDamage() {
        return damage * RandomDamageMultiplier() * damageMultiplier;
    }

    public void SetDamageMultiplier(float value) {
        damageMultiplier = value;
    }
}
