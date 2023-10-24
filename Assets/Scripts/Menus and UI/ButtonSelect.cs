using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour {

    [SerializeField] AudioClip selectAudio, clickAudio;
    [SerializeField] Sprite defaultSprite, selectSprite, clickSprite;
    [SerializeField] UnityEvent eventOnClick;
    [SerializeField] bool waitForAudio = false;
    [SerializeField] bool startSelected = false;
    [SerializeField] RunInformation runInformation;

    EventSystem eventSystem;
    TextMeshProUGUI[] tmp;
    Color highlightedColor = new Color(1f, 221f / 255f, 71f / 255f, 1f);
    AudioSource audioSource;
    Image image;
    Selectable selectable;

    public bool selected = false;
    bool clicked = false;
    bool playSound = true;

    private void Awake() {
        eventSystem = FindObjectOfType<EventSystem>();
        tmp = GetComponentsInChildren<TextMeshProUGUI>();
        audioSource = GetComponent<AudioSource>();
        image = GetComponent<Image>();
        selectable = GetComponent<Selectable>();
    }

    void OnEnable() {
        if (startSelected) {
            audioSource.Stop();
            playSound = false;

            Select();
        } else {
            playSound = true;
        }
    }

    private void OnDisable() {
        Deselect();
    }

    private void Update() {
        if (clicked && (!waitForAudio || !audioSource.isPlaying)) {
            clicked = false;
            selected = false;
            eventSystem.enabled = true;
            eventOnClick.Invoke();
        }

    }

    public void Select() {
        if (tmp == null) {
            Awake();
        }

        selectable.Select();
        SetAllTMPColor(highlightedColor);
        image.sprite = selectSprite;
        selected = true;

        if(playSound) {

            PlayAudio("select");
        }
    }

    public void Click() {
        PlayAudio("click");
        clicked = true;
        image.sprite = clickSprite;
        eventSystem.enabled = false;
    }



    void PlayAudio(string name) {
        switch (name) {
            case "select":
                audioSource.clip = selectAudio;
                audioSource.volume = 0.3f * runInformation.soundVolume;
                break;
            case "click":
                audioSource.clip = clickAudio;
                audioSource.volume = runInformation.soundVolume;
                break;
        }

        audioSource.Stop();
        audioSource.Play();
    }



    public void Deselect() {
        if (!eventSystem.alreadySelecting)
            eventSystem.SetSelectedGameObject(null);
        SetAllTMPColor(Color.white);
        image.sprite = defaultSprite;
        
        selected = false;
        playSound = true;
    }

    void SetAllTMPColor(Color color) {
    foreach(TextMeshProUGUI t in tmp) {
            t.color = color;
    }
    }

}
