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
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void NotifyItemChanged() => OnItemChangedCallback?.Invoke();

    public bool AddItem(ItemData itemData)
    {
        if (itemData.isStackable)
        {
            foreach (var invItem in items)
            {
                if (invItem.itemData.id == itemData.id && invItem.quantity < itemData.maxStack)
                {
                    invItem.quantity++;
                    NotifyItemChanged();
                    return true;
                }
            }
        }
        if (items.Count >= capacity)
        {
            Debug.Log("인벤토리 풀");
            return false;
        }

        items.Add(new InventoryItem(itemData, 1, 1));
        NotifyItemChanged();
        return true;
    }

    public bool HasRoom() => items.Count < capacity;

    public void RemoveItem(ItemData item, int quantity = 1)
    {
        var invItem = items.Find(i => i.itemData == item);
        if (invItem != null)
        {
            invItem.quantity -= quantity;
            if (invItem.quantity <= 0)
                items.Remove(invItem);

            NotifyItemChanged();
        }
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
                NotifyItemChanged();
                break;

            case ItemType.Equipment:
                EquipmentManager.instance.EquipItem(invItem);
                NotifyItemChanged();
                break;

            case ItemType.Quest:
                Debug.Log("퀘스트 아이템은 사용할 수 없습니다.");
                break;
        }
    }

    public void SellItem(ItemData item, int sellQuantity = 1)
    {
        var invItem = items.Find(i => i.itemData == item);
        if (invItem == null) return;

        int quantityToSell = Mathf.Min(sellQuantity, invItem.quantity);
        GoldManager.instance.AddGold(item.price * quantityToSell);

        invItem.quantity -= quantityToSell;
        if (invItem.quantity <= 0)
            items.Remove(invItem);

        NotifyItemChanged();
    }

    public void Clear()
    {
        items.Clear();
        NotifyItemChanged();
    }
}
