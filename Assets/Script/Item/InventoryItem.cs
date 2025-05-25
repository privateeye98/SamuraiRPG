using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class InventoryItem
{
    public ItemData itemData;
    public int quantity;
    public int level;

    public Dictionary<StatType,int> GetEnhancedStats()
    {
        var stats = new Dictionary<StatType, int>();

        stats[StatType.ATK] = itemData.atk;
        stats[StatType.HP] = itemData.hpBonusPerLevel * level;
        stats[StatType.MP] = itemData.mpBonusPerLevel * level;
        stats[StatType.STR] = itemData.strBonusPerLevel * level;
        stats[StatType.DEX] = itemData.dexBonusPerLevel * level;
        stats[StatType.CRIT] = itemData.critBonusPerLevel * level;
        return stats;
    }

    public InventoryItem(ItemData data, int qty = 1, int lv = 1)
    {
        itemData = data;
        quantity = qty;
        this.level = lv;
    }
}
