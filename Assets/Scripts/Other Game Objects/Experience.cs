using UnityEngine;


public class Experience : MonoBehaviour {

    ObjectManager objects;
    GameObject player;

    Rigidbody2D myRigidbody;
    public int experienceAmount = 20;
    float movementMultiplier;

    public bool disregardDistance = false;

    void Awake() {
        objects = GameObject.Find("RunManager").GetComponent<ObjectManager>();

        myRigidbody = GetComponent<Rigidbody2D>();
        movementMultiplier = Random.Range(0.2f, 2f);

        player = objects.player;
    }

    void FixedUpdate() {
        if (IsPlayerClose()) {
            float magnitude = movementMultiplier * Mathf.Min(5, Mathf.Max(2.5f, (player.transform.position - transform.position).magnitude));
            myRigidbody.velocity = (player.transform.position - transform.position).normalized * magnitude * Time.deltaTime * 500f;
        } else {
            myRigidbody.velocity = Vector3.zero;
        }
    }

    public void SetProperties(int value) {
        experienceAmount = value;
        disregardDistance = false;
    }

    bool IsPlayerClose() {
        return disregardDistance || ((player.transform.position - transform.position).magnitude < 10);
    }
}
