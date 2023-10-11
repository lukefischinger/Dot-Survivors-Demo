using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    [SerializeField] List<AudioClip> playlist;

    RunInformation runInformation;
    ObjectManager objects;

    const float timeBetweenExplosions = 0.25f;
    const float timeBetweenEnemyHit = 0.05f;
    public const float explosionVolume = 0.15f;
    public const float enemyHitVolume = 0.1f;
    public const float playerHitVolume = 0.2f;
    public const float buttonSelectVolume = 0.3f;
    public const float buttonClickVolume = 1f;

    public bool areUpgradesMaxed = false;
    int currentTrack = 0;
    float timeSinceLastExplosionSound, timeSinceLastEnemyHitSound;
    AudioSource audioSource;

    public float soundVolume => runInformation.soundVolume;
    public float musicVolume => runInformation.musicVolume;

    public bool canHearExplosion => timeSinceLastExplosionSound > timeBetweenExplosions;
    public bool canHearEnemyHit => timeSinceLastEnemyHitSound > timeBetweenEnemyHit;


    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        objects = GetComponent<ObjectManager>();
        runInformation = objects.runInformation;
    }

    private void Update() {
        timeSinceLastExplosionSound += Time.deltaTime;
        timeSinceLastEnemyHitSound += Time.deltaTime;

        CheckPlaylist();
        audioSource.volume = runInformation.musicVolume;
    }


    public void ResetExplosionTimer() {
        timeSinceLastExplosionSound = 0;
    }

    public void ResetEnemyHitTimer() {
        timeSinceLastEnemyHitSound = 0;
    }

    void CheckPlaylist() {
        
        if (audioSource.time >= audioSource.clip.length || (areUpgradesMaxed && currentTrack < playlist.Count - 1)) {
            currentTrack = GetNextTrack();

            audioSource.clip = playlist[currentTrack];
            audioSource.Play();
        }
    }

    int GetNextTrack() {
        if (areUpgradesMaxed)
            return playlist.Count - 1;
        else if (currentTrack == playlist.Count - 2 && !areUpgradesMaxed)
            return 0;
        else return currentTrack + 1;
    }




}
