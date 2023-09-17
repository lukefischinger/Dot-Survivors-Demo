using UnityEngine;

public class ProjectileCircle : Projectile {

    CircleCollider2D myCollider;

    float maxRadius = 8f;
    float timeToMax = 0.25f;
    float timeOffset = 0.05f;

    protected override void OtherAwake() {
        transform.localScale = Vector3.one;
        myCollider = GetComponent<CircleCollider2D>();
    }


    private void FixedUpdate() {
        Move();
    }


    void Move() {
        if (timeElapsed - timeOffset < timeToMax) {
            myCollider.radius = maxRadius * (timeElapsed - timeOffset) / timeToMax;
        }
        else myCollider.radius = maxRadius;
        
    }
}
