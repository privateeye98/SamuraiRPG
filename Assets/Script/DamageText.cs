using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] float floatSpeed = 30f;
    [SerializeField] float duration = 1f;

    Vector3 moveDir = Vector3.up;

    public void Init(int damage, bool isCritical)
    {
        Debug.Log($"[텍스트 설정됨] {damage}");
        text.text = damage.ToString();
        text.color = isCritical ? Color.red : Color.white;
        text.fontSize = 40;
        text.alpha = 1f;
        text.enabled = true;
        text.gameObject.SetActive(true);

        transform.localScale = Vector3.one;

        Destroy(gameObject, duration);
    }

    void Update()
    {
        transform.position += moveDir * floatSpeed * Time.deltaTime;
    }
}
