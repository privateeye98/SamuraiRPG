using System.Collections.Generic;
using System.IO;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSaveManager : MonoBehaviour
{
    public static GameSaveManager instance;
    private string savePath => Application.persistentDataPath + "/save.json";

    public void SaveGame()
    {
        SaveData data = new();

        data.sceneName = SceneManager.GetActiveScene().name;
        data.playerPosition = Player.instance?.transform.position ?? Vector3.zero;

        data.level = PlayerLevel.instance.currentLevel;
        data.exp = PlayerLevel.instance.currentExp;

        data.str = PlayerStat.instance.strength;
        data.dex = PlayerStat.instance.dexterity;
        data.crit = PlayerStat.instance.critical;
        data.expMultiplier = PlayerStat.instance.expMultiplier;

        data.gold = GoldManager.instance.currentGold;

        foreach (var item in Inventory.instance.items)
        {
            data.inventory.Add(new SavedInventoryItem
            {
                itemId = item.itemData.id,
                quantity = item.quantity
            });
        }

        foreach (var quest in QuestManager.instance.activeQuests)
        {
            data.savedQuests.Add(new SavedQuest
            {
                questID = quest.data.questID,
                currentAmount = quest.currentAmount,
                state = quest.state
            });
        }

        foreach (var kvp in UpgradeUI.instance.upgradeItems)
        {
            data.equippedItems.Add(new SavedItem
            {
                part = kvp.Key.ToString(),
                itemId = kvp.Value.id,
                level = kvp.Value.level,
                statType = kvp.Value.statType
            });
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        PlayerPrefs.SetString("SaveData", json);
        PlayerPrefs.Save();

        Debug.Log($"[GameSaveManager] 저장 완료: {savePath}");
    }

    public void LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("저장 파일 없음");
            return;
        }

        string json = File.ReadAllText(savePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        PlayerLevel.instance.SetLevel(data.level);
        PlayerLevel.instance.currentExp = data.exp;

        PlayerStat.instance.strength = data.str;
        PlayerStat.instance.dexterity = data.dex;
        PlayerStat.instance.critical = data.crit;
        PlayerStat.instance.expMultiplier = data.expMultiplier;

        GoldManager.instance.SetGold(data.gold);

        Inventory.instance.Clear();
        foreach (var item in data.inventory)
        {
            ItemData loaded = ItemDatabase.instance.GetItemById(item.itemId);
            for (int i = 0; i < item.quantity; i++)
                Inventory.instance.AddItem(loaded);
        }

        QuestManager.instance.activeQuests.Clear();
        foreach (var saved in data.savedQuests)
        {
            QuestData questData = QuestManager.instance.GetQuestDataByID(saved.questID);
            if (questData != null)
            {
                Quest quest = new Quest(questData);
                quest.currentAmount = saved.currentAmount;
                quest.state = saved.state;
                QuestManager.instance.activeQuests.Add(quest);
            }
        }

        UpgradeUI.instance.LoadUpgrades(data.equippedItems);

        Debug.Log("[GameSaveManager] 불러오기 완료");
    }
}
