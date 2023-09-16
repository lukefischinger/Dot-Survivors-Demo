using System.Collections.Generic;
using System;
using UnityEngine;

// player component
// manages the various player attributes which are upgradeable
public class AttributeManager : MonoBehaviour {
    private static readonly float[] healths = new float[] { 200, 240, 280, 350 };
    private static readonly float[] speeds = new float[] { 1f, 1.2f, 1.4f, 1.7f };
    private static readonly float[] healings = new float[] { 0, 1, 2, 4 };
    private static readonly float[] armors = new float[] { 0, 2f, 4f, 6f };
    private static readonly float[] damageMultipliers = new float[] { 1, 1.2f, 1.4f, 1.7f };

    // weapon attributes (circle)
    // unlike the attributes above, all weapon attributes are determined by a single weapon level from 0 to 6
    private static readonly float[] ratesCircle = new float[] { 2f, 1.6f, 1.28f, 1.024f, 0.8192f, 0.65536f, 0.524288f };
    private static readonly float[] speedsCircle = new float[] { 500f, 550f, 600f, 650f, 700f, 750f, 800f };
    private static readonly float[] hitCountsCircle = new float[] { 10f, 20f, 30f, 40f, 50f, 75f, 100f };

    private static readonly string[] attributeNames = new string[] { "Health", "Speed", "Healing", "Armor", "Damage", "Weapon", "Weapon", "Weapon" };
    private static readonly int[] maxAttributeLevels = new int[] { 3, 3, 3, 3, 3, 6, 6, 6 };

    float[][] attributes = new float[][] { healths, speeds, healings, armors, damageMultipliers, ratesCircle, speedsCircle, hitCountsCircle };
    int[] attributeLevels = new int[] { 0, 0, 0, 0, 0, 0, 0, 0 };

    HealthManager healthManager;
    PlayerMovement playerMovement;
    Weapon weapon;

    private void Awake() {
        healthManager = GetComponent<HealthManager>();
        playerMovement = GetComponent<PlayerMovement>();
        weapon = GetComponent<Weapon>();
        SetWeaponValues();
        healthManager.SetMaxHealth(GetAttributeValue("Health"), false);
    }

    public void ApplyUpgrade(string attributeName) {
        LevelUp(attributeName);

        switch (attributeName) {
            case "Health":
                healthManager.SetMaxHealth(GetAttributeValue(attributeName));
                break;
            case "Speed":
                playerMovement.SetSpeedMultiplier(GetAttributeValue(attributeName));
                break;
            case "Healing":
                healthManager.SetHealing(GetAttributeValue(attributeName));
                break;
            case "Armor":
                healthManager.SetArmor(GetAttributeValue(attributeName));
                break;
            case "Damage":
                weapon.SetDamageMultiplier(GetAttributeValue(attributeName));
                break;
            case "Weapon":
                SetWeaponValues();
                break;

            default:
                break;
        }
    }


    // returns a sorted dictionary containing the names of all upgradeable attributes, paired with their next level
    // attributes are upgradeable if they are not yet max level
    public SortedList<string, int> GetAvailableUpgradeLevels() {
        SortedList<string, int> result = new SortedList<string, int>();
        foreach (string attributeName in attributeNames) {
            if (IsUpgradeable(attributeName) && !result.ContainsKey(attributeName)) {
                result.Add(attributeName, Level(attributeName) + 1);
            }
        }

        return result;
    }



    // returns the current level of the given attribute
    int Level(string attributeName) {
        return (attributeLevels[System.Array.IndexOf(attributeNames, attributeName)]);
    }

    // returns the max level of the given attribute
    int MaxLevel(string attributeName) {
        return (maxAttributeLevels[System.Array.IndexOf(attributeNames, attributeName)]);
    }

    // increases the current level for the given attribute (capped at max level)
    void LevelUp(string attributeName) {
        for (int i = 0; i < attributeLevels.Length; i++) {
            if (attributeNames[i] == attributeName) {
                attributeLevels[i]++;
                attributeLevels[i] = Mathf.Min(attributeLevels[i], MaxLevel(attributeName));
            }
        }
    }

    // returns true if the attribute is not yet at max level
    bool IsUpgradeable(string attributeName) {
        return (Level(attributeName) < MaxLevel(attributeName));
    }

    // returns the value of the given attribute given that attribute's current level
    // e.g. if health is level 0, returns 200
    public float GetAttributeValue(string attribute) {
        int attributeIndex = Array.IndexOf(attributeNames, attribute);
        int attributeLevel = attributeLevels[attributeIndex];

        return (attributes[attributeIndex][attributeLevel]);
    }

    // since upgrading the weapon involves upgrading multiple different attributes, we define a separate function to take care of this case
    void SetWeaponValues() {
        List<float> attributeValues = new List<float>();

        for (int i = 0; i < attributeNames.Length; i++) {
            if(attributeNames[i] == "Weapon") {
                attributeValues.Add(attributes[i][Level("Weapon")]);
            }    
        }

        weapon.SetRateSpeedAndHitCount(attributeValues);
    }

}
