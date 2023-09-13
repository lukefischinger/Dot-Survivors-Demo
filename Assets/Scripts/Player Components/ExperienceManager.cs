using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
    int experience = 0;
    int level = 0;
    Bar experienceBar;
    RectTransform canvasRect;

    private void Awake() {
        canvasRect = GameObject.Find("Canvas").GetComponent<RectTransform>();
        
        experienceBar = canvasRect.GetComponentInChildren<Bar>();
        experienceBar.SetFillValue(0);
        experienceBar.SetScale(new Vector3(canvasRect.rect.width, canvasRect.rect.height / 40, 0));
        Debug.Log(canvasRect.rect.width);
    }

    public void AddExperience(int exp) {
        int levelThreshold = CalculateLevelThreshold();

        if (experience + exp >= levelThreshold) {
            LevelUp();
            if (experience + exp > levelThreshold)
                AddExperience(experience + exp - levelThreshold);
        }
        else {
            experience += exp;
            experienceBar.SetFillValue(experience / levelThreshold);
        }
    }

    int CalculateLevelThreshold() {
        return ((int)(1.2f * Mathf.Pow(level + 2, 1.5f)));
    }

    void LevelUp() {
        level++;
        experience = 0;
    }
}
