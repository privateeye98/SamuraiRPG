using UnityEngine;

public class DamageTextSpawner : MonoBehaviour
{
    public static DamageTextSpawner I;

    [SerializeField] GameObject damageTextPrefab;
    [SerializeField] Canvas canvas;

    private static DamageTextSpawner instance;

    void Awake()
    {
        if (I != null && I != this)
        {
            Destroy(gameObject);  // 이미 있는 인스턴스와 다르면 제거
            return;
        }

        I = this;
        DontDestroyOnLoad(gameObject);

    }
    public void Spawn(int damage, Vector3 worldPos, bool isCritical)
    {
        if (damageTextPrefab == null || canvas == null)
        {
            Debug.LogWarning("DamageTextSpawner: 프리팹이나 캔버스가 비어있습니다.");
            return;
        }

        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        GameObject dmgObj = Instantiate(damageTextPrefab);
        dmgObj.transform.SetParent(canvas.transform, false);
        dmgObj.GetComponent<RectTransform>().position = screenPos;

        DamageText txt = dmgObj.GetComponent<DamageText>();
        txt.Init(damage, isCritical);
    }
    public void SpawnText(string message, Vector3 worldPos)
    {
        if (damageTextPrefab == null || canvas == null)
        {
            Debug.LogWarning("DamageTextSpawner: 프리팹이나 캔버스가 비어있습니다.");
            return;
        }

        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        GameObject obj = Instantiate(damageTextPrefab);
        obj.transform.SetParent(canvas.transform, false);
        obj.GetComponent<RectTransform>().position = screenPos;

        DamageText txt = obj.GetComponent<DamageText>();
        txt.Init(message); // 이거를 위한 Init(string message) 함수가 DamageText.cs에 있어야 함
    }


}