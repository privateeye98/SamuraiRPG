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

    public bool AddItem(ItemData itemData, int amount)
    {
        if (itemData.isStackable)
        {
            int remaining = amount;

            foreach (var inv in items)
            {
                if (inv.itemData.id == itemData.id)
                {
                    int space = itemData.maxStack - inv.quantity;
                    int toAdd = Mathf.Min(space, remaining);
                    inv.quantity += toAdd;
                    remaining -= toAdd;
                    if (remaining <= 0) break;
                }
            }

            while (remaining > 0 && items.Count < capacity)
            {
                int toAdd = Mathf.Min(itemData.maxStack, remaining);
                items.Add(new InventoryItem(itemData, toAdd, 1));
                remaining -= toAdd;
            }

            if (remaining > 0)
                return false; 
        }
        else
        {
            if (items.Count + amount > capacity)
                return false;

            for (int i = 0; i < amount; i++)
                items.Add(new InventoryItem(itemData, 1, 1));
        }

        NotifyItemChanged();
        return true;
    }
    public bool AddItem(ItemData itemData)
    {
        return AddItem(itemData, 1);
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
