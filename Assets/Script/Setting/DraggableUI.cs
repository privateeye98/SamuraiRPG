using UnityEngine;
using UnityEngine.EventSystems;


public class DraggableUI : MonoBehaviour, IDragHandler
{
    private RectTransform _rect;
    private Canvas _canvas;

    void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        if (_canvas == null)
            Debug.LogError("DragWindow: Canvas가 없습니다.");
    }

    // 마우스를 드래그할 때마다 호출
    public void OnDrag(PointerEventData eventData)
    {
        if (_canvas == null) return;
        // 캔버스 스케일 보정
        float scale = _canvas.scaleFactor;
        // delta를 anchoredPosition에 더해 부드럽게 이동
        _rect.anchoredPosition += eventData.delta / scale;
    }
}