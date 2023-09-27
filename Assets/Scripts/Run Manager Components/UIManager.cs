using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

// maintains appropriate scaling for the experience bar and upgrade screen based on the width of the screen
// all other UI elements are scaled automatically according to the screen height
public class UIManager : MonoBehaviour {

    [SerializeField] public List<Sprite> sprites;
    [SerializeField] public List<Color> colors;
    [SerializeField] public List<string> spriteNames, colorNames;

    ObjectManager objects;
    Canvas canvas;
    List<GameObject> damagePool;
    GameObject upgradeDisplay, pauseButton;
    RunInformation runInformation;
    PlayerMovement playerMovement;

    Vector2
        currentDimensions,
        newDimensions;

    RectTransform
    canvasRect,
        experienceBar,
        upgradeScreen;

    const float barHeightRatio = 1f / 35f;
    const float referenceWidth = 1920f;

    private void Awake() {
        objects = GetComponent<ObjectManager>();
        canvas = objects.canvas.GetComponent<Canvas>();
        damagePool = objects.damagePool.GetComponent<Pool>().pool;
        upgradeDisplay = objects.upgradeDisplay;
        runInformation = objects.runInformation;
        pauseButton = objects.pauseButton;
        playerMovement = objects.player.GetComponent<PlayerMovement>();

        canvasRect = canvas.GetComponent<RectTransform>();
        experienceBar = objects.experienceBar.GetComponent<RectTransform>();
        upgradeScreen = objects.upgradeScreen.GetComponent<RectTransform>();


        currentDimensions = canvasRect.localScale;

    }

    private void Start() {
        SetPauseButton();
        SetUI();
        UpdateUpgradeDisplay();
    }

    private void Update() {
        SetPauseButton();
            newDimensions = canvasRect.localScale;
        if (newDimensions == currentDimensions) return;

        SetUI();
    }


    void SetUI() {
        float ratioFromReference = canvasRect.rect.width / referenceWidth;

        upgradeScreen.localScale = 2 * ratioFromReference * Vector3.one;
        if (upgradeScreen.localScale.x > 1)
            upgradeScreen.localScale = Vector3.one;

        experienceBar.localScale = new Vector3(canvasRect.rect.width, barHeightRatio * canvasRect.rect.height, 1);

        for (int i = 0; i < damagePool.Count; i++) {
            damagePool[i].transform.localScale = Vector3.one * ratioFromReference;
        }

        currentDimensions = newDimensions;
    }

    void SetPauseButton() {
        pauseButton.SetActive(playerMovement.usingJoystick);
    }


    public void UpdateUpgradeDisplay() {
        for (int i = 0; i < runInformation.upgradeOrder.Count; i++) {
            SetUpgradeDisplaySprite(i);
            SetUpgradeDisplayLevel(i);
        }

        for (int i = runInformation.upgradeOrder.Count; i < 6; i++) {
            upgradeDisplay.transform.GetChild(1).GetChild(i).gameObject.SetActive(false);
            upgradeDisplay.transform.GetChild(2).GetChild(i).gameObject.SetActive(false);
        }


        for (int i = 0; i < runInformation.colorOrder.Count; i++) {
            SetColorDisplaySprite(i);
        }

        for (int i = runInformation.colorOrder.Count; i < 3; i++) {
            upgradeDisplay.transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
        }
    }

    private void SetColorDisplaySprite(int i) {
        Color color = colors[colorNames.IndexOf(runInformation.colorOrder[i])];
        upgradeDisplay.transform.GetChild(0).GetChild(i).GetComponent<SpriteRenderer>().color = color;
        upgradeDisplay.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
    }

    private void SetUpgradeDisplaySprite(int i) {
        Sprite sprite = sprites[spriteNames.IndexOf(runInformation.upgradeOrder[i])];
        upgradeDisplay.transform.GetChild(1).GetChild(i).GetComponent<SpriteRenderer>().sprite = sprite;
        upgradeDisplay.transform.GetChild(1).GetChild(i).gameObject.SetActive(true);
    }

    private void SetUpgradeDisplayLevel(int i) {
        int level = runInformation.upgradeLevels[Array.IndexOf(runInformation.upgrades, runInformation.upgradeOrder[i])];
        upgradeDisplay.transform.GetChild(2).GetChild(i).GetComponent<TextMeshPro>().text = level.ToString();
        upgradeDisplay.transform.GetChild(2).GetChild(i).gameObject.SetActive(true);
    }


}
