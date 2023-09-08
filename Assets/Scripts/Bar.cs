using UnityEngine;
using UnityEngine.UIElements;

public class Bar : MonoBehaviour
{

    [SerializeField] Color color;
    [SerializeField] Vector3 scale;

    float percentFull = 1;
    Transform parentTransform;
    RectTransform myRectTransform, fillTransform, emptyTransform;

    private void Start()
    {
        parentTransform = transform.parent;
        emptyTransform = transform.GetChild(0).GetComponent<RectTransform>();
        fillTransform = transform.GetChild(1).GetComponent<RectTransform>();
        myRectTransform = GetComponent<RectTransform>();

        fillTransform.GetComponent<SpriteRenderer>().color = this.color;
        emptyTransform.GetComponent<SpriteRenderer>().color = Color.white;
    }


    public void SetFillValue(float current)
    {
        percentFull = current;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        fillTransform.localScale = new Vector3(percentFull, 1, 0);
        emptyTransform.localScale = new Vector3(1 - percentFull, 1, 0);
        fillTransform.anchoredPosition = new Vector3(-0.5f + 0.5f * percentFull, 0, 0);
        emptyTransform.anchoredPosition = new Vector3(0.5f * percentFull, 0, 0);

    }

    private void Update() {
        Rotate();
    }

    void Rotate() {
        myRectTransform.localRotation = Quaternion.Euler(0, 0, -parentTransform.eulerAngles.z);
    }

}
