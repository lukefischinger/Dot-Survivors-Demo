using UnityEngine;

// player component
// tracks player max and current health, and communicates this information with the player health bar display
public class HealthManager : MonoBehaviour
{
 
    [SerializeField] float maxHealth;
    Bar healthBar;

    float currentHealth;
    float currentArmor = 0;
    float currentHealing = 0;
    float healingCooldown = 1f;
    float healingCooldownRemaining = 1f;

    private void Start() {
        currentHealth = maxHealth;
        healthBar = transform.GetChild(0).GetComponent<Bar>();
        SetHealthBar();
    }

    private void Update() {
        PeriodicHealing();
    }


    public void Damage(float damageAmount, bool ignoreArmor = false) {
        float actualDamage = Mathf.Max(0, damageAmount - (ignoreArmor ? 0 : currentArmor));

        currentHealth -= actualDamage;
        currentHealth = Mathf.Max(0, currentHealth);

        SetHealthBar();

        if (currentHealth <= 0) {
            Kill();
        }
    }

    public void Heal(float healAmount) {
        currentHealth += healAmount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        SetHealthBar();

    }

    void Kill() {
        Destroy(gameObject);
    }

    public float GetHealth() { return currentHealth; }

    public float GetMaxHealth() { return maxHealth;}

    public void SetMaxHealth(float maxHealth) {
        float increase = maxHealth - this.maxHealth;
        this.maxHealth = maxHealth;
        Heal(increase);
    }

    public void SetArmor(float value) {
        currentArmor = value;
    }

    public void SetHealing(float value) {
        currentHealing = value;
    }

    public

    void SetHealthBar() {
        healthBar.SetFillValue(currentHealth / maxHealth);
    }

    void PeriodicHealing() {
        if (currentHealing == 0)
            return;
        
        if(healingCooldownRemaining <= 0) {
            Heal(currentHealing);
            healingCooldownRemaining += healingCooldown;
        } else {
            healingCooldownRemaining -= Time.deltaTime;
        }
    }
}