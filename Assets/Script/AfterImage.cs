using UnityEngine;

public class AfterImage : MonoBehaviour
{
 

    public float duration = 0.3f;
    void Start()
    {
        Destroy(gameObject, duration);
    }


}
