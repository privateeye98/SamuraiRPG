using UnityEngine;

public class DamageTextSpawner : MonoBehaviour
{
    public static DamageTextSpawner I;

    [SerializeField] GameObject damageTextPrefab;
    [SerializeField] Canvas canvas;

    void Awake()
    {
        I = this;
    }

    public void Spawn(int damage, Vector3 worldPos, bool isCritical)
    {
        if (damageTextPrefab == null || canvas == null)
        {
            Debug.LogError("❌ DamageTextSpawner 설정이 안 되어 있음.");
            return;
        }

        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        GameObject go = Instantiate(damageTextPrefab, canvas.transform);
        RectTransform rect = go.GetComponent<RectTransform>();
        rect.position = screenPos;

        DamageText dt = go.GetComponent<DamageText>();
        dt.Init(damage, isCritical);
    }
}
