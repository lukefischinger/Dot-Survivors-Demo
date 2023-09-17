using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    ObjectManager objects;
    StateManager stateManager;
    Button unpauseButton;

    private void Awake() {
        objects = GameObject.Find("RunManager").GetComponent<ObjectManager>();
        stateManager = objects.GetComponent<StateManager>();
        unpauseButton = transform.GetChild(0).GetComponent<Button>();
    }

    public void QuitToMainMenu() {
        SceneManager.LoadScene(0);
    }

    public void Unpause() {
        stateManager.Unpause();
    }

    private void OnEnable() {
        unpauseButton.Select();
    }
}
