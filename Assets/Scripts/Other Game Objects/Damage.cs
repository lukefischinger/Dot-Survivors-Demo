using UnityEngine;
using TMPro;
using System.Runtime.InteropServices.WindowsRuntime;

// displays damage amounts to the user briefly, then disappears
public class Damage : MonoBehaviour {

    [SerializeField] float timeToDestroy;
    [SerializeField] AudioClip 
        experienceAudio,
        playerDamageAudio,
        enemyDamageAudio;

    float timeElapsed;

    TextMeshPro text;
    Transform myTransform;
    ObjectManager objects;
    AudioManager audioManager;
    UIManager ui;
    Pool damagePool;
    AudioSource audioSource;


    void Awake() {
        objects = GameObject.Find("RunManager").GetComponent<ObjectManager>();
        audioManager = objects.GetComponent<AudioManager>();
        damagePool = objects.damagePool.GetComponent<Pool>();
        ui = objects.GetComponent<UIManager>();

        timeElapsed = 0;
        text = GetComponent<TextMeshPro>();
        myTransform = transform;
        audioSource = GetComponent<AudioSource>();

    }

    void Update() {
        if (timeElapsed > timeToDestroy) {
            damagePool.ReturnPooledObject(gameObject);
        }
        else {
            timeElapsed += Time.deltaTime;
        }
    }


    public void SetDamage(float damage, Vector3 position, string colorName, float size = 1f) {
        timeElapsed = 0;
        text.text = "" + ((damage < 1f) ? Mathf.Round(damage * 10) / 10 : Mathf.Round(damage));
        if (colorName == "Experience")
            text.text = "+" + text.text;
        text.color = GetColor(colorName);
        myTransform.position = position + new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f), 0);
        myTransform.localScale = Vector3.one * size * ui.damageScale;

        PlayAudioClip(colorName);

    }

    public Color GetColor(string colorName) {
        return colorName switch {
            "White" => objects.basicDamageColor,
            "Red" => objects.redDamageColor,
            "Blue" => objects.blueDamageColor,
            "Yellow" => objects.yellowDamageColor,
            "Green" => Color.green,
            "Experience" => new Color(194f / 255f, 214 / 255f, 1f, 1f),
            "Player Red" => Color.red,
            _ => Color.magenta
        };
    }

    void PlayAudioClip(string colorName) {
        audioSource.volume = audioManager.soundVolume;
        audioSource.Stop();

        switch (colorName) {
            case ("Experience"):
                audioSource.clip = experienceAudio;
                break;

            case ("White"):
                if (!audioManager.canHearEnemyHit) return;
                else audioManager.ResetEnemyHitTimer();

                audioSource.clip = enemyDamageAudio;
                audioSource.volume *= AudioManager.enemyHitVolume;
                break;

            case ("Player Red"):
                audioSource.clip = playerDamageAudio;
                audioSource.volume *= AudioManager.playerHitVolume;
                break;

            default:
                audioSource.clip = null;
                return;
        }

        audioSource.Play();

    }
}
