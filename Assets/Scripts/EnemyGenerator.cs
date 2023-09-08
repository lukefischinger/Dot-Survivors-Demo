using UnityEngine;

// generates enemies at an increasing rate throughout a run
public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] GameObject prefabEnemy;


    Camera cam;

    // variables for determining when a new enemy should be created
    float timeElapsed;
    float lastEnemyTime;
    float enemiesPerSecond, rate;
    int numberPerInterval = 1;
    float enemyCarryOver = 0;

    // constants determined through trial and error
    const float exponent = 1.2f;
    const float rateMultiplier = 0.02f;
    const float rateConstant = 1.2f;

    private void Start() {
        CalculateRate();
        lastEnemyTime = -rate;
        cam = Camera.main;
    }

    private void Update() {
        CalculateRate();
        CreateEnemies();
    }


    // updates "rate" field with the rate at which enemies are generated
    // rate increases over time
    void CalculateRate() {
        timeElapsed = Time.time; // connect this to future runtime variable that accounts for pauses

        enemiesPerSecond = rateMultiplier * Mathf.Pow(timeElapsed, exponent) + rateConstant;
        rate = 1 / enemiesPerSecond;     
    }

    void CreateEnemies() {
        if(timeElapsed > lastEnemyTime + rate) {
            // if rate is small enough, we may need to generate multiple enemies per Update
            float trueNumberPerInterval = (timeElapsed - lastEnemyTime) / rate + enemyCarryOver;
            numberPerInterval = Mathf.FloorToInt(trueNumberPerInterval);
            enemyCarryOver = trueNumberPerInterval - numberPerInterval;
    
            // create the calculated number of enemies in random locations around the border of the screen
            for(int i = 0; i < numberPerInterval; i++) {
                GameObject enemy = Instantiate(prefabEnemy);
                enemy.transform.position = GetRandomSpawnLocation();
                enemy.GetComponent<Enemy>().enabled = true;
            }

            lastEnemyTime = Time.time;
        }

    }

    // returns a random position in world space on the edge of the screen
    public Vector3 GetRandomSpawnLocation() {
        int t = Random.Range(0, 2);     // 0 => randomize the y coord, 1 => randomize the x coord
        int q = Random.Range(0, 2);     // the value of the unrandomized coord--i.e., 0 is left/bottom, 1 is right/top
        float r = Random.value;         // the position of the randomized coord

        Vector3 screenPosition = new Vector3((t * r + (1f - t) * q) * Screen.width, ((1f - t) * r + t * q) * Screen.height, 0f);
        Vector3 positionCreated = cam.ScreenToWorldPoint(screenPosition);
        positionCreated.z = 0;
        return (positionCreated);
    }

}
