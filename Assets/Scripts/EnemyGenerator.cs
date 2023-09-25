using System;
using System.Collections.Generic;
using UnityEngine;

// generates enemies at an increasing rate throughout a run
public class EnemyGenerator : MonoBehaviour {
    [SerializeField] GameObject prefabEnemy;
    [SerializeField] List<Sprite> sprites;

    private static readonly float[] healths = new float[7] { 30f, 60f, 100f, 180f, 300f, 500f, 800f };
    private static readonly float[] damages = new float[7] { 4f, 6f, 8f, 10f, 13f, 16f, 20f };
    private static readonly float[] timings = new float[7] { 60f, 120f, 180f, 240f, 300f, 360f, Mathf.Infinity };

    int level = 0;

    ObjectManager objects;

    Clock clock;
    Pool enemyPool;
    Camera cam;


    // variables for determining when a new enemy should be created
    float timeElapsed;
    float lastEnemyTime;
    float enemiesPerSecond, rate;
    int numberPerInterval = 1;
    float enemyCarryOver = 0;

    const float exponent = 1.25f;
    const float rateMultiplier = 0.03f;
    const float rateConstant = 1.2f;

    private void Awake() {
        objects = GameObject.Find("RunManager").GetComponent<ObjectManager>();
        clock = objects.clock.GetComponent<Clock>();

        CalculateRate();
        lastEnemyTime = -rate;
        cam = Camera.main;

        enemyPool = objects.enemyPool.GetComponent<Pool>();
    }

    private void Update() {
        UpdateLevel();
        CalculateRate();
        CreateEnemies();
    }

    void UpdateLevel() {
        level = Array.BinarySearch(timings, (int)clock.timeSpan.TotalSeconds);
        if (level < 0)
            level = ~level; // apply bitwise complement operator to get first upper bound, per function documentation

    }

    // updates "rate" field with the rate at which enemies are generated
    // rate increases over time
    void CalculateRate() {
        timeElapsed = Time.time - clock.timeAwake; // connect this to future runtime variable that accounts for pauses

        enemiesPerSecond = rateMultiplier * Mathf.Pow(timeElapsed, exponent) + rateConstant;
        rate = 1 / enemiesPerSecond;
    }

    void CreateEnemies() {
        if (timeElapsed > lastEnemyTime + rate) {
            // if rate is small enough, we need to generate multiple enemies per Update
            float trueNumberPerInterval = (timeElapsed - lastEnemyTime) / rate + enemyCarryOver;
            numberPerInterval = Mathf.FloorToInt(trueNumberPerInterval);
            enemyCarryOver = trueNumberPerInterval - numberPerInterval;

            // create the calculated number of enemies in random locations around the border of the screen
            for (int i = 0; i < numberPerInterval; i++) {
                CreateEnemy();
            }

            lastEnemyTime = Time.time - clock.timeAwake;
        }

    }
    void CreateEnemy() {
        GameObject enemy = enemyPool.GetPooledObject();
        if (enemy != null) {
            enemy.transform.position = GetRandomSpawnLocation();
            enemy.GetComponent<Enemy>().ResetEnemy(healths[level], damages[level], sprites[level]);

        }
    }


    // returns a random position in world space on the edge of the screen
    public Vector3 GetRandomSpawnLocation() {
        int t = UnityEngine.Random.Range(0, 2);     // 0 => randomize the y coord, 1 => randomize the x coord
        int q = UnityEngine.Random.Range(0, 2);     // the value of the unrandomized coord--0 is left/bottom, 1 is right/top
        float r = UnityEngine.Random.value;         // the value of the randomized coord

        Vector3 screenPosition = new Vector3((t * r + (1f - t) * q) * Screen.width, ((1f - t) * r + t * q) * Screen.height, 0f);
        Vector3 positionCreated = cam.ScreenToWorldPoint(screenPosition);
        positionCreated.z = 0;
        return (positionCreated);
    }



}
