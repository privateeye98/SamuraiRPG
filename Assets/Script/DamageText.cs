using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] float floatSpeed = 30f;
    [SerializeField] float duration = 1f;

    Vector3 moveDir = Vector3.up;
    float timeElapsed = 0f;
    bool isCritical = false;
    public void Init(string message)
    {
        isCritical = false;

        text.text = message;
        text.fontSize = 40;
        text.color = Color.gray;
        text.alpha = 1f;
        text.enabled = true;
        text.gameObject.SetActive(true);

        transform.localScale = Vector3.one;

        Destroy(gameObject, duration);
    }

    public void Init(int damage, bool isCritical)
    {
        this.isCritical = isCritical;

        text.text = damage.ToString();
        text.fontSize = isCritical ? 60 : 40;
        text.color = isCritical ? Color.red : Color.white;
        text.alpha = 1f;
        text.enabled = true;
        text.gameObject.SetActive(true);

        transform.localScale = Vector3.one;

        Destroy(gameObject, duration);
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        // 🎯 1. 위로 부드럽게 이동
        transform.position += moveDir * floatSpeed * Time.deltaTime;
        floatSpeed = Mathf.Lerp(floatSpeed, 0, Time.deltaTime * 2f);

        // 🎯 2. 알파값 계산
        float alpha = Mathf.Lerp(1f, 0f, timeElapsed / duration);

        // 🎯 3. 크리티컬이면 점멸 + 알파 적용
        if (isCritical)
        {
            float flash = Mathf.PingPong(timeElapsed * 8f, 1f); // 0~1 반복
            Color flashColor = Color.Lerp(Color.red, Color.white, flash);
            flashColor.a = alpha;
            text.color = flashColor;
        }
        else
        {
            Color c = text.color;
            c.a = alpha;
            text.color = c;
        }
    }
}
