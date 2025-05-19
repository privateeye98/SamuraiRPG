using UnityEngine;

public class SupervisionManager : MonoBehaviour
{
    private static SupervisionManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject); // 이미 존재하면 새로 생긴 건 제거
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}