using UnityEngine;

public class SupervisionManager : MonoBehaviour
{
    private static SupervisionManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject); // �̹� �����ϸ� ���� ���� �� ����
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}