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
                // ü���� ���� �� ������ ��� �Ұ�
                if (PlayerHealth.instance.currentHP >= PlayerHealth.instance.maxHP)
                {
                    Debug.Log("ü���� ���� �� �־� �������� ����� �� �����ϴ�.");
                    return;
                }

                PlayerHealth.instance.Heal(item.healAmount);
                RemoveItem(item);
                break;

            case ItemType.Equipment:
                Debug.Log("��� ����� ���� �������� ����");
                break;

            case ItemType.Quest:
                Debug.Log("����Ʈ �������� ����� �� �����ϴ�.");
                break;
        }

        // �� �̻� �ڵ� �Ǹ� �� ��
    }
    public void SellItem(ItemData item)
    {
        if (!items.Contains(item)) return;

        GoldManager.instance.AddGold(item.price);
        RemoveItem(item);
        Debug.Log($"{item.itemName}��(��) {item.price}��忡 �Ǹ��߽��ϴ�.");
    }

}
