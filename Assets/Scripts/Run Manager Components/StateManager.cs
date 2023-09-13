using UnityEngine;

// manages the state of the run
public class StateManager : MonoBehaviour
{

    public enum State {
        Running,
        Paused,
        Upgrading
    }

    public State state = State.Running;
    public State previousState = State.Running;
    bool pause;

    PlayerInput uiInput;

    private void Start() {
        uiInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().myPlayerInput;
    }

    private void UpdateState() {
        pause = uiInput.UI.Pause.triggered;

        switch (state) {
            case State.Running:
                Time.timeScale = 1;
                if (pause) {
                    state = State.Paused;
                    previousState = State.Running;
                }
                break;
            case State.Upgrading:
                Time.timeScale = 0;
                
                // fill in
                break;
            case State.Paused:
                Time.timeScale = 0;
                

                // fill in
                break;
            default:
                break;
        }
    }
}
