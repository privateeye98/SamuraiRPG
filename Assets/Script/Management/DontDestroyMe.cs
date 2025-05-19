using UnityEngine;

public class DontDestroyMe : MonoBehaviour
{
    void Awake()
    {
        Debug.Log("나는 살아있다");
        DontDestroyOnLoad(gameObject);
    }

}
