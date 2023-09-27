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

    private void Start() {
        objects = GameObject.Find("RunManager").GetComponent<ObjectManager>();
        stateManager = objects.GetComponent<StateManager>();

        canvasRect = objects.canvas.GetComponent<RectTransform>();

        experienceBar = objects.experienceBar.GetComponent<Bar>();
        //experienceBar.SetScale(new Vector3(canvasRect.rect.width, canvasRect.rect.height / 35, 0));
    }


    // add experience and set the experience bar display accordingly
    // if enough experience is gained at once, multiple level-ups may occur immediately one after the other
    public void AddExperience(int exp) {
        
        while (exp > 0) {

            int levelThreshold = CalculateLevelThreshold();
            int overflow = experience + exp - levelThreshold;

            if (overflow >= 0) {
                experience = 0;
                exp = overflow;
                experienceBar.SetFillValue(1f); // show a full experience bar during upgrade selection

                LevelUp();
            }
            else {
                experience += exp;
                exp = 0;
            }
        }

        experienceBar.SetFillValue((float)experience / CalculateLevelThreshold());
    }

    int CalculateLevelThreshold() {
        return ((int)(1.2f * Mathf.Pow(level + 2, 1.5f)));
    }

    void LevelUp() {
        level++;
        objects.runInformation.level = level;

        // alert the StateManager it is time to create a new UpgradeSelection
        stateManager.AddUpgrade();

    }
}
