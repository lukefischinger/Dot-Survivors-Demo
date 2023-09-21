using UnityEngine;

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
        upgradeScreen,
        eventSystem,
        clock;

    [SerializeField] public Color yellowDamageColor,
                    basicDamageColor,
                    redDamageColor,
                    blueDamageColor;


    [SerializeField] RunInformation runInformation;

    private void Awake() {
        runInformation.ClearAll();
    }
}
