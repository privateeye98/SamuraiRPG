using System.Collections.Generic;
[System.Serializable]
public class SaveData
{
    // ��ġ
    public float playerX, playerY, playerZ;
    // ü��/����
    public int currentHP, maxHP;
    public int currentMP, maxMP;
    public int strength, dexterity, critical;
    // ����/����ġ
    public int level, exp, maxLevel;

    public List<ItemSaveInfo> inventoryItems = new List<ItemSaveInfo>();
    public List<ItemSaveInfo> equippedItems = new List<ItemSaveInfo>();

}
