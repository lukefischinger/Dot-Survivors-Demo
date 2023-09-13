using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [SerializeField] float speed = 1000;

    public PlayerInput myPlayerInput;
    Vector2 moveInput;

    Rigidbody2D myRigidbody;
    Transform myTransform;

    float speedMultiplier = 1;

    void Awake() {
        myPlayerInput = new PlayerInput();
        myRigidbody = GetComponent<Rigidbody2D>();
        myTransform = GetComponent<Transform>();
    }

    void FixedUpdate() {
        UpdateInputs();
        Move();
    }


    void UpdateInputs() {
        moveInput = myPlayerInput.Player.Move.ReadValue<Vector2>();
    }

    void Move() {
        myRigidbody.velocity = speed * speedMultiplier * moveInput * Time.deltaTime;

        // set player rotation only when moveInput changes to something nonzero
        if (moveInput != Vector2.zero) {
            myTransform.rotation = Quaternion.Euler(0, 0, -Mathf.Sign(moveInput.x) * Mathf.Rad2Deg * Mathf.Acos(Vector2.Dot(moveInput, Vector2.up)));

        }
    }

    public void SetSpeedMultiplier(float value) {
        speedMultiplier = value;
    }


    private void OnEnable() {
        myPlayerInput.Enable();
    }

    private void OnDisable() {
        myPlayerInput.Disable();
    }


}