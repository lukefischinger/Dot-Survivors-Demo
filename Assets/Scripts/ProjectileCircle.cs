using UnityEngine;

public class ProjectileCircle : Projectile {

    float expansionRate = 20f;

    private void FixedUpdate() {
        Move();
    }


    void Move() {
        transform.localScale = Vector3.one * expansionRate * timeElapsed;
    }
}
