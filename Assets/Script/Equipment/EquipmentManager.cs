using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager instance;

    public Dictionary<ItemPartType, InventoryItem> equippedItems = new Dictionary<ItemPartType, InventoryItem>();

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void EquipItem(InventoryItem item)
    {
        var data = item.itemData;
        var part = data.part;

        if (PlayerLevel.instance.currentLevel < data.requiredLevel)
        {
            Debug.LogWarning($"Lv{data.requiredLevel} 이상만 착용할 수 있습니다.");
            return;
        }
        foreach (var req in data.requiredStats)
        {
            if (PlayerStat.instance.GetStat(req.stat) < req.value)
            {
                Debug.LogWarning($"{req.stat} {req.value} 이상이어야 착용할 수 있습니다.");
                return;
            }
        }

        if (equippedItems.TryGetValue(part, out var oldItem))
        {
            Inventory.instance.items.Add(oldItem);
            equippedItems.Remove(part);
        }

        equippedItems[part] = item;
        Inventory.instance.items.Remove(item);

        ApplyEquipmentEffects();
        Inventory.instance.NotifyItemChanged();
    }

    public void UnequipItem(ItemPartType part)
    {
        if (!equippedItems.ContainsKey(part)) return;

        Inventory.instance.items.Add(equippedItems[part]);
        equippedItems.Remove(part);

        ApplyEquipmentEffects();
        Inventory.instance.NotifyItemChanged();
    }

    private void ApplyEquipmentEffects()
    {
        PlayerStat.instance.ResetEquipmentBonuses();

        foreach (var kv in equippedItems)
        {
            foreach (var mod in kv.Value.itemData.bonusStats)
            {
                PlayerStat.instance.AddEquipmentBonus(mod.stat, mod.amount);
            }
        }

        PlayerStat.instance.NotifyStatChanged();
        StatUI.instance.UpdateUI();
        EquipmentUI.instance?.RefreshUI();
        InventoryUI.instance?.UpdateUI();
    }

    public void ApplyEquipmentStats(
        Dictionary<ItemPartType, ItemData> items,
        Dictionary<ItemPartType, int> levels)
    {
        PlayerStat.instance.ApplyEquipmentStats(items, levels);

        PlayerStat.instance.NotifyStatChanged();
        StatUI.instance.UpdateUI();
        EquipmentUI.instance?.RefreshUI();
        InventoryUI.instance?.UpdateUI();
    }
}
