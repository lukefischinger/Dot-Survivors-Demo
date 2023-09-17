using UnityEngine;


public class Experience : MonoBehaviour {

    ObjectManager objects;
    GameObject player;
    Pool experiencePool;

    Rigidbody2D myRigidbody;
    public int experienceAmount = 1;
    float movementMultiplier;

    void Awake() {
        objects = GameObject.Find("RunManager").GetComponent<ObjectManager>();

        myRigidbody = GetComponent<Rigidbody2D>();
        movementMultiplier = Random.Range(0.2f, 2f);

        player = objects.player;
        experiencePool = objects.experiencePool.GetComponent<Pool>();
    }

    void FixedUpdate() {
        if (IsPlayerClose()) {
            float magnitude = movementMultiplier * Mathf.Min(5, Mathf.Max(2.5f, (player.transform.position - transform.position).magnitude));
            myRigidbody.velocity = (player.transform.position - transform.position).normalized * magnitude * Time.deltaTime * 500f;
        }
    }

    public void SetProperties(int value) {
        experienceAmount = value;
    }

    bool IsPlayerClose() {
        return ((player.transform.position - transform.position).magnitude < 10);
    }


    private void OnTriggerEnter2D(Collider2D collision) {

        if (collision.gameObject.tag == "Player") {
            player.GetComponent<ExperienceManager>().AddExperience(experienceAmount);
            experiencePool.ReturnPooledObject(gameObject);
        }
    }
}
