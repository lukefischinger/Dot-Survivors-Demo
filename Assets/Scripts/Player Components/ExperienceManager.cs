using UnityEngine;

// player component
// tracks current experience and threshold needed for next level up
// passes information to the ExperienceBar for display
// upon level up, alerts the RunManager object to generate a new UpgradeSelection
public class ExperienceManager : MonoBehaviour {

    ObjectManager objects;

    int experience = 0;
    int level = 0;
    Bar experienceBar;
    RectTransform canvasRect;
    StateManager stateManager;

    private void Awake() {
        objects = GameObject.Find("RunManager").GetComponent<ObjectManager>();
        stateManager = objects.GetComponent<StateManager>();

        canvasRect = objects.canvas.GetComponent<RectTransform>();

        experienceBar = objects.experienceBar.GetComponent<Bar>();
    }

    private void Start() {
        experienceBar.SetScale(new Vector3(canvasRect.rect.width, canvasRect.rect.height / 40, 0));
        experienceBar.SetFillValue(0);
    }

    public void AddExperience(int exp) {
        int levelThreshold = CalculateLevelThreshold();

        if (experience + exp >= levelThreshold) {
            if (experience + exp > levelThreshold)
                AddExperience(experience + exp - levelThreshold);
            LevelUp();
        }
        else {
            experience += exp;
            experienceBar.SetFillValue((float)experience / levelThreshold);
        }
    }

    int CalculateLevelThreshold() {
        return ((int)(1.2f * Mathf.Pow(level + 2, 1.5f)));
    }

    void LevelUp() {
        level++;
        experience = 0;

        // alert the RunManager it is time to create a new UpgradeSelection
        stateManager.CreateUpgradeSelection();

    }
}
