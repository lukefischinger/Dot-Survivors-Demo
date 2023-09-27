using UnityEngine;

// used both for the player health bar and for the experience bar
public class Bar : MonoBehaviour
{

    [SerializeField] Color color;

    float percentFull = 0;
    Transform parentTransform;
    RectTransform myRectTransform, fillTransform, emptyTransform;

    private void Awake()
    {
        parentTransform = transform.parent;

        emptyTransform = transform.GetChild(0).GetComponent<RectTransform>();
        fillTransform = transform.GetChild(1).GetComponent<RectTransform>();
        myRectTransform = GetComponent<RectTransform>();

        myRectTransform.SetParent(parentTransform, false);

        fillTransform.GetComponent<SpriteRenderer>().color = this.color;
        emptyTransform.GetComponent<SpriteRenderer>().color = Color.white;
        UpdateDisplay();

    }


    public void SetFillValue(float current)
    {
        percentFull = current;
        UpdateDisplay();
    }
    
    public void SetScale(Vector3 value) {
        myRectTransform.localScale = value;
    }

    // update the display to reflect the current value of percentFull
    private void UpdateDisplay()
    {
        fillTransform.localScale = new Vector3(percentFull, 1, 0);
        emptyTransform.localScale = new Vector3(1 - percentFull, 1, 0);
        fillTransform.anchoredPosition = new Vector3(-0.5f + 0.5f * percentFull, 0, 0);
        emptyTransform.anchoredPosition = new Vector3(0.5f * percentFull, 0, 0);


    }

    public void Rotate() {
        myRectTransform.localRotation = Quaternion.Euler(0, 0, -parentTransform.eulerAngles.z);
    }


}
