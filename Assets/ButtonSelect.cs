using UnityEngine;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour
{

    Button button;

    private void Awake() {
        button = GetComponent<Button>();    
    }
    public void OnHover() {
        button.Select();
    }
}
