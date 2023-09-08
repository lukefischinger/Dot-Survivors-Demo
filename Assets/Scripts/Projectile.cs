using UnityEngine;

// sets basic projectile properties
// extended by ProjectileCircle
public class Projectile : MonoBehaviour
{

    [SerializeField] protected float timeToDestroy;

    protected float speed;
    protected Vector2 initialDirection;

    float timeInstantiated;
    protected float timeElapsed;
    
    protected Rigidbody2D myRigidbody;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        timeInstantiated = Time.time;

    }

    private void Update()
    {
        timeElapsed = Time.time - timeInstantiated;
        if(timeElapsed > timeToDestroy)
        {
            Destroy(gameObject);
        }

    }

    public void SetProperties(float speed, Vector3 position, Vector2 direction)
    {
        transform.position = position;
        initialDirection = direction;
        this.speed = speed;
    }


}

