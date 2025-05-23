using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class GameSaveData
{
    public Vector3 playerPosition;
    public int playerLevel;
    public float currentExp;

    public List<InventoryItem> inventoryItems;
    public List<QuestSaveData> questDataList;
    public string currentScene;
    public PlayerStat playerStats;

    [Serializable]
    public class PlayerStats
    {
        public int maxHP;
        public int currentHP;
        public int maxMP;
        public int attack;
        public int dexterity;

    }


    [Serializable]
    public class InventoryItem
    {
        public int itemID;
        public int count;
        public int enhancementLevel;
    }

    [SerializeField]
    public class QuestSaveData
    {
        public int qeustID;
        public int progress;
    }
}
