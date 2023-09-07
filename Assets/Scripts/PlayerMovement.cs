using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

  
    public PlayerInput myPlayerInput;
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Collider2D myCollider;
    [SerializeField] float speed = 1000;
    Transform myTransform;

    
    void Awake()
    {
        myPlayerInput = new PlayerInput();
        myRigidbody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        myTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }


    private void Update()
    {
        UpdateInputs();
    }


    void UpdateInputs()
    {
        moveInput = myPlayerInput.Player.Move.ReadValue<Vector2>();
    }

    void Move()
    {
        myRigidbody.velocity = speed * moveInput * Time.deltaTime;
        myTransform.up = myRigidbody.velocity;        
    }

   
    private void OnEnable()
    {
        myPlayerInput.Enable();
    }

    private void OnDisable()
    {
        myPlayerInput.Disable();
    }

    public void SetSpeed(float multiplier)
    {
        speed = multiplier;
    }

 
}
