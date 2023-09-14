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
    
    private static readonly string[] attributeNames = new string[] { "Health", "Speed", "Healing", "Armor", "Damage" };
    private static readonly int[] maxAttributeLevels = new int[] { 3, 3, 3, 3, 3, 3 };

    float[][] attributes = new float[][] { healths, speeds, healings, armors, damageMultipliers };
    int[] attributeLevels = new int[] { 0, 0, 0, 0, 0 };

    HealthManager healthManager;
    PlayerMovement playerMovement;
    Weapon weapon;

    private void Awake() {
        healthManager = GetComponent<HealthManager>();
        playerMovement = GetComponent<PlayerMovement>();
        weapon = GetComponent<Weapon>();
    }


    public void ApplyUpgrade(string attributeName) {
        LevelUp(attributeName);
        float attributeValue = GetAttributeValue(attributeName);

        switch (attributeName) {
            case "Health":
                healthManager.SetMaxHealth(attributeValue);
                break;
            case "Speed":
                playerMovement.SetSpeedMultiplier(attributeValue);
                break;
            case "Healing":
                healthManager.SetHealing(attributeValue);
                break;
            case "Armor":
                healthManager.SetArmor(attributeValue);
                break;
            case "Damage":
                weapon.SetDamageMultiplier(attributeValue);
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
            if (IsUpgradeable(attributeName)) {
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
        attributeLevels[System.Array.IndexOf(attributeNames, attributeName)]++;
        attributeLevels[System.Array.IndexOf(attributeNames, attributeName)] = Mathf.Min(Level(attributeName), MaxLevel(attributeName));

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

}
