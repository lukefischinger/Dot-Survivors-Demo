using UnityEngine;
using UnityEngine.SceneManagement;

// player component
// tracks player max and current health, and communicates this information with the player health bar display
public class HealthManager : MonoBehaviour {


    ObjectManager objects;
    RunInformation runInformation;
    Bar healthBar;
    Pool damagePool;
    Transform myTransform;

    float maxHealth;
    float currentHealth;
    float currentArmor = 0;
    float currentHealing = 0;
    float healingCooldown = 1f;
    float healingCooldownRemaining = 1f;

    private void Awake() {
        objects = GameObject.Find("RunManager").GetComponent<ObjectManager>();
        damagePool = objects.damagePool.GetComponent<Pool>();
        runInformation = objects.runInformation;

        currentHealth = maxHealth;
        healthBar = transform.GetChild(0).GetComponent<Bar>();
        myTransform = transform;
    }

    private void Start() {
        SetHealthBar();
    }

    private void FixedUpdate() {
        PeriodicHealing();
    }


    public void Damage(float damageAmount, string colorName, bool ignoreArmor = false) {
        float actualDamage = Mathf.Max(0, damageAmount - (ignoreArmor ? 0 : currentArmor));

        currentHealth -= actualDamage;
        currentHealth = Mathf.Max(0, currentHealth);

        SetHealthBar();

        // display damage
        GameObject damageUI = damagePool.GetPooledObject();
        if(damageUI != null)
            damageUI.GetComponent<Damage>().SetDamage(actualDamage, myTransform.position, colorName);

        if (currentHealth <= 0) {
            Kill();
        }
    }

    public void Heal(float healAmount) {
        float actualHeal = Mathf.Min(maxHealth - currentHealth, healAmount);
        currentHealth += actualHeal;
        SetHealthBar();

        // increment run information
        objects.runInformation.healing += actualHeal;


        // display healing
        GameObject damageUI = damagePool.GetPooledObject();
        if(damageUI != null) 
            damageUI.GetComponent<Damage>().SetDamage(actualHeal, myTransform.position, "Green");

    }

    void Kill() {
        runInformation.runStatus = RunInformation.RunStatus.died;
        SceneManager.LoadScene(2);
    }

    public float GetHealth() { return currentHealth; }

    public float GetMaxHealth() { return maxHealth; }

    public void SetMaxHealth(float maxHealth, bool displayHeal = true) {
        if (displayHeal) {
            float increase = maxHealth - this.maxHealth;
            this.maxHealth = maxHealth;
            Heal(increase);
        }
        else {
            currentHealth += maxHealth - this.maxHealth;
            this.maxHealth = maxHealth;
        }
        
    }

    public void SetArmor(float value) {
        currentArmor = value;
    }

    public void SetHealing(float value) {
        currentHealing = value;
    }

    public void SetHealthBar() {
        healthBar.SetFillValue(currentHealth / maxHealth);
    }

    void PeriodicHealing() {
        if (currentHealing == 0)
            return;

        if (healingCooldownRemaining <= 0) {
            Heal(currentHealing);
            healingCooldownRemaining += healingCooldown;
        }
        else {
            healingCooldownRemaining -= Time.deltaTime;
        }
    }
}
