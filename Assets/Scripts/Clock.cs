using System;
using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public float timeAwake;
    TextMeshProUGUI text;

    private void Awake() {
        timeAwake = Time.time;
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Update() {
        SetClock();
    }

    void SetClock() {
        TimeSpan timeSpan = TimeSpan.FromSeconds(Time.time - timeAwake);
        string timeText = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        text.text = timeText;
    }
}
