using UnityEngine;
using System.Collections.Generic;


public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public List<InventoryItem> items = new List<InventoryItem>();

    public int capacity = 9;

    public delegate void OnItemChanged();
    public event OnItemChanged OnItemChangedCallback;

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

    public bool AddItem(ItemData itemData)
    {
        if(itemData.isStackable)
        {
            foreach(var invItem in items)
            {
                if(invItem.itemData.id == itemData.id && invItem.quantity < itemData.maxStack)
                    {
                    invItem.quantity++;
                    OnItemChangedCallback?.Invoke();
                    return true;
                }
            }
        }
        if (items.Count >= capacity)
        {
            Debug.Log("Not enough room.");
            return false;
        }

        items.Add(new InventoryItem(itemData));
        OnItemChangedCallback?.Invoke(); // UI 갱신 호출
        return true;
    }
    public bool HasRoom()
    {
        return items.Count < capacity;
    }

    public void RemoveItem(ItemData item)
    {
        var invItem = items.Find(i => i.itemData == item);
        if (invItem != null)
            items.Remove(invItem);

        OnItemChangedCallback?.Invoke();
    }

    public void UseItem(ItemData item)
    {
        var invItem = items.Find(i => i.itemData == item);
        if (invItem == null) return;

        switch (item.type)
        {
            case ItemType.Consumable:
                PlayerHealth.instance.Heal(item.healAmount);
                invItem.quantity--;

                if (invItem.quantity <= 0)
                    items.Remove(invItem);

                OnItemChangedCallback?.Invoke();
                break;

            case ItemType.Equipment:
                Debug.Log("장비 사용은 아직 구현되지 않음");
                break;

            case ItemType.Quest:
                Debug.Log("퀘스트 아이템은 사용할 수 없습니다.");
                break;
        }
    }
    public void SellItem(ItemData item)
    {
        var invItem = items.Find(i => i.itemData == item);
        if(invItem == null)return;

        GoldManager.instance.AddGold(item.price);
        RemoveItem(item);
        Debug.Log($"{item.itemName}을(를) {item.price}골드에 판매했습니다.");
    }
    public void Clear()
    {
        items.Clear();
        OnItemChangedCallback?.Invoke(); // UI 갱신
    }

}
