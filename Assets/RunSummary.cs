using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunSummary : MonoBehaviour
{

    [SerializeField] RunInformation runInformation;

    private void Start() {
        PopulateStatistics();
        
    }

    public void StartRun() {
        SceneManager.LoadScene(1);
    }

    public void QuitToMainMenu() {
        SceneManager.LoadScene(0);
    }

    private void PopulateStatistics() {
        GameObject.Find("Total Damage Amount").GetComponent<TextMeshProUGUI>().text = "" + FormatNumber(runInformation.damage);
        GameObject.Find("White Damage Amount").GetComponent<TextMeshProUGUI>().text = "" + FormatNumber(runInformation.damageWhite);
        GameObject.Find("Red Damage Amount").GetComponent<TextMeshProUGUI>().text = "" + FormatNumber(runInformation.damageRed);
        GameObject.Find("Blue Damage Amount").GetComponent<TextMeshProUGUI>().text = "" + FormatNumber(runInformation.damageBlue);
        GameObject.Find("Yellow Damage Amount").GetComponent<TextMeshProUGUI>().text = "" + FormatNumber(runInformation.damageYellow);
        GameObject.Find("Enemies Killed Amount").GetComponent<TextMeshProUGUI>().text = "" + FormatNumber(runInformation.enemiesKilled);
        GameObject.Find("Healing Done Amount").GetComponent<TextMeshProUGUI>().text = "" + FormatNumber(runInformation.healing);



    }

    private string FormatNumber(float number) {
        return string.Format("{0:#,#}", Mathf.RoundToInt(number));
    }

}
