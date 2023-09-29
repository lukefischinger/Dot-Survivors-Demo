using TMPro;
using UnityEngine;

public class UIText : MonoBehaviour
{
    ObjectManager objects;
    Clock clock;
    TextMeshPro tmp;

    const float disappearTime = 5f;

    private void Awake() {
        objects = GameObject.Find("RunManager").GetComponent<ObjectManager>();
        clock = objects.clock.GetComponent<Clock>();
        tmp = GetComponent<TextMeshPro>();
        SetText("survive for 10 minutes", Color.white);
    }


    private void Update() {
        if (clock.timeSpan.TotalSeconds > disappearTime)
            gameObject.SetActive(false);
        
    }

    public void SetText(string text, Color color) {
        tmp.text = text;
        tmp.color = color;
    }
}
