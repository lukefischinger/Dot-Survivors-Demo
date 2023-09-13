using UnityEngine;
using TMPro;

// displays damage amounts to the user briefly, then disappears
public class Damage : MonoBehaviour
{

    [SerializeField] float timeToDestroy;
    float timeElapsed;
    TextMeshProUGUI text;
    Transform myTransform;

    void Awake()
    {
        timeElapsed = 0;
        text = GetComponent<TextMeshProUGUI>();
        myTransform = transform;
    }

    void Update()
    {
        if(timeElapsed > timeToDestroy)
        {
            Destroy(gameObject);
        } else
        {
            timeElapsed += Time.deltaTime;
        }
    }


    public void SetDamage(float damage, Vector3 position, Color color, float size = 1f)
    {
        timeElapsed = 0;
        text.text = "" + Mathf.Round(damage * 10f) / 10f;
        text.color = color;
        myTransform.position = position + new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f), 0) ;
        myTransform.localScale = Vector3.one * size;

    }
}
