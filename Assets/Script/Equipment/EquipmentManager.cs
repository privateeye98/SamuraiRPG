using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager instance;

    public Dictionary<ItemPartType, InventoryItem> equippedItems = new();
     void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }



    public void EquipItem(InventoryItem item)
    {
        var part = item.itemData.part;

        if (equippedItems.TryGetValue(part, out var oldItem))
        {
            Inventory.instance.items.Add(oldItem);
            equippedItems.Remove(part);
        }

        item.level = item.itemData.level;

        equippedItems[part] = item;

        Inventory.instance.items.Remove(item);

        var itemDatas = equippedItems.ToDictionary(x => x.Key, x => x.Value.itemData);
        var levels = equippedItems.ToDictionary(x => x.Key, x => x.Value.level);
        ApplyEquipmentStats(itemDatas, levels);

        Inventory.instance.NotifyItemChanged();
    }



public void ApplyEquipmentStats(Dictionary<ItemPartType, ItemData> items, Dictionary<ItemPartType, int> levels)
    {
        PlayerStat.instance?.ApplyEquipmentStats(items, levels);

        foreach (var part in items.Keys)
        {
            var item = items[part];
            var lv = levels.ContainsKey(part) ? levels[part] : 1;
            
        }

        foreach (var kv in equippedItems)
        {
            if(levels.TryGetValue(kv.Key,out var lvl))
            {
                kv.Value.level = lvl;
            }

            PlayerStat.instance?.ApplyEquipmentStats(items,levels);
        }
        EquipmentUI.instance?.RefreshUI();
         InventoryUI.instance?.UpdateUI();
    }

    // 장비 해제 함수
    public void UnequipItem(ItemPartType part)
    {
        if (!equippedItems.ContainsKey(part)) return;
        Inventory.instance.items.Add(equippedItems[part]);
        equippedItems.Remove(part);

        var itemDatas = equippedItems.ToDictionary(x => x.Key, x => x.Value.itemData);
        var levels = equippedItems.ToDictionary(x => x.Key, x => x.Value.level);
        ApplyEquipmentStats(itemDatas, levels);

        Inventory.instance.NotifyItemChanged();
    }

}