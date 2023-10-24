using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour {

    [SerializeField] Menu mainMenu;
    [SerializeField] RunInformation runInformation;
    [SerializeField] TextMeshProUGUI difficultyButtonText;
    [SerializeField] Slider soundEffectsVolume, musicVolume;


    string[] difficultyLevels = new string[3] { "normal", "hard", "brutal" };

    int index;


    private void Awake() {
        Reset();
      
    }

    private void OnSetVisible() {
        Reset();
    }


    public void CycleDifficulty() {
        index = (index + 1) % difficultyLevels.Length;
        runInformation.difficultyLevel = index;
        difficultyButtonText.text = difficultyLevels[index];
    }

    public void BackToMainMenu() {
        SetVisible(false);
        mainMenu.SetVisible(true);
    }

    private void Reset() {
        index = runInformation.difficultyLevel;
        difficultyButtonText.text = difficultyLevels[index];
        soundEffectsVolume.value = runInformation.soundVolume;
        musicVolume.value = runInformation.musicVolume;

    }

    public void SetVisible(bool visible = true) {
        OnSetVisible();

        GetComponent<TextMeshProUGUI>().enabled = visible;

        foreach (SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>()) {
            sprite.enabled = visible;
        }
        foreach (Image image in GetComponentsInChildren<Image>()) {
            image.enabled = visible;
        }
        foreach (TextMeshProUGUI tmp in GetComponentsInChildren<TextMeshProUGUI>()) {
            tmp.enabled = visible;
        }
        foreach (Selectable selectable in GetComponentsInChildren<Selectable>()) {
            selectable.enabled = visible;
        }
        foreach (ButtonSelect buttonSelect in GetComponentsInChildren<ButtonSelect>()) {
            buttonSelect.enabled = visible;
        }
        foreach (Slider slider in GetComponentsInChildren<Slider>()) {
            slider.enabled = visible;
        }

    }

    public bool isVisible => GetComponent<TextMeshProUGUI>().enabled;


    public void SetSoundVolume(float value) => runInformation.soundVolume = value;
    public void SetMusicVolume(float value) => runInformation.musicVolume = value;


}
