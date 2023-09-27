using UnityEngine;

public class ProjectileCircle : Projectile {

    CircleCollider2D myCollider;

    float maxRadius = 8f;
    float timeToMax = 0.25f;
    float timeOffset = 0.05f;



    protected override void OtherAwake() {
        myCollider = GetComponent<CircleCollider2D>();
        transform.localRotation = Quaternion.Euler(0, 0, Mathf.Round(Random.Range(0, 360) / 90) * 90);
        GetComponent<Animator>().enabled = false;
        GetComponent<Animator>().enabled = true;


    }

    protected override void OtherReset() {
        myCollider.radius = 0;
    }

    protected override void OtherKill() {
    }

    private void FixedUpdate() {
        Move();
        UpdateTimeToDisable();
    }


    void Move() {
        if (timeElapsed - timeOffset < timeToMax) {
            myCollider.radius = maxRadius * (timeElapsed - timeOffset) / timeToMax;
        }
        else myCollider.radius = maxRadius;
        
    }
}
