using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour {
    EventSystem eventSystem;
    Button button;
    TextMeshProUGUI tmp;
    Color highlightedColor = new Color(1f, 221f / 255f, 71f / 255f, 1f);

    public bool selected = false;

    private void Awake() {
        eventSystem = FindObjectOfType<EventSystem>();
        button = GetComponent<Button>();
        tmp = button.GetComponentInChildren<TextMeshProUGUI>();

    }
    public void Select() {
        if (tmp == null)
            Awake();
         
        button.Select();
        selected = true;
        tmp.color = highlightedColor;

    }

    public void Deselect() {
        if(!eventSystem.alreadySelecting)
            eventSystem.SetSelectedGameObject(null);
        selected = false;
        tmp.color = Color.white;
    }

}
