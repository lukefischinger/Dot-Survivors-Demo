using UnityEngine;

// sets basic projectile properties
// extended by ProjectileCircle
public class Projectile : MonoBehaviour {

    [SerializeField] protected float timeToDestroy;
    ObjectManager objects;
    Pool explosionPool;

    protected float timeElapsed;

    protected Rigidbody2D myRigidbody;
    protected Transform myTransform;

    private void Awake() {
        myRigidbody = GetComponent<Rigidbody2D>();
        OtherAwake();
        objects = GameObject.Find("RunManager").GetComponent<ObjectManager>();
        explosionPool = objects.explosionPool.GetComponent<Pool>();
    }

    protected void UpdateTimeToDisable() {
        timeElapsed += Time.fixedDeltaTime;
        if (timeElapsed > timeToDestroy) {
            Kill();
        }
    }

    // scale is reduced for explosions relative to standard circular projectiles
    public void Reset(Vector3 position, float scale = 1f) {
        timeElapsed = 0;
        transform.position = position;
        transform.localScale = scale * Vector3.one;
        OtherReset();
    }

    public void Kill() {
        OtherKill();
        if (tag == "Explosion") {
            explosionPool.ReturnPooledObject(gameObject);
        }
        else gameObject.SetActive(false);
    }

    protected virtual void OtherAwake() { }

    protected virtual void OtherReset() { }

    protected virtual void OtherKill() { }
}

