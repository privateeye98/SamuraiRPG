using UnityEngine;

public class DontDestroyMe : MonoBehaviour
{
    private static DontDestroyMe _instance;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
