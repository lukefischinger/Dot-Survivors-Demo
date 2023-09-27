using UnityEngine;

// stores all objects used in the Run scene for centralized access
public class ObjectManager : MonoBehaviour {

    [SerializeField]
    public GameObject
        player,
        followCamera,
        canvas,
        enemyGenerator,
        enemyPool,
        experiencePool,
        damagePool,
        explosionPool,
        experienceBar,
        pauseScreen,
        optionsScreen,
        pauseButton,
        upgradeDisplay,
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


}
