using UnityEngine;

public class MiniMapIcon : MonoBehaviour
{
    public RectTransform iconUI;
    public Transform target;
    public RectTransform miniMapPanel;
    public Vector2 worldSize;

    void Update()
    {
        if (target == null) return;

        Vector2 relativePos = (Vector2)target.position / worldSize;
        iconUI.anchoredPosition = new Vector2(
            relativePos.x * miniMapPanel.rect.width,
            relativePos.y * miniMapPanel.rect.height
        );
    }
}
