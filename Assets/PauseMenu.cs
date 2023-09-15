using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    ObjectManager objects;
    StateManager stateManager;

    private void Awake() {
        objects = GameObject.Find("RunManager").GetComponent<ObjectManager>();
        stateManager = objects.GetComponent<StateManager>();
    }

    public void QuitToMainMenu() {
        SceneManager.LoadScene(0);
    }

    public void Unpause() {
        stateManager.Unpause();
    }
}
