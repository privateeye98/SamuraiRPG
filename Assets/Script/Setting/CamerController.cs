using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("대상")]
    public Transform target;

    [Header("추가 오프셋")]
    public Vector3 offset = new Vector3(0, 1.5f, -10f);

    [Header("부드럽게 따라가기")]
    [Range(0f, 1f)]
    public float smooth = 0.15f;

    [Header("맵 제한")]
    public Vector2 minPosition;
    public Vector2 maxPosition;

    Vector3 _velocity = Vector3.zero;

    void LateUpdate()
    {
        if (!target) return;

        Vector3 desired = target.position + offset;

        // 카메라 반지름만큼 Clamp
        float vertExtent = Camera.main.orthographicSize;
        float horzExtent = vertExtent * Screen.width / Screen.height;

        float leftBound = minPosition.x + horzExtent;
        float rightBound = maxPosition.x - horzExtent;
        float bottomBound = minPosition.y + vertExtent;
        float topBound = maxPosition.y - vertExtent;

        desired.x = Mathf.Clamp(desired.x, leftBound, rightBound);
        desired.y = Mathf.Clamp(desired.y, bottomBound, topBound);

        transform.position = Vector3.SmoothDamp(transform.position,
                                                desired,
                                                ref _velocity,
                                                smooth);
    }
}
