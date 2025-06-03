// InventoryItem.cs
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public ItemData itemData;  
    public int quantity;     
    public int level;        

    public InventoryItem(ItemData data, int qty = 1, int lv = 1)
    {
        itemData = data;
        quantity = qty;
        level = lv;
    }


    public Dictionary<StatType, int> GetEnhancedStats()
    {
        var dict = new Dictionary<StatType, int>();
        foreach (var mod in itemData.bonusStats)
        {
            dict[mod.stat] = mod.amount * level;
        }
        return dict;
    }
}
