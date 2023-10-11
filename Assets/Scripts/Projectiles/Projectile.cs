using UnityEngine;

// sets basic projectile properties
// extended by ProjectileCircle
public class Projectile : MonoBehaviour {

    [SerializeField] protected float timeToDestroy;
    ObjectManager objects;
    AudioManager audioManager;
    Pool explosionPool;
    AudioSource audioSource;

    protected float timeElapsed;

    protected Rigidbody2D myRigidbody;
    protected Transform myTransform;

    private void Awake() {
        myRigidbody = GetComponent<Rigidbody2D>();
        OtherAwake();
        objects = GameObject.Find("RunManager").GetComponent<ObjectManager>();
        audioManager = objects.GetComponent<AudioManager>();
        explosionPool = objects.explosionPool.GetComponent<Pool>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        audioSource.volume = audioManager.soundVolume * AudioManager.explosionVolume;
    }

    protected void UpdateTimeToDisable() {
        timeElapsed += Time.fixedDeltaTime;
        if (timeElapsed > timeToDestroy) {
            Kill();
        }
    }

   

    public void ResetProjectile(Vector3 position, float scale = 1f) {
        timeElapsed = 0;
        transform.position = position;
        transform.localScale = scale * Vector3.one;
        OtherReset();

        if (tag == "Explosion") {
            PlayExplosionSound();
        }
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

    void PlayExplosionSound() {
        if (audioManager.canHearExplosion) {
            audioSource.Play();
            audioManager.ResetExplosionTimer();
        }
        else {
            audioSource.Stop();
        }
    }
}

