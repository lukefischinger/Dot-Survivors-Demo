using UnityEngine;

public class Enemy : MonoBehaviour
{

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

    private void Move() {
        myRigidbody.velocity = baseSpeed * Time.deltaTime * (playerTransform.position - myTransform.position).normalized;
    }
    
}
