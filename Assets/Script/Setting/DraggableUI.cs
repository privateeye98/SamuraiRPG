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
            Debug.LogError("DragWindow: Canvas�� �����ϴ�.");
    }

    // ���콺�� �巡���� ������ ȣ��
    public void OnDrag(PointerEventData eventData)
    {
        if (_canvas == null) return;
        // ĵ���� ������ ����
        float scale = _canvas.scaleFactor;
        // delta�� anchoredPosition�� ���� �ε巴�� �̵�
        _rect.anchoredPosition += eventData.delta / scale;
    }
}