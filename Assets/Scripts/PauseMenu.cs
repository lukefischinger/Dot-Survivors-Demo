using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    ObjectManager objects;
    StateManager stateManager;
    Button unpauseButton;
    GameObject optionsScreen;

    private void Awake() {
        objects = GameObject.Find("RunManager").GetComponent<ObjectManager>();
        stateManager = objects.GetComponent<StateManager>();
        unpauseButton = transform.GetChild(0).GetComponent<Button>();
        optionsScreen = objects.optionsScreen;
    }

   

    public void OpenOptions() {
        optionsScreen.SetActive(true);
        transform.parent.gameObject.SetActive(false);
    }

   
   
}
