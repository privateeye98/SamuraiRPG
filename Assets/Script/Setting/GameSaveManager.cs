using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSaveManager : MonoBehaviour
{
    public static GameSaveManager instance;

    [Header("플레이어 프리팹 및 참조")]
    public GameObject playerPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            ItemDatabaseGlobal.Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGame()
    {
        SaveData data = new SaveData();
        
        data.sceneName = SceneManager.GetActiveScene().name;

        if (Player.instance != null)
        {
            data.playerPosition = Player.instance.transform.position;
        }

        data.level = PlayerLevel.instance.currentLevel;
        data.exp = PlayerLevel.instance.currentExp;

        data.str = PlayerStat.instance.strength;
        data.dex = PlayerStat.instance.dexterity;
        data.crit = PlayerStat.instance.critical;
        data.expMultiplier = PlayerStat.instance.expMultiplier;

        // 강화 아이템 저장
        if (UpgradeUI.instance != null && UpgradeUI.instance.upgradeItems != null)
        {
            foreach (var kvp in UpgradeUI.instance.upgradeItems)
            {
                SavedItem item = new SavedItem
                {
                    part = kvp.Key.ToString(),
                    itemId = kvp.Value.id,
                    level = kvp.Value.level,
                    statType = kvp.Value.statType
                };
                data.equippedItems.Add(item);
            }
        }

        foreach (var quest in QuestManager.instance.activeQuests)
        {
            SavedQuest saved = new()
            {
                questID = quest.data.questID,
                currentAmount = quest.currentAmount,
                state = quest.state
            };
            data.savedQuests.Add(saved);
        }

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("SaveData", json);
        PlayerPrefs.Save();
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
    }

    public void LoadGame()
    {
        if (!PlayerPrefs.HasKey("SaveData")) return;

        string json = PlayerPrefs.GetString("SaveData");
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        SceneManager.sceneLoaded += (scene, mode) => OnSceneLoaded(scene, mode, data);
        SceneManager.LoadScene(data.sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode, SaveData data)
    {
        SceneManager.sceneLoaded -= (s, m) => OnSceneLoaded(s, m, data);

        if (playerPrefab != null && Player.instance == null)
        {
            var obj = Instantiate(playerPrefab, data.playerPosition, Quaternion.identity);
            Player.instance = obj.GetComponent<Player>();
        }
        else if (Player.instance != null)
        {
            Player.instance.transform.position = data.playerPosition;
        }

        PlayerLevel.instance.currentLevel = data.level;
        PlayerLevel.instance.currentExp = data.exp;

        PlayerStat.instance.strength = data.str;
        PlayerStat.instance.dexterity = data.dex;
        PlayerStat.instance.critical = data.crit;
        PlayerStat.instance.expMultiplier = data.expMultiplier;

        Dictionary<ItemPartType, ItemData> items = new();

        foreach (var saved in data.equippedItems)
        {
            if (System.Enum.TryParse(saved.part, out ItemPartType part))
            {
                ItemData original = ItemDatabaseGlobal.GetItemById(saved.itemId);
                if (original != null)
                {
                    ItemData clone = Instantiate(original);
                    clone.level = saved.level;
                    clone.statType = saved.statType;
                    items[part] = clone;
                }
            }
        }

        if (UpgradeUI.instance != null)
        {
            UpgradeUI.instance.upgradeItems = items;
            UpgradeUI.instance.RefreshStatPreview();
        }

        PlayerStat.instance.ApplyEquipmentStats(items);
        StatUI.instance?.UpdateUI();
    }
}

[System.Serializable]
public class SaveData
{
    public string sceneName;
    public Vector3 playerPosition;
    public int level;
    public int exp;
    public int str, dex, crit;
    public float expMultiplier;
    public List<SavedItem> equippedItems = new();
}

[System.Serializable]
public class SavedItem
{
    public string part;
    public int itemId;
    public int level;
    public StatType statType;
}

public static class ItemDatabaseGlobal
{
    public static List<ItemData> allItems;

    public static void Init()
    {
        allItems = new List<ItemData>(Resources.LoadAll<ItemData>("Items"));
    }

    public static ItemData GetItemById(int id)
    {
        return allItems?.Find(i => i.id == id);
    }
}


