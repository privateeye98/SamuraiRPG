using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    Vector3 initialPosition;

    void Awake()
    {
        instance = this;
        initialPosition = transform.position;
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * magnitude;

            transform.position = initialPosition + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = initialPosition;
    }

}
