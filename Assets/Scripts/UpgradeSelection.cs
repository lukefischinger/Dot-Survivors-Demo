using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;
using System.Linq;

public class UpgradeSelection : MonoBehaviour {

    [SerializeField] GameObject prefabUpgrade;
    [SerializeField] UpgradeText[] upgradeTexts;
    [SerializeField] UpgradeQueue upgradeQueue;
    string[] upgradeTextTypes;

    // the list of game objects choices presented to the user on this screen
    List<GameObject> upgradeChoices = new List<GameObject>();

    SortedList<string, int> availableUpgradeLevels;

    ObjectManager objects;
    GameObject player;
    StateManager stateManager;
    AttributeManager attributeManager;
    Transform canvasTransform;

    private void Awake() {
        objects = GameObject.Find("RunManager").GetComponent<ObjectManager>();
        player = objects.player;
        stateManager = objects.GetComponent<StateManager>();
        attributeManager = player.GetComponent<AttributeManager>();
        canvasTransform = objects.canvas.transform;

        transform.SetParent(canvasTransform, false);

        upgradeTextTypes = PopulateUpgradeTextsArray(upgradeTexts);
        SortedList<string, int> upgradeSelections = AddQueuedUpgradeChoices();
        upgradeSelections = AddRandomUpgradeChoice(upgradeSelections);
        BuildUpgradeSelection(upgradeSelections);

    }

    void Start() {

        
    }

    // places chosen upgrades in Upgrade prefabs and then places these objects on the screen with the appropriate descriptive text
    void BuildUpgradeSelection(SortedList<string, int> selectedUpgrades) {

        int numUpgrades = selectedUpgrades.Count;
        if (numUpgrades == 0)
            Kill();

        for (int i = 0; i < numUpgrades; i++) {
            string upgradeType = selectedUpgrades.Keys[i];
            int upgradeLevel = selectedUpgrades[upgradeType];

            PlaceUpgradeChoice(i, upgradeType, upgradeLevel);

        }

    }


    void PlaceUpgradeChoice(int i, string upgradeType, int upgradeLevel) {
        upgradeChoices.Add(Instantiate(prefabUpgrade));
        upgradeChoices[i].transform.SetParent(transform, false);
        upgradeChoices[i].transform.localPosition = CalculatePosition(i);

        upgradeChoices[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = upgradeType;
        upgradeChoices[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = GetUpgradeText(upgradeType, upgradeLevel);

        int j = i;
        upgradeChoices[i].GetComponent<Button>().onClick.AddListener(delegate { TaskOnClick(j); });
    }

    Vector3 CalculatePosition(int index) {
        return new Vector3(0, 200 - index * 200);
    }


    // called once in Awake to faciliate retrieval of upgrade description text in stored in scriptable objects
    string[] PopulateUpgradeTextsArray(UpgradeText[] upgradeTexts) {
        string[] result = new string[upgradeTexts.Length];
        for (int i = 0; i < upgradeTexts.Length; i++) {
            result[i] = upgradeTexts[i].upgradeType;
        }

        return result;
    }

    // returns the descriptive text associated with the given upgrade type and level
    // e.g. "+20% damage" for upgradeType = "Damage" and level = 1
    string GetUpgradeText(string upgradeType, int level) {
        return upgradeTexts[Array.IndexOf(upgradeTextTypes, upgradeType)].upgradeTextByLevel[level];
    }

    // executes when one of the upgrade buttons is clicked
    void TaskOnClick(int i) {
        string upgradeChoice = upgradeChoices[i].GetComponentInChildren<TextMeshProUGUI>().text;
        ApplyUpgrade(upgradeChoice);

        Kill();
    }

    void Kill() {
        stateManager.CompleteUpgrade();
        Destroy(gameObject);
    }

    void ApplyUpgrade(string choice) {
        attributeManager.ApplyUpgrade(choice);
    }

    // chooses up to 2 elements from the queue (to ensure every available upgrade is seen periodically),
    // then up to 1 element at random from the remaining available upgrades
    SortedList<string, int> AddQueuedUpgradeChoices() {
        SortedList<string, int> result = new SortedList<string, int>();

        availableUpgradeLevels = attributeManager.GetAvailableUpgradeLevels();
        if (availableUpgradeLevels.Count == 0) return result;

        // add queued choices to the result list
        int max = 2;
        string next;
        bool queueRebuilt = false;

        // build the upgrade queue from all available upgrades if it is empty
        if (upgradeQueue.upgradeNames.Count == 0) {
            BuildQueue(availableUpgradeLevels.Keys.ToList());
            queueRebuilt = true;
        }

        while (upgradeQueue.upgradeNames.Count > 0 && max > 0) {
            max--;
            next = upgradeQueue.upgradeNames[0];
            result.Add(next, availableUpgradeLevels[next]);

            // remove this selection from both the queue and the list of available upgrades
            upgradeQueue.upgradeNames.Remove(next);
            availableUpgradeLevels.Remove(next);

            // if the queue has not already been rebuilt during this selection process, rebuild it from the available upgrades (minus those already selected)
            if (upgradeQueue.upgradeNames.Count == 0 && !queueRebuilt) {
                availableUpgradeLevels = GetAvailableUpgradeLevelsWithExclusions(result);

                BuildQueue(availableUpgradeLevels.Keys.ToList());
                queueRebuilt = true;
            }
        }

        return result;

    }


    // add a single random choice if available
    SortedList<string, int> AddRandomUpgradeChoice(SortedList<string, int> result) {

        availableUpgradeLevels = GetAvailableUpgradeLevelsWithExclusions(result);
        if (availableUpgradeLevels.Count == 0)
            return result;
        else {
            int index = UnityEngine.Random.Range(0, availableUpgradeLevels.Count);
            string upgradeName = availableUpgradeLevels.Keys[index];
            result.Add(upgradeName, availableUpgradeLevels[upgradeName]);

            // we also remove the random selection from the queue if it is present
            if (upgradeQueue.upgradeNames.Contains(upgradeName))
                upgradeQueue.upgradeNames.Remove(upgradeName);

            return result;
        }
    }

    SortedList<string, int> GetAvailableUpgradeLevelsWithExclusions(SortedList<string, int> exclusions) {
        SortedList<string, int> result = attributeManager.GetAvailableUpgradeLevels();
        foreach (string exclusion in exclusions.Keys) {
            if (result.ContainsKey(exclusion)) {
                result.Remove(exclusion);
            }
        }

        return result;
    }

    // shuffles a List<T> by iteratively selecting a random element from among [0, i) and placing it in the ith index
    List<T> ShuffleList<T>(List<T> a) {
        for (int i = a.Count - 1; i > 0; i--) {
            int rnd = UnityEngine.Random.Range(0, i);
            T temp = a[i];

            // Swap values
            a[i] = a[rnd];
            a[rnd] = temp;
        }
        return (a);
    }



    void BuildQueue(List<string> available) {
        upgradeQueue.upgradeNames = ShuffleList(available);
    }
}
