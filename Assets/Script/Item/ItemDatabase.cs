using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;

    [Header("��ϵ� ��� ������")]
    public List<ItemData> items;

    private Dictionary<int, ItemData> itemDict = new();

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        foreach (var item in items)
        {
            if (item != null)
                itemDict[item.id] = item;
        }
    }

    public ItemData GetItemById(int id)
    {
        if (itemDict.TryGetValue(id, out var item))
            return item;
        return null;
    }
}
