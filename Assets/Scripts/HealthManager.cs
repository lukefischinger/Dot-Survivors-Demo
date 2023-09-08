using Unity.VisualScripting;
using UnityEngine;

// player component
// tracks player max and current health, and communicartes this information with the player health bar display
public class HealthManager : MonoBehaviour
{
    [SerializeField] float maxHealth;
    float currentHealth;
    Bar healthBar;


    private void Start() {
        currentHealth = maxHealth;
        healthBar = transform.GetChild(0).GetComponent<Bar>();
        SetHealthBar();
    }
    public void Damage(float damageAmount, float armorAmount = 0, bool ignoreArmor = false) {
        float actualDamage = Mathf.Max(0, damageAmount - (ignoreArmor ? 0 : armorAmount));

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

    public void SetMaxHealth(float maxHealth) { this.maxHealth = maxHealth; }

    void SetHealthBar() {
        healthBar.SetFillValue(currentHealth / maxHealth);
    }
}
