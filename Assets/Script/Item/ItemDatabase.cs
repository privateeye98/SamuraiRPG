using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;

    [Header("등록된 모든 아이템")]
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
        return items.Find(item => item.id == id);
    }
}
