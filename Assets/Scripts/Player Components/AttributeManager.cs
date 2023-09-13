using System.Collections.Generic;
using System;
using UnityEngine;

// player component
// manages the various player attributes which are upgradeable
public class AttributeManager : MonoBehaviour {
    private static readonly float[] healths = new float[] { 200, 240, 280, 350 };
    private static readonly float[] speeds = new float[] { 300, 360, 420, 510 };
    private static readonly float[] healings = new float[] { 0, 1, 2, 4 };
    private static readonly float[] armors = new float[] { 0, 2f, 4f, 6f };
    private static readonly float[] damageMultipliers = new float[] { 1, 1.2f, 1.4f, 1.7f };
    private static readonly float[] knockbackMultipliers = new float[] { 1, 1.2f, 1.4f, 1.7f };
    
    private static readonly string[] attributeNames = new string[] { "Health", "Speed", "Healing", "Armor", "Damage", "Knockback" };
    private static readonly int[] maxAttributeLevels = new int[] { 3, 3, 3, 3, 3, 3 };

    float[][] attributes = new float[][] { healths, speeds, healings, armors, damageMultipliers, knockbackMultipliers };
    int[] attributeLevels = new int[] { 0, 0, 0, 0, 0, 0 };

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

        switch (attributeName) {
            case "Health":
                healthManager.SetMaxHealth(healths[Level("Health")]);
                break;
            case "Speed":
                playerMovement.SetSpeedMultiplier(speeds[Level("Speed")]);
                break;
            case "Damage":
                weapon.SetDamageMultiplier(damageMultipliers[Level("Damage")]);
                break;
            default:
                break;
        }
    }

    public List<string> GetAvailableUpgrades() {
        List<string> returnList = new List<string>();
        foreach (string attributeName in attributeNames) {
            if (IsUpgradeable(attributeName)) {
                returnList.Add(attributeName);
            }
        }

        return (returnList);
    }

    public List<int> GetAvailableAttributeLevels() {
        List<int> returnList = new List<int>();
        foreach (string attributeName in GetAvailableUpgrades()) {
            returnList.Add(Level(attributeName));
        }

        return returnList;
    }

    int Level(string attributeName) {
        return (attributeLevels[System.Array.IndexOf(attributeNames, attributeName)]);
    }

    int MaxLevel(string attributeName) {
        return (maxAttributeLevels[System.Array.IndexOf(attributeNames, attributeName)]);
    }

    void LevelUp(string attributeName) {
        attributeLevels[System.Array.IndexOf(attributeNames, attributeName)]++;
        attributeLevels[System.Array.IndexOf(attributeNames, attributeName)] = Mathf.Min(Level(attributeName), MaxLevel(attributeName));

    }

    bool IsUpgradeable(string attributeName) {
        return (Level(attributeName) < MaxLevel(attributeName));
    }


    public float GetAttributeValue(string attribute) {
        int attributeIndex = Array.IndexOf(attributeNames, attribute);
        int attributeLevel = attributeLevels[attributeIndex];

        return (attributes[attributeIndex][attributeLevel]);
    }

}
