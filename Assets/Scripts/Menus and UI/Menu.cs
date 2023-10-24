using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    [SerializeField] OptionsManager options;
    [SerializeField] ObjectManager objects;

    List<GameObject> menuButtons = new List<GameObject>();

    private void Awake() {
        for (int i = 0; i < transform.childCount; i++) {
            menuButtons.Add(transform.GetChild(i).gameObject);
        }

        options.SetVisible(false);
        SetVisible(false);
        SetVisible(true);

    }

    public void StartRun() {
        SceneManager.LoadScene(1);
    }


    public void QuitGame() {
        Application.Quit();
    }

    public void OpenOptionsMenu() {
        SetVisible(false);
        options.SetVisible(true);
    }

    public void EndRun() {
        SceneManager.LoadScene(2);
    }

    // only used during the run scene, when the Run object manager is available
    public void Unpause() {
        if (objects != null)
            objects.GetComponent<StateManager>().Unpause();
    }

    public void SetVisible(bool visible = true) {
        GetComponent<TextMeshProUGUI>().enabled = visible;

        for (int i = 0; i < menuButtons.Count; i++) {
            Image image = menuButtons[i].GetComponent<Image>();
            if (image != null)
                image.enabled = visible;

            Selectable selectable = menuButtons[i].GetComponent<Selectable>();
            if (selectable != null)
                selectable.enabled = visible;

            ButtonSelect buttonSelect = menuButtons[i].GetComponent<ButtonSelect>();
            if (buttonSelect != null) {
                buttonSelect.enabled = visible;
            }

            SpriteRenderer spriteRenderer = menuButtons[i].GetComponent<SpriteRenderer>();
            if (spriteRenderer != null) {
                spriteRenderer.enabled = visible;
            }

            if (menuButtons[i].GetComponentInChildren<TextMeshProUGUI>() != null)
                menuButtons[i].GetComponentInChildren<TextMeshProUGUI>().enabled = visible;

        }
    }

    public bool isVisible => GetComponent<TextMeshProUGUI>().enabled;


}
