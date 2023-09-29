using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [SerializeField] List<Sprite> sprites;

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
    SpriteRenderer mySpriteRenderer;
    Bar healthBar;

    const float speed = 7.5f;

    float speedMultiplier = 1;
    public bool usingJoystick;
    float joystickCountdown = 5f;
    float joystickCountdownRemaining;


    void Awake() {
        myPlayerInput = new PlayerInput();
        myRigidbody = GetComponent<Rigidbody2D>();
        myTransform = GetComponent<Transform>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        healthBar = GetComponentInChildren<Bar>();

        objects = GameObject.Find("RunManager").GetComponent<ObjectManager>();
        joystickAnchorObject = objects.movementJoystick;
        joystickAnchor = joystickAnchorObject.transform;
        joystick = joystickAnchor.GetChild(0);
        joystick.SetParent(joystickAnchor, false);
        cam = Camera.main;
        pauseButton = objects.pauseButton.GetComponent<ButtonSelect>();
        usingJoystick = false;
    }

    void Update() {
        UpdateInputs();
        Move();
        SetSpriteRenderer();
    }

    void UpdateInputs() {
        // keyboard input
        moveInput = myPlayerInput.Player.Move.ReadValue<Vector2>();

        // use mouse or touch based inputs if currently pressed
        UpdateJoystickInputs();

        // set usingJoystick to false if no touch/click within the last [joystickCountdown] seconds
        joystickCountdownRemaining -= Time.deltaTime;
        usingJoystick = joystickCountdownRemaining > 0;
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
        joystickCountdownRemaining = joystickCountdown;


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
        myRigidbody.velocity = speed * speedMultiplier * moveInput;

        
    }

    public void SetSpeedMultiplier(float value) {
        speedMultiplier = value;
    }


    private void OnEnable() {
        myPlayerInput.Enable();
    }

    private void SetSpriteRenderer() {
        float rawZ = Mathf.Round(-Mathf.Sign(moveInput.x) * Mathf.Rad2Deg * Mathf.Acos(Vector2.Dot(moveInput, Vector2.up)));
        int spriteIndex = MathUtilities.Mod(Mathf.FloorToInt(rawZ / 45f), 2);
        mySpriteRenderer.sprite = sprites[spriteIndex];

        // set player rotation only when moveInput changes to something nonzero
        if (moveInput != Vector2.zero) {
            float rotationZ = Mathf.Floor(rawZ / 90f) * 90f;
            if (spriteIndex == 1)
                rotationZ += 90f;

            myTransform.rotation = Quaternion.Euler(0, 0, rotationZ);
            healthBar.Rotate();
        }

        
        
    }



}
