using UnityEngine;

public class DontDestroyMe : MonoBehaviour
{
    void Awake()
    {
        Debug.Log("���� ����ִ�");
        DontDestroyOnLoad(gameObject);
    }

}
