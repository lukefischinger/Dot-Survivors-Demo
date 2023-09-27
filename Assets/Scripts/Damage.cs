using UnityEngine;
using TMPro;

// displays damage amounts to the user briefly, then disappears
public class Damage : MonoBehaviour {

    [SerializeField] float timeToDestroy;

    float timeElapsed;

    TextMeshPro text;
    Transform myTransform;
    ObjectManager objects;
    Pool damagePool;


    void Awake() {
        objects = GameObject.Find("RunManager").GetComponent<ObjectManager>();
        damagePool = objects.damagePool.GetComponent<Pool>();

        timeElapsed = 0;
        text = GetComponent<TextMeshPro>();
        myTransform = transform;

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
        myTransform.localScale = Vector3.one * size;
    }

    public Color GetColor(string colorName) {
        switch(colorName) {
            case "White":
                return objects.basicDamageColor;
            case "Red":
                return objects.redDamageColor;
            case "Blue":
                return objects.blueDamageColor;    
            case "Yellow":
                return objects.yellowDamageColor;
            case "Green":
                return Color.green;
            case "Experience":
                return new Color(194f / 255f, 214 / 255f, 1f, 1f);
            default:
                return Color.magenta;
        }
    }
}
