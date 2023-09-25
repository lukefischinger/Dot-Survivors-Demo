using UnityEngine;
using UnityEngine.TerrainUtils;

// stores all objects used in the Run scene for centralized access
public class ObjectManager : MonoBehaviour {

    [SerializeField]
    public GameObject
        player,
        followCamera,
        canvas,
        EnemyGenerator,
        enemyPool,
        experiencePool,
        damagePool,
        explosionPool,
        experienceBar,
        pauseScreen,
        pauseButton,
        upgradeScreen,
        eventSystem,
        clock,
        movementJoystick;

    [SerializeField]
    public Color yellowDamageColor,
                    basicDamageColor,
                    redDamageColor,
                    blueDamageColor;


    [SerializeField] public RunInformation runInformation;

    private void Awake() {
        runInformation.ClearAll();
    }

    public void IncrementDamage(float amount, Color color) {
        runInformation.damage += amount;

        if (color == basicDamageColor)
            runInformation.damageWhite += amount;
        else if (color == yellowDamageColor)
            runInformation.damageYellow += amount;
        else if (color == redDamageColor)
            runInformation.damageRed += amount;
        else if (color == blueDamageColor)
            runInformation.damageBlue += amount;
        else runInformation.damageError += amount;
    }
}
