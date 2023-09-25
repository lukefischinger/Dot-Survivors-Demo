using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [SerializeField] float speed = 1000;

    ObjectManager objects;
    GameObject joystickAnchorObject;
    Transform joystick, joystickAnchor;
    Camera cam;
    ButtonSelect pauseButton;

    public PlayerInput myPlayerInput;
    Vector2 moveInput;
    Vector2 clickPoint;

    Rigidbody2D myRigidbody;
    Transform myTransform;

    float speedMultiplier = 1;

    void Awake() {
        myPlayerInput = new PlayerInput();
        myRigidbody = GetComponent<Rigidbody2D>();
        myTransform = GetComponent<Transform>();

        objects = GameObject.Find("RunManager").GetComponent<ObjectManager>();
        joystickAnchorObject = objects.movementJoystick;
        joystickAnchor = joystickAnchorObject.transform;
        joystick = joystickAnchor.GetChild(0);
        joystick.SetParent(joystickAnchor, false);
        cam = Camera.main;
        pauseButton = objects.pauseButton.GetComponent<ButtonSelect>();
    }

    void Update() {
        UpdateInputs();
    }


    private void FixedUpdate() {
        Move();
    }


    void UpdateInputs() {
        // keyboard input
        moveInput = myPlayerInput.Player.Move.ReadValue<Vector2>();

        // use mouse or touch based inputs if currently pressed
        UpdateJoystickInputs();
    }

    void UpdateJoystickInputs() {
        if (myPlayerInput.Player.Click.ReadValue<float>() == 0) {
            joystickAnchor.gameObject.SetActive(false);
            return;
        }




        // update the joystick anchor location
        if (myPlayerInput.Player.Click.triggered) {
            if (pauseButton.selected) // the joystick anchor can't be set over the pause button
                return;

            clickPoint = myPlayerInput.Player.MousePosition.ReadValue<Vector2>();
            clickPoint = cam.ScreenToWorldPoint(clickPoint);
            joystickAnchor.position = clickPoint;
        }

        joystickAnchor.gameObject.SetActive(true);


        // update the joystick direction
        moveInput = cam.ScreenToWorldPoint(myPlayerInput.Player.MousePosition.ReadValue<Vector2>()) - cam.transform.position - joystickAnchor.localPosition;
        moveInput = EightWayDirection(moveInput);
        if (moveInput.magnitude < 0.2f)
            moveInput = Vector2.zero;
        joystick.transform.localPosition = moveInput.magnitude > 0.5f ? moveInput.normalized * 0.5f : moveInput;
        moveInput.Normalize();
    }

    // snaps the input Vector2 to the nearest 8-way direction. e.g. (1,1), (0.707, 0.707), etc.
    Vector2 EightWayDirection(Vector2 direction) {
        float angle = Vector2.SignedAngle(Vector2.up, direction);
        angle = Mathf.Round(angle / 45f) * 45f;
        return Quaternion.Euler(0, 0, angle) * Vector2.up * direction.magnitude;
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
