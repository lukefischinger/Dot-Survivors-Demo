using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] float damage;

    const int baseSpeed = 140;

    Transform myTransform, playerTransform;
    Rigidbody2D myRigidbody;

    private void Awake() {
        myTransform = transform;
        myRigidbody = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }


    private void FixedUpdate() {
        Move();
    }

    // move towards the player at a constant speed
    private void Move() {
        myRigidbody.velocity = baseSpeed * Time.deltaTime * (playerTransform.position - myTransform.position).normalized;
    }


    public void Damage(float damage) {
        health -= damage;
        if(health <= 0)
            Kill(); 
    }

    // called when health <= 0
    private void Kill() {
        Destroy(gameObject);
    }

    // returns the damage inflicted by this enemy on the player
    public float GetDamage() {
        return damage;
    }

}
