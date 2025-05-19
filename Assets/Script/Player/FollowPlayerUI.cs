using UnityEngine;

public class PlayerUIFollow : MonoBehaviour
{
    public Transform target; // 플레이어
    public Vector3 offset = new Vector3(0, -1f, 0); // 캐릭터 밑 위치
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