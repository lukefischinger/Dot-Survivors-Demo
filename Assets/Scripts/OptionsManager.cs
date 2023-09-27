using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour {

    [SerializeField] GameObject mainMenu;
    [SerializeField] RunInformation runInformation;
    [SerializeField] TextMeshProUGUI difficultyButtonText;

    string[] difficultyLevels = new string[3] { "normal", "hard", "brutal" };

    int index;

    private void Awake() {
        Reset();
    }

    public void CycleDifficulty() {
        index = (index + 1) % difficultyLevels.Length;
        runInformation.difficultyLevel = index;
        difficultyButtonText.text = difficultyLevels[index];
    }

    public void BackToMainMenu() {
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void SelectDifficulty() {
        difficultyButtonText.color = new Color(255f / 255f, 221f / 255f, 73f / 255f, 1);
    }

    public void DeselectDifficulty() {
        difficultyButtonText.color = Color.white;

    }

    private void OnEnable() {
        Reset();   
    }

    private void Reset() {
        index = runInformation.difficultyLevel;
        difficultyButtonText.text = difficultyLevels[index];
        difficultyButtonText.GetComponentInParent<Button>().Select();
    }

}
