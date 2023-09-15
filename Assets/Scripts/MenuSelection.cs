using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// this script can be placed on an object to allow the user to navigate between MenuButton children objects using the mouse or keyboard arrows
// does not include the functionality of button clicks
public class MenuSelection : MonoBehaviour {


    ObjectManager objects;
    PlayerInput uiInput;
    GameObject[] buttons;

    int moveInputY = 0;
    bool moveInputYTriggered = false;
    int indexSelected;
    float selectionChangeDelay = 1f;
    float selectionChangeDelayRemaining = 0.2f;

    Vector2 mousePosition;

    void Start() {
        objects = GameObject.Find("RunManager").GetComponent<ObjectManager>();
        uiInput = objects.player.GetComponent<PlayerMovement>().myPlayerInput;

        buttons = new GameObject[transform.childCount];

        for (int i = 0; i < buttons.Length; i++) {
            buttons[i] = transform.GetChild(i).gameObject;
        }
        indexSelected = 0;
        SelectButton(indexSelected);
    }

    private void Update() {
        UpdateInput();

        // if no keyboard input this frame, check if the mouse is hovering over a button
        // otherwise, use the keyboard input
        if (moveInputY == 0) {
            int index = GetMousedOverButtonIndex();
            if (index != -1) {
                indexSelected = index;
                SelectButton(indexSelected);
            }
        }
        else {
            int index = GetKeyboardButtonIndex();
            if (index != -1) {
                indexSelected = index;
                SelectButton(indexSelected);
            }
        }
    }

    // get selected button index based on keyboard inputs
    // -1 if none
    int GetKeyboardButtonIndex() {
        int result = -1;
        if (selectionChangeDelayRemaining <= 0 || moveInputYTriggered) {
            result = MathUtilities.Mod((indexSelected - moveInputY), buttons.Length);
            Debug.Log(result + ": " + selectionChangeDelayRemaining);
            selectionChangeDelayRemaining = selectionChangeDelay;
        } else {
            selectionChangeDelayRemaining -= Time.unscaledDeltaTime;
        }


        return result;
    }

    // get selected button index based on mouse position
    // -1 if none
    int GetMousedOverButtonIndex() {
        int result = -1;

        PointerEventData eventData = new PointerEventData(null);
        eventData.position = mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        objects.canvas.GetComponent<GraphicRaycaster>().Raycast(eventData, results);

        if (results.Count > 0) {
            Debug.Log(results[0].gameObject.tag);

            for (int i = 0; i < buttons.Length; i++) {
                if (results[0].gameObject == buttons[i].transform.GetChild(0).gameObject) {
                    result = i;
                    break;
                }
            }
        }
        return result;
    }

    // updates the display to highlight the button specified by the index
    void SelectButton(int index) {
        buttons[index].GetComponent<Button>().Select();
    }

    // updates the keyboard and mouse inputs
    void UpdateInput() {
        moveInputY = Mathf.RoundToInt(uiInput.Player.Move.ReadValue<Vector2>().y);
        moveInputYTriggered = uiInput.Player.Move.triggered && moveInputY != 0;
        if (moveInputY != 0 && !moveInputYTriggered)
            selectionChangeDelayRemaining -= Time.unscaledDeltaTime;

        mousePosition = uiInput.UI.Point.ReadValue<Vector2>();
    }
}
