using UnityEngine;
using UnityEngine.UI;

public class DragItemUI : MonoBehaviour
{
    public static DragItemUI instance;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image icon;

     void Awake() => instance = this;

    public void Show(Sprite sprite)
    {
        icon.sprite = sprite;
        icon.enabled = true;
        canvasGroup.alpha = 1;
    }

    public void Hide()
    {
        icon.enabled = false;
        canvasGroup.alpha = 0;
    }

     void Update()
    {
        transform.position = Input.mousePosition;
    }

}
