using UnityEngine;

public class PlayerUIFollow : MonoBehaviour
{
    public Transform target; // �÷��̾�
    public Vector3 offset = new Vector3(0, -1f, 0); // ĳ���� �� ��ġ
    RectTransform rt;

    void Start()
    {
        rt = GetComponent<RectTransform>();
        target = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (target == null) return;
        Vector3 worldPos = target.position + offset;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        rt.position = screenPos;
    }
}