using UnityEngine;
using UnityEngine.EventSystems;


public class DraggableUI : MonoBehaviour, IDragHandler
{
     RectTransform _rect;
     Canvas _canvas;

    void Awake()
    {
        _rect = GetComponent<RectTransform>();

        Transform t = transform;
        Canvas found = null;
        Debug.Log($"[DraggableUI] Awake 시작: GameObject = {gameObject.name}");
        while (t != null)
        {
            Canvas c = t.GetComponent<Canvas>();
            Debug.Log($"  부모 계층 확인: '{t.name}' (Active={t.gameObject.activeInHierarchy}) 에 Canvas={(c != null ? "있음" : "없음")}");
            if (c != null) found = c;
            t = t.parent;
        }

        _canvas = GetComponentInParent<Canvas>();
        if (_canvas == null)
            Debug.LogError("DragWindow: Canvas가 없습니다.");
    }

    // 마우스를 드래그할 때마다 호출
    public void OnDrag(PointerEventData eventData)
    {
        if (_canvas == null) return;
        float scale = _canvas.scaleFactor;
        _rect.anchoredPosition += eventData.delta / scale;
    }
}