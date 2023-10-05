using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

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
    private static readonly float[] ratesCircle = new float[] { 2f, 1.6f, 1.3f, 1.05f, 0.85f, 0.7f, 0.6f };
    private static readonly float[] hitCountsCircle = new float[] { 10f, 20f, 30f, 40f, 50f, 75f, 100f };

    // red attributes
    private static readonly float[] redDamageMultiplier = new float[] { 0.5f, 1f, 1f, 1f, 1f, 1.5f, 1.5f, 1.5f };
    private static readonly float[] redCriticalChance = new float[] { 0.1f, 0.1f, 0.2f, 0.2f, 0.3f, 0.3f, 0.4f, 0.4f };
    private static readonly float[] redExplosionSize = new float[] { 0f, 0f, 0f, 0.25f, 0.25f, 0.25f, 0.25f, 0.5f };
    private static readonly int[] redChainNumber = new int[] { 0, 0, 0, 1, 1, 1, 1, 1 };

    // yellow attributes
    private static readonly float[] yellowDamageMultiplier = new float[] { 0.6f, 1.2f, 1.2f, 1.8f, 1.8f, 3f, 3f, 3f };
    private static readonly int[] yellowSpreadNumber = new int[] { 2, 2, 4, 4, 6, 6, 8, 8 };
    private static readonly float[] yellowTickLength = new float[] { 0.2f, 0.2f, 0.2f, 0.2f, 0.2f, 0.2f, 0.2f, 0.1f };
    private static readonly float[] yellowDuration = new float[] { 5f, 5f, 5f, 5f, 7f, 7f, 7f, 7f };

    // blue attributes
    private static readonly float[] blueDamage = new float[] { 0f, 10f, 10f, 10f, 35f, 35f, 35f, 35f };
    private static readonly float[] blueSpeedModifier = new float[] { 0.7f, 0.7f, 0.7f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f };
    private static readonly float[] blueDuration = new float[] { 3f, 3f, 5f, 5f, 5f, 5f, 10f, 10f };
    private static readonly float[] blueDamageDelay = new float[] { 0f, 3f, 3f, 3f, 3f, 1.5f, 1.5f, 1.5f };
    private static readonly bool[] blueCountTrigger = new bool[] { false, false, false, false, false, false, false, true };


    // secondary color variables
    const int upgradedRedChainNumber = 2;

    // summary of all upgrade names/categories and max levels
    private static readonly string[] attributeNames = new string[] { "Health", "Speed", "Healing", "Armor", "Damage",
                                                                     "Weapon", "Weapon",
                                                                     "Red", "Yellow", "Blue",
                                                                     "Orange", "Purple", "Green"};
    private static readonly int[] maxAttributeLevels = new int[] { 3, 3, 3, 3, 3, 6, 6, 7, 7, 7, 2, 2, 2 };
    readonly int[] startingAttributeLevels = new int[] { 0, 0, 0, 0, 0, 0, 0, -1, -1, -1, -1, -1, -1 };
    int[] attributeLevels;

    float[][] attributes = new float[][] { healths, speeds, healings, armors, damageMultipliers, ratesCircle, hitCountsCircle };
    private static readonly string[] colorNames = new string[] { "Red", "Yellow", "Blue", "Orange", "Purple", "Green" };

    HealthManager healthManager;
    PlayerMovement playerMovement;
    ObjectManager objects;

    // Lists to maintain the order in which upgrades are encountered, used in UI
    List<string> upgradeOrder, colorOrder;

    private void Awake() {

        objects = GameObject.Find("RunManager").GetComponent<ObjectManager>();

        healthManager = GetComponent<HealthManager>();
        playerMovement = GetComponent<PlayerMovement>();


    }
   

    public void ApplyUpgrade(string attributeName) {
        LevelUp(attributeName);
        SetValue(attributeName);
    }

    private void SetValue(string attributeName) {
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
                Weapon.SetDamageMultiplier(GetAttributeValue(attributeName));
                break;
            case "Weapon":
                SetWeaponValues();
                break;
            case "Red":
                SetRedValues();
                break;
            case "Yellow":
                SetYellowValues();
                break;
            case "Blue":
                SetBlueValues();
                break;
            case "Orange":
                SetOrangeValues();
                break;
            case "Purple":
                SetPurpleValues();
                break;
            case "Green":
                SetGreenValues();
                break;

            default:
                break;
        }
    }


    // returns a sorted dictionary containing the names of all upgradeable attributes, paired with their next level
    // basic & weapon attributes are upgradeable if they are not yet max level
    // color upgrades have additional requirements described above the methods CullColorUpgrades and CullSecondaryColorUpgrades
    public SortedList<string, int> GetAvailableUpgradeLevels() {
        SortedList<string, int> result = new SortedList<string, int>();
        foreach (string attributeName in attributeNames) {
            if (!IsMaxLevel(attributeName) && !result.ContainsKey(attributeName)) {
                result.Add(attributeName, Level(attributeName) + 1);
            }
        }

        return CullColorUpgrades(result);
    }

    // only 2 color upgrades can be active at the same time
    // checks if two colors are level > -1 and if so removes the third
    SortedList<string, int> CullColorUpgrades(SortedList<string, int> result) {
        bool red = Level("Red") > -1,
            yellow = Level("Yellow") > -1,
            blue = Level("Blue") > -1;

        if (red && yellow)
            result.Remove("Blue");
        else if (red && blue)
            result.Remove("Yellow");
        else if (yellow && blue)
            result.Remove("Red");

        return CullSecondaryColorUpgrades(result);
    }

    // removes any secondary color upgrades that are not available
    // a secondary color upgrade is only available if both of its component primary colors are max level
    SortedList<string, int> CullSecondaryColorUpgrades(SortedList<string, int> result) {
        bool red = IsMaxLevel("Red"),
            yellow = IsMaxLevel("Yellow"),
            blue = IsMaxLevel("Blue");

        if (!red || !yellow)
            result.Remove("Orange");
        if (!red || !blue)
            result.Remove("Purple");
        if (!yellow || !blue)
            result.Remove("Green");

        return result;
    }


    // returns the current level of the given attribute
    int Level(string attributeName) {
        return (attributeLevels[Array.IndexOf(attributeNames, attributeName)]);
    }

    // returns the max level of the given attribute
    int MaxLevel(string attributeName) {
        return (maxAttributeLevels[Array.IndexOf(attributeNames, attributeName)]);
    }

    // increases the current level for the given attribute (capped at max level)
    void LevelUp(string attributeName) {
        for (int i = 0; i < attributeLevels.Length; i++) {
            if (attributeNames[i] == attributeName) {
                attributeLevels[i]++;
                attributeLevels[i] = Mathf.Min(attributeLevels[i], MaxLevel(attributeName));
            }
        }

        if (colorNames.Contains(attributeName)) {
            if (!colorOrder.Contains(attributeName))
                colorOrder.Insert(0, attributeName);
        }
        else if (!upgradeOrder.Contains(attributeName))
            upgradeOrder.Insert(0, attributeName);

        objects.runInformation.upgrades = attributeNames;
        objects.runInformation.upgradeLevels = attributeLevels;
        objects.runInformation.upgradeOrder = upgradeOrder;
        objects.runInformation.colorOrder = colorOrder;
        objects.GetComponent<UIManager>().UpdateUpgradeDisplay();
    }

    // returns true if the attribute is not yet at max level
    bool IsMaxLevel(string attributeName) {
        return (Level(attributeName) == MaxLevel(attributeName));
    }

    // returns the value of the given attribute given that attribute's current level
    // e.g. if health is level 0, returns 200
    public float GetAttributeValue(string attribute) {
        int attributeIndex = Array.IndexOf(attributeNames, attribute);
        int attributeLevel = attributeLevels[attributeIndex];

        return (attributes[attributeIndex][attributeLevel]);
    }

    // since upgrading the weapon and the various weapon color attributes involves upgrading multiple different attributes,
    // we define separate functions to take care of these case
    void SetWeaponValues() {
        int level = Level("Weapon");

        Weapon.rate = ratesCircle[level];
        Weapon.hitCount = hitCountsCircle[level];
    }

    void SetRedValues() {
        int level = Level("Red");
        if(level == -1) {
            Weapon.isRedActive = false;
            return;
        }

        Weapon.isRedActive = true;
        Weapon.redCriticalChance = redCriticalChance[level];
        Weapon.redDamageMultiplier = redDamageMultiplier[level];
        Weapon.redExplosionSize = redExplosionSize[level];
        Weapon.redChainNumber = redChainNumber[level];
    }

    void SetYellowValues() {
        int level = Level("Yellow");
        if(level == -1) {
            Weapon.isYellowActive = false;
            return;
        }

        Weapon.isYellowActive = true;
        Weapon.yellowSpreadNumber = yellowSpreadNumber[level];
        Weapon.yellowTickLength = yellowTickLength[level];
        Weapon.yellowDamageMultiplier = yellowDamageMultiplier[level];
        Weapon.yellowDuration = yellowDuration[level];
    }

    void SetBlueValues() {
        int level = Level("Blue");
        if(level == -1) {
            Weapon.isBlueActive = false;
            return;
        }

        Weapon.isBlueActive = true;
        Weapon.blueDamage = blueDamage[level];
        Weapon.blueDamageDelay = blueDamageDelay[level];
        Weapon.blueDuration = blueDuration[level];
        Weapon.blueSpeedModifier = blueSpeedModifier[level];
        Weapon.isBlueCountTrigger = blueCountTrigger[level];

    }

    void SetGreenValues() {
        int level = Level("Green");

        Weapon.blueSpreadsWithYellow = level >= 0;
        Weapon.yellowUsesBlueDuration = level >= 1;
        Weapon.isBlueMultiHitActive = level >= 2;
    }

    void SetOrangeValues() {
        int level = Level("Orange");

        Weapon.yellowCanHitCritically = level >= 0;
        Weapon.redChainNumber = (level >= 1 ? upgradedRedChainNumber : 1);
        Weapon.yellowDamageTriggersExplosion = level >= 2;

    }

    void SetPurpleValues() {
        int level = Level("Purple");

        Weapon.explosionsAddBlue = level >= 0;
        Weapon.redDamageTriggersBlue = level >= 1;
        Weapon.blueDamageTriggersExplosion = level >= 2;
    }

    public void ResetAll() {
        upgradeOrder = new List<string>();
        colorOrder = new List<string>();
        attributeLevels = startingAttributeLevels;

        for (int i = 0; i < attributeNames.Length; i++) {
            SetValue(attributeNames[i]);
        }
    }
}
