using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;

public class UpgradeSelection : MonoBehaviour {

    [SerializeField] UpgradeText[] upgradeTexts;
    [SerializeField] UpgradeQueue upgradeQueue;

    string[] upgradeTextTypes;

    // the list of game objects choices presented to the user on this screen
    List<GameObject> upgradeChoices = new List<GameObject>();

    SortedList<string, int> availableUpgradeLevels;

    ObjectManager objects;
    UIManager ui;
    GameObject player;
    StateManager stateManager;
    AttributeManager attributeManager;
    Transform canvasTransform;

    public bool isVisible;

    private void Awake() {
        objects = GameObject.Find("RunManager").GetComponent<ObjectManager>();
        ui = objects.GetComponent<UIManager>();
        player = objects.player;
        stateManager = objects.GetComponent<StateManager>();
        attributeManager = player.GetComponent<AttributeManager>();
        canvasTransform = objects.canvas.transform;
        transform.SetParent(canvasTransform, false);

        PopulateUpgradeChoices();
        upgradeQueue.upgradeNames.Clear();
        EraseAll();
        upgradeTextTypes = PopulateUpgradeTextsArray(upgradeTexts);
    }

    // the upgrade menu is shown and re-hidden for each upgrade selection throughout a run
    // only repopulate if the upgrade selection buttons are blank
    private void Fill() {
        if (IsBlank()) {
            SortedList<string, int> upgradeSelections = AddQueuedUpgradeChoices();
            upgradeSelections = AddRandomUpgradeChoice(upgradeSelections);
            BuildUpgradeSelection(upgradeSelections);
        }

    }

    // populates the existing menu buttons with the appropriate descriptive text
    void BuildUpgradeSelection(SortedList<string, int> selectedUpgrades) {

        int numUpgrades = selectedUpgrades.Count;
        if (numUpgrades == 0)
            Kill();

        for (int i = 0; i < numUpgrades; i++) {
            string upgradeType = selectedUpgrades.Keys[i];
            int upgradeLevel = selectedUpgrades[upgradeType];

            PlaceUpgradeChoice(i, upgradeType, upgradeLevel);

        }

        // disable any unused upgrade objects
        for (int i = numUpgrades; i < 3; i++) {
            upgradeChoices[i].SetActive(false);
        }

    }

    // fills in the text for a single upgrade choice button
    void PlaceUpgradeChoice(int i, string upgradeType, int upgradeLevel) {
        SetUpgradeText(i, upgradeType, GetUpgradeText(upgradeType, upgradeLevel));
        SetUpgradeSprite(i, upgradeType);
    }

    void SetUpgradeText(int i, string title, string description) {
        upgradeChoices[i].SetActive(true);
        upgradeChoices[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = title;
        upgradeChoices[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = description;
    }

    // sets the appropriate sprite for the upgrade type in slot i
    void SetUpgradeSprite(int i, string upgradeType) {
        SpriteRenderer renderer = upgradeChoices[i].transform.GetChild(2).GetComponent<SpriteRenderer>();
        if (ui.colorNames.Contains(upgradeType)) {
            renderer.sprite = ui.sprites[ui.spriteNames.IndexOf("Weapon")];
            renderer.color = ui.colors[ui.colorNames.IndexOf(upgradeType)];
        }
        else {
            renderer.sprite = ui.sprites[ui.spriteNames.IndexOf(upgradeType)];
            renderer.color = Color.white;
        }
    }

    void EraseAll() {
        for (int i = 0; i < 3; i++) {
            SetUpgradeText(i, "", "");
        }
    }

    bool IsBlank() {
        return upgradeChoices.Count > 0 && upgradeChoices[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text == "";
    }

    // populates the list upgradeChoices for later use, and adds button listeners
    void PopulateUpgradeChoices() {
        for (int i = 0; i < 3; i++) {
            upgradeChoices.Add(transform.GetChild(i).gameObject);
        }
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
    public void TaskOnClick(int i) {
        string upgradeChoice = upgradeChoices[i].GetComponentInChildren<TextMeshProUGUI>().text;
        ApplyUpgrade(upgradeChoice);

        Kill();
    }

    void Kill() {
        stateManager.CompleteUpgrade();
        EraseAll();
        SetVisible(false);
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


        bool queueRebuilt = false;

        // build the upgrade queue from all available upgrades if it is empty
        if (upgradeQueue.upgradeNames.Count == 0) {
            BuildQueue(availableUpgradeLevels.Keys.ToList());
            queueRebuilt = true;
        }

        int max = 2;
        string next;
        while (upgradeQueue.upgradeNames.Count > 0 && max > 0) {
            next = upgradeQueue.upgradeNames[0];
            upgradeQueue.upgradeNames.Remove(next);

            if (availableUpgradeLevels.Keys.Contains(next)) {
                max--;
                result.Add(next, availableUpgradeLevels[next]);

                // remove this selection from both the queue and the list of available upgrades
                availableUpgradeLevels.Remove(next);
            }

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
        return a;
    }



    void BuildQueue(List<string> available) {
        upgradeQueue.upgradeNames = ShuffleList(available);
    }


    public void SetVisible(bool visible) {
        if(visible)
            Fill();

        isVisible = visible;

        foreach (SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>()) {
            sprite.enabled = visible;
        }
        foreach (Image image in GetComponentsInChildren<Image>()) {
            image.enabled = visible;
        }
        foreach (TextMeshProUGUI tmp in GetComponentsInChildren<TextMeshProUGUI>()) {
            tmp.enabled = visible;
        }
        foreach(Selectable selectable in GetComponentsInChildren<Selectable>()) {
            selectable.enabled = visible;
        }
        foreach (ButtonSelect buttonSelect in GetComponentsInChildren<ButtonSelect>()) {
            buttonSelect.enabled = visible;
        }
    }

}
