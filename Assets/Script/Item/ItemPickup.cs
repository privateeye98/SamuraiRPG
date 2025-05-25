using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Collider2D))]
public class ItemPickup : MonoBehaviour
{

    [Tooltip("획득할 아이템 데이터(ScriptableObject)")]
    private ItemData _itemData;
    public ItemData itemData => _itemData;
    private SpriteRenderer _sr;
    private Collider2D _col;


    private void Awake()
    {
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;

        _sr = GetComponent<SpriteRenderer>();
        _col = GetComponent<Collider2D>();
        _col.isTrigger = true;
    }
    public void Initiallize(ItemData data)
    {
        _itemData = data;
        _sr.sprite = itemData.icon;

        StartCoroutine(PopAndSeetle());
    }
    private IEnumerator PopAndSeetle()
    {
        Vector3 startPos = transform.position;
        Vector3 peakpos = startPos + Vector3.up * 1f;

        float upDuration = 0.2f;
        float downDuration = 0.2f;

        float t = 0f;

        while (t < upDuration)
        {
           float ratio = t / upDuration;
           transform.position = Vector3.Lerp(startPos, peakpos, ratio);
            t += Time.deltaTime;
            yield return null;
        }

        transform.position = peakpos;

        t = 0f;

        while (t < downDuration)
        {
            float ratio = t / downDuration;
            transform.position = Vector3.Lerp(peakpos, startPos, ratio);
            t += Time.deltaTime;
            yield return null;
        }
        transform.position = startPos;

    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // 인벤토리에 추가 시도
        if (Inventory.instance.AddItem(itemData))
        {
            Debug.Log($"{itemData.itemName} 획득!");
            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("인벤토리 추가 실패: 공간 부족 혹은 스택 최대치 도달");
        }
    }
}
