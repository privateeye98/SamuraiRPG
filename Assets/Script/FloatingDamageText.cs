using UnityEngine;
using TMPro;

public class FloatingDamageText : MonoBehaviour
{
    public TMP_Text damageText;
    public float floatSpeed = 30f;
    public float lifetime = 1f;

    void Update()
    {
        transform.Translate(Vector3.up * floatSpeed * Time.deltaTime);
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
            Destroy(gameObject);
    }

    public void SetDamage(int damage)
    {
        damageText.text = damage.ToString();
    }
}
