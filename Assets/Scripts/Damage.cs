using UnityEngine;
using TMPro;

// displays damage amounts to the user briefly, then disappears
public class Damage : MonoBehaviour
{

    [SerializeField] float timeToDestroy;
    float timeElapsed;
    TextMeshProUGUI text;

    // Start is called before the first frame update
    void Awake()
    {
        timeElapsed = 0;
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
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
        transform.position = position + new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f), 0) ;
        transform.localScale = Vector3.one * size;

    }
}
