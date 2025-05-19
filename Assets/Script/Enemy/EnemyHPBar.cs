using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] float floatSpeed = 20f;
    [SerializeField] float duration = 1.5f;

    Vector3 moveDir = Vector3.up;

    public void Init(float current, float max)
    {
        slider.maxValue = max;
        slider.value = current;

        Destroy(gameObject, duration);
    }

    void Update()
    {
        transform.position += moveDir * floatSpeed * Time.deltaTime;
    }
}
