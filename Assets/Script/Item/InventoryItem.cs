using UnityEngine;


[System.Serializable]
public class InventoryItem
{
    public ItemData itemData;
    public int quantity;

    public InventoryItem(ItemData data,int amount =1)
    {
        itemData = data;
        quantity = amount;
    }
}
