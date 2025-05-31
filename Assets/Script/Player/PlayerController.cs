using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}
