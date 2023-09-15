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
        experienceBar,
        pause;

}
