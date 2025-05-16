using UnityEngine;
using System.Collections.Generic;


public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public List<ItemData> items = new List<ItemData>();
    public int capacity = 9;

    public delegate void OnItemChanged();
    public event OnItemChanged onItemChangedCallback;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool AddItem(ItemData item)
    {
        if (items.Count >= capacity)
        {
            Debug.Log("Not enough room.");
            return false;
        }

        items.Add(item);
        Debug.Log($"{item.name} ������ �߰���");
        onItemChangedCallback?.Invoke(); // UI ���� ȣ��


        return true;
    }


    public void RemoveItem(ItemData item)
    {
        items.Remove(item);
        onItemChangedCallback?.Invoke();
    }

    public void UseItem(ItemData item)
    {
        if (!items.Contains(item)) return;

        Debug.Log("���� ����: " + item.itemName);

        switch (item.type)
        {
            case ItemType.Consumable:
                PlayerHealth.instance.Heal(item.healAmount);
                RemoveItem(item);
                break;
            case ItemType.Equipment:
                break;
            case ItemType.Quest:
                break;
        }
        if (items.Contains(item))
        {
            GoldManager.instance.AddGold(item.price);
            RemoveItem(item);
            Debug.Log($"{item.itemName}��(��) {item.price} ��忡 �Ǹ�!");
        }
    }
}
