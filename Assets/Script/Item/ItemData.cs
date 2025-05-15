using UnityEngine;

public enum ItemType { Equipment, Consumable, Quest }
[CreateAssetMenu(fileName = "NewItem", menuName ="Iventory/Item")]
public class ItemData : ScriptableObject
{
    public int id;
    public string itemName;
    public Sprite icon;
    public ItemType type;
    public int healAmount;

    [TextArea]
    public string description;
    public int price;
}
