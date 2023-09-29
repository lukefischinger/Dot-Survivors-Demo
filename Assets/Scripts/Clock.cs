using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Clock : MonoBehaviour
{
    public float timeAwake;
    TextMeshProUGUI text;
    public TimeSpan timeSpan;
    ObjectManager objects;

    const float timeToWin = 10f;

    private void Awake() {
        timeAwake = Time.time;
        text = GetComponent<TextMeshProUGUI>();
        objects = GameObject.Find("RunManager").GetComponent<ObjectManager>();
    }

    private void Update() {
        SetClock();
        CheckWin();
    }

    void SetClock() {
        timeSpan = TimeSpan.FromSeconds(Time.time - timeAwake);
        string timeText = string.Format("{0:D1}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        text.text = timeText;
    }

    void CheckWin() {
        if(timeSpan.TotalMinutes > 10) {
            objects.runInformation.runStatus = RunInformation.RunStatus.won;
            SceneManager.LoadScene(2);
        }
    }
}
