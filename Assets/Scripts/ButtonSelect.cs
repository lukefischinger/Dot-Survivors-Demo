using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour {
    EventSystem eventSystem;
    Button button;

    public bool selected = false;

    private void Awake() {
        eventSystem = GameObject.FindObjectOfType<EventSystem>();
        button = GetComponent<Button>();
    }
    public void OnHoverEnter() {
        button.Select();
        selected = true;
    }

    public void OnHoverExit() {
        eventSystem.SetSelectedGameObject(null);
        selected = false;
    }
}
