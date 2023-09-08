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

        // set player rotation only when moveInput changes to something nonzero
        if (moveInput != Vector2.zero) {
            myTransform.rotation = Quaternion.Euler(0, 0, -Mathf.Sign(moveInput.x) * Mathf.Rad2Deg * Mathf.Acos(Vector2.Dot(moveInput, Vector2.up)));

        }
    }


    private void OnEnable()
    {
        myPlayerInput.Enable();
    }

    private void OnDisable()
    {
        myPlayerInput.Disable();
    }

 
}
