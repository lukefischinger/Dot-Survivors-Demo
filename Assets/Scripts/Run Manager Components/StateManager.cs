using UnityEngine;

// manages the state of the run (running, upgrading, or paused)
public class StateManager : MonoBehaviour
{

    [SerializeField] GameObject upgradeSelectionPrefab;

    public enum State {
        Running,
        Paused,
        Upgrading
    }

    public State state = State.Running;
    public State previousState = State.Running;
    bool clickPause, pause, upgradeActive;

    ObjectManager objects;
    PlayerInput uiInput;
    int upgradesQueued = 0;
    GameObject upgradeSelection, pauseMenu, optionsMenu;

    private void Start() {
        objects = GetComponent<ObjectManager>();
        uiInput = objects.player.GetComponent<PlayerMovement>().myPlayerInput;
        pauseMenu = objects.pauseScreen;
        optionsMenu = objects.optionsScreen;
        upgradeSelection = objects.upgradeScreen;
    }

    private void Update() {
        UpdateState();
    }

    private void UpdateState() {
        UpdatePauseInput();

        switch (state) {
            case State.Running:
                Running();
                break;
            case State.Upgrading:
                Upgrading();
                break;
            case State.Paused:
                Paused();
                break;
            default:
                break;
        }
    }

    public void ClickPause() {
        clickPause = true;
    }

    void UpdatePauseInput() {
        pause = clickPause || uiInput.UI.Pause.triggered;
        clickPause = false;
    }

    void Running() {
        Time.timeScale = 1;
        if (pause) {
            state = State.Paused;
            previousState = State.Running;
        }
    }

    // if there is no active upgrade screen, create one
    private void Upgrading() {
        Time.timeScale = 0;

        if(pause) {
            state = State.Paused;
            previousState = State.Upgrading;
            upgradeSelection.SetActive(false);
        } else if(!upgradeActive){
            if (upgradesQueued > 0) {
                CreateUpgradeSelection();
            }
        } else if(upgradeSelection != null && !upgradeSelection.activeInHierarchy)
            upgradeSelection.SetActive(true);
    }

    private void Paused() {
        Time.timeScale = 0;
        
        if(!pauseMenu.activeInHierarchy && !optionsMenu.activeInHierarchy)
            pauseMenu.SetActive(true);

        if (pause) {
            Unpause();
        }
    }

    void CreateUpgradeSelection() {
        state = State.Upgrading;
        upgradeActive = true;
        upgradeSelection.SetActive(true);
        
    }

    // if multiple level ups occur at the same time, we want to ensure the upgrade selection objects are made in sequence rather than simultaneously
    // to do this, the experience manager adds an upgrade to the queue, and the state manager creates the upgrade selections at the appropriate times
    public void AddUpgrade() {
        upgradesQueued++;
        state = State.Upgrading;
    }

    public void CompleteUpgrade() {
        upgradesQueued = Mathf.Max(upgradesQueued - 1, 0);
        upgradeActive = false;
        upgradeSelection.SetActive(false);

        if(upgradesQueued == 0) {
            previousState = state;
            state = State.Running;
        }
    }

    public void Unpause() {
        state = previousState;
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
    }
}
