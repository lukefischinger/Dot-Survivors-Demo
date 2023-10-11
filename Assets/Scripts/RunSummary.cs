using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunSummary : MonoBehaviour
{

    [SerializeField] RunInformation runInformation;
    [SerializeField] AudioClip wonMusic, quitMusic, diedMusic;

    AudioSource audioSource;

    private void Start() {
        audioSource = GetComponent<AudioSource>();

        SetTitle();
        PopulateStatistics();
        PlayMusic();
    }


    private void Update() {
        audioSource.volume = runInformation.musicVolume;
    }

    public void StartRun() {
        SceneManager.LoadScene(1);
        runInformation.ClearAll();
    }

    public void QuitToMainMenu() {
        SceneManager.LoadScene(0);
        runInformation.ClearAll();
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

    void SetTitle() {
        TextMeshProUGUI title = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        
        switch(runInformation.runStatus) {
            case RunInformation.RunStatus.won:
                title.text = "you won!";
                title.color = Color.green;
               
                break;
            case RunInformation.RunStatus.quit:
                title.text = "summary";
                title.color = Color.white;
                
                break;
            case RunInformation.RunStatus.died:
                title.text = "you died";
                title.color = Color.red;
                
                break;

        }

    }

    void PlayMusic() {
        audioSource.clip = runInformation.runStatus switch {
            RunInformation.RunStatus.won => wonMusic,
            RunInformation.RunStatus.quit => quitMusic,
            RunInformation.RunStatus.died => diedMusic,
            _ => null
        };

        if(audioSource.clip != null)
            audioSource.Play();
    }

}
