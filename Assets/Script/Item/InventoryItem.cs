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

        PopulateBonusStats();
    }
    private void PopulateBonusStats()
    {
        var list = new List<ItemData.StatModifier>();

        if (itemData.perLevelATK != 0)
            list.Add(new ItemData.StatModifier { stat = StatType.ATK, amount = itemData.perLevelATK });
        if (itemData.perLevelHP != 0)
            list.Add(new ItemData.StatModifier { stat = StatType.HP, amount = itemData.perLevelHP });
        if (itemData.perLevelMP != 0)
            list.Add(new ItemData.StatModifier { stat = StatType.MP, amount = itemData.perLevelMP });
        if (itemData.perLevelSTR != 0)
            list.Add(new ItemData.StatModifier { stat = StatType.STR, amount = itemData.perLevelSTR });
        if (itemData.perLevelDEX != 0)
            list.Add(new ItemData.StatModifier { stat = StatType.DEX, amount = itemData.perLevelDEX });
        if (itemData.perLevelCRIT != 0)
            list.Add(new ItemData.StatModifier { stat = StatType.CRIT, amount = itemData.perLevelCRIT });

        itemData.bonusStats = list.ToArray();
        Debug.Log($"[PopulateBonusStats] {itemData.itemName} bonusStats count: {itemData.bonusStats.Length}");
    }
    public Dictionary<StatType, int> GetEnhancedStats()
    {
        var dict = new Dictionary<StatType, int>();
        foreach (var mod in itemData.bonusStats)
        {
            int total = mod.amount * level;
            dict[mod.stat] = mod.amount * level;
            Debug.Log($"[GetEnhancedStats] {itemData.itemName} ·¹º§ {level} ¡æ {mod.stat} +{total}");

        }
        return dict;
    }
}
