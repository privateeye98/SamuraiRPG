using UnityEngine;

public class MainCameraSingleton : MonoBehaviour
{
    private static MainCameraSingleton instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // 중복 카메라 제거
            return;
        }

        instance = this;
    }
}
