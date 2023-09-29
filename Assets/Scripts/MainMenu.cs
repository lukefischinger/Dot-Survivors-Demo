using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    [SerializeField] GameObject options;
    [SerializeField] Button startButton;

    public void StartRun() {
        SceneManager.LoadScene(1);
    }


    public void QuitGame() {
        Application.Quit();
    }

    public void OpenOptionsMenu() {
        options.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnEnable() {
        startButton.GetComponent<ButtonSelect>().Select();
    }
}
