using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
public class GameSaveManager : MonoBehaviour
{
    public static GameSaveManager instance;
    private string savePath;

    void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
        savePath = Path.Combine(Application.persistentDataPath, "save.json");
    }

    public void SaveGame()
    {
        SaveData data = new SaveData();

        foreach (var invItem in Inventory.instance.items)
        {
            data.inventoryItems.Add(new ItemSaveInfo
            {
                itemId = invItem.itemData.id,
                level = invItem.level, // 강화레벨
                amount = invItem.quantity,
                partType = (int)invItem.itemData.part
            });
        }

        var player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            data.playerX = player.transform.position.x;
            data.playerY = player.transform.position.y;
            data.playerZ = player.transform.position.z;
        }

        // 체력/스탯 (싱글톤 인스턴스에서 읽어옴)
        data.currentHP = PlayerHealth.instance != null ? PlayerHealth.instance.currentHP : 0;
        data.maxHP = PlayerHealth.instance != null ? PlayerHealth.instance.maxHP : 0;

        if (PlayerStat.instance != null)
        {
            data.currentMP = PlayerStat.instance.currentMP;
            data.maxMP = PlayerStat.instance.maxMP;
            data.strength = PlayerStat.instance.strength;
            data.dexterity = PlayerStat.instance.dexterity;
            data.critical = PlayerStat.instance.critical;
        }

        if (PlayerLevel.instance != null)
        {
            data.level = PlayerLevel.instance.currentLevel;
            data.exp = PlayerLevel.instance.currentExp;
            data.maxLevel = PlayerLevel.instance.maxLevel;
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("게임 저장 완료!");
    }

    public void LoadGame()
    {
        if (!File.Exists(savePath)) { Debug.Log("세이브 파일 없음!"); return; }
        string json = File.ReadAllText(savePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        // 위치 복원
        var player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.position = new Vector3(data.playerX, data.playerY, data.playerZ);
        }

        // 체력/스탯 복원
        if (PlayerHealth.instance != null)
        {
            PlayerHealth.instance.currentHP = data.currentHP;
            PlayerHealth.instance.maxHP = data.maxHP;
        }
        if (PlayerStat.instance != null)
        {
            PlayerStat.instance.currentMP = data.currentMP;
            PlayerStat.instance.maxMP = data.maxMP;
            PlayerStat.instance.strength = data.strength;
            PlayerStat.instance.dexterity = data.dexterity;
            PlayerStat.instance.critical = data.critical;
            PlayerStat.instance.NotifyStatChanged();
        }
        if (PlayerLevel.instance != null)
        {
            PlayerLevel.instance.currentLevel = data.level;
            PlayerLevel.instance.currentExp = data.exp;
            PlayerLevel.instance.maxLevel = data.maxLevel;
        }

        // 인벤토리 복원
        Inventory.instance.items.Clear();
        foreach (var saveInfo in data.inventoryItems)
        {
            var itemData = ItemDatabase.instance.GetItemById(saveInfo.itemId);
            if (itemData != null)
            { 

                Inventory.instance.items.Add(new InventoryItem(itemData, saveInfo.amount));
            }
        }
        Inventory.instance.NotifyItemChanged();
        Debug.Log("게임 로드 완료!");
    }
}
