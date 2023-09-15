using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartRun() {
        SceneManager.LoadScene(1);
    }


    public void QuitGame() {
        Application.Quit();
    }
}
