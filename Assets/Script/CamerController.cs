using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("대상")]
    public Transform target;          // 따라갈 플레이어 Transform

    [Header("추가 오프셋")]
    public Vector3 offset = new Vector3(0, 1.5f, -10f);

    [Header("부드럽게 따라가기")]
    [Range(0f, 1f)]
    public float smooth = 0.15f;      // 0 에 가까울수록 카메라가 바로 붙고, 1 에 가까울수록 느리게 이동

    Vector3 _velocity = Vector3.zero; // 내부 계산용

    void LateUpdate()
    {
        if (!target) return;

        // 1) 목표 위치 계산 (z 는 고정)
        Vector3 desired = target.position + offset;

        // 2) 부드럽게 보간
        transform.position = Vector3.SmoothDamp(transform.position,
                                                desired,
                                                ref _velocity,
                                                smooth);
    }
}
