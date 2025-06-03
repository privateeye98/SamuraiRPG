using UnityEngine;

public class MainCameraSingleton : MonoBehaviour
{
    private static MainCameraSingleton instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); 
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
