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
            Debug.LogError("❌ DamageTextSpawner 설정이 안 되어 있음.");
            return;
        }

        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);


        GameObject dmgObj = Instantiate(damageTextPrefab); // 에셋에서 인스턴스를 만들고
        dmgObj.transform.SetParent(canvas.transform, false); // 
        dmgObj.GetComponent<RectTransform>().position = screenPos; // 

        dmgObj.GetComponent<DamageText>().Init(damage, isCritical); //

    }
}
