using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public string sceneName;
    public Vector3 playerPosition;

    public int level;
    public int exp;

    public int str, dex, crit;
    public float expMultiplier;

    public int gold;

    public List<SavedInventoryItem> inventory = new();
    public List<SavedQuest> savedQuests = new();
    public List<SavedItem> equippedItems = new();
}

[Serializable]
public class SavedInventoryItem
{
    public int itemId;
    public int quantity;
}

[Serializable]
public class SavedQuest
{
    public string questID;
    public int currentAmount;
    public QuestState state;
}

[Serializable]
public class SavedItem
{
    public string part;
    public int itemId;
    public int level;
    public StatType statType;
}
