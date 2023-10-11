using System.Collections.Generic;
using UnityEngine;

// scriptable object that stores information about a run
// displayed after a run terminates, in the "Stats" scene
[CreateAssetMenu]
public class RunInformation : ScriptableObject {

    public enum RunStatus {
        won,
        died,
        quit
    };

    public float damage;
    public float damageWhite;
    public float damageRed;
    public float damageBlue;
    public float damageYellow;
    public float damageError;
    public float enemiesKilled;
    public float healing;

    public int level;
    public string[] upgrades;
    public int[] upgradeLevels;

    public List<string> upgradeOrder;
    public List<string> colorOrder;

    public int difficultyLevel; // not cleared by ClearAll()
    public RunStatus runStatus;

    public float soundVolume = 1f;
    public float musicVolume = 1f;


    public void ClearAll() {
        damage = 0;
        damageWhite = 0;
        damageRed = 0;
        damageBlue = 0;
        damageYellow = 0;
        damageError = 0;
        enemiesKilled = 0;
        healing = 0;

        level = 0;
        upgrades = null;
        upgradeLevels = null;
        upgradeOrder = new List<string>();
        colorOrder = new List<string>();
        
        runStatus = RunStatus.quit;
    }


    public void IncrementDamage(float amount, string colorName) {
        damage += amount;

        switch (colorName) {
            case "White":
                damageWhite += amount;
                break;
            case "Red":
                damageRed += amount;
                break;
            case "Yellow":
                damageYellow += amount;
                break;
            case "Blue":
                damageBlue += amount;
                break;
        }
    }

}
