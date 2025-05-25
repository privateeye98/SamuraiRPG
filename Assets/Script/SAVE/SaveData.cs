using System.Collections.Generic;
[System.Serializable]
public class SaveData
{
    // 위치
    public float playerX, playerY, playerZ;
    // 체력/스탯
    public int currentHP, maxHP;
    public int currentMP, maxMP;
    public int strength, dexterity, critical;
    // 레벨/경험치
    public int level, exp, maxLevel;

    public List<ItemSaveInfo> inventoryItems = new List<ItemSaveInfo>();
    public List<ItemSaveInfo> equippedItems = new List<ItemSaveInfo>();

}
