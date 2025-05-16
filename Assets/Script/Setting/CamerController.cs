using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("���")]
    public Transform target;          // ���� �÷��̾� Transform

    [Header("�߰� ������")]
    public Vector3 offset = new Vector3(0, 1.5f, -10f);

    [Header("�ε巴�� ���󰡱�")]
    [Range(0f, 1f)]
    public float smooth = 0.15f;      // 0 �� �������� ī�޶� �ٷ� �ٰ�, 1 �� �������� ������ �̵�

    Vector3 _velocity = Vector3.zero; // ���� ����

    void LateUpdate()
    {
        if (!target) return;

        // 1) ��ǥ ��ġ ��� (z �� ����)
        Vector3 desired = target.position + offset;

        // 2) �ε巴�� ����
        transform.position = Vector3.SmoothDamp(transform.position,
                                                desired,
                                                ref _velocity,
                                                smooth);
    }
}
