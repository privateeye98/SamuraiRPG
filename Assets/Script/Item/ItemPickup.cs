using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Collider2D))]
public class ItemPickup : MonoBehaviour
{

    [Tooltip("ȹ���� ������ ������(ScriptableObject)")]
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

        // �κ��丮�� �߰� �õ�
        if (Inventory.instance.AddItem(itemData))
        {
            Debug.Log($"{itemData.itemName} ȹ��!");
            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("�κ��丮 �߰� ����: ���� ���� Ȥ�� ���� �ִ�ġ ����");
        }
    }
}
