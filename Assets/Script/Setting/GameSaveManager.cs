using System.Collections.Generic;
using System.IO;
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
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGame()

    {
        PlayerPrefs.SetString("SceneName", SceneManager.GetActiveScene().name);

        var player = Player.instance;
        Vector3 pos = player.transform.position;
        PlayerPrefs.SetFloat("PlayerX", pos.x);
        PlayerPrefs.SetFloat("PlayerY", pos.y);
        PlayerPrefs.SetFloat("PlayerZ", pos.z);

        PlayerPrefs.SetInt("PlayerLevel", PlayerLevel.instance.currentLevel);
        PlayerPrefs.SetInt("PlayerExp", PlayerLevel.instance.currentExp);

        PlayerPrefs.SetInt("PlayerSTR", PlayerStat.instance.strength);
        PlayerPrefs.SetInt("PlayerDEX", PlayerStat.instance.dexterity);
        PlayerPrefs.SetInt("PlayerCRIT", PlayerStat.instance.critical);
        PlayerPrefs.SetFloat("PlayerExpBonus", PlayerStat.instance.expMultiplier);

        // 강화 아이템 저장
        var upgradeItems = UpgradeUI.instance.upgradeItems;
        int index = 0;
        foreach (var kvp in upgradeItems)
        {
            var item = kvp.Value;
            PlayerPrefs.SetString($"Item{index}_Name", item.itemName);
            PlayerPrefs.SetInt($"Item{index}_Level", item.level);
            PlayerPrefs.SetInt($"Item{index}_StatType", (int)item.statType);
            index++;
        }


        PlayerPrefs.Save();
    }

    public void LoadGame()
    {
        string scene = PlayerPrefs.GetString("SceneName", "");
        if (!string.IsNullOrEmpty(scene))
            SceneManager.LoadScene(scene);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        Vector3 pos = new Vector3(
            PlayerPrefs.GetFloat("PlayerX", 0f),
            PlayerPrefs.GetFloat("PlayerY", 0f),
            PlayerPrefs.GetFloat("PlayerZ", 0f));

        var player = Player.instance;
        if (player == null && playerPrefab != null)
        {
            player = Instantiate(playerPrefab, pos, Quaternion.identity).GetComponent<Player>();
        }
        else
        {
            player.transform.position = pos;
        }

        PlayerLevel.instance.currentLevel = PlayerPrefs.GetInt("PlayerLevel", 1);
        PlayerLevel.instance.currentExp = PlayerPrefs.GetInt("PlayerExp", 0);

        PlayerStat.instance.strength = PlayerPrefs.GetInt("PlayerSTR", 0);
        PlayerStat.instance.dexterity = PlayerPrefs.GetInt("PlayerDEX", 0);
        PlayerStat.instance.critical = PlayerPrefs.GetInt("PlayerCRIT", 0);
        PlayerStat.instance.expMultiplier = PlayerPrefs.GetFloat("PlayerExpBonus", 1f);

        PlayerStat.instance.RecalculateStats();
        StatUI.instance?.UpdateUI();

        // 강화 아이템 불러오기
        var upgradeItems = UpgradeUI.instance.upgradeItems;
        int index = 0;
        List<ItemPartType> keys = new List<ItemPartType>(upgradeItems.Keys);
        foreach (var key in keys)
        {
            var item = upgradeItems[key];
            item.itemName = PlayerPrefs.GetString($"Item{index}_Name", "");
            item.level = PlayerPrefs.GetInt($"Item{index}_Level", 0);
            item.statType = (StatType)PlayerPrefs.GetInt($"Item{index}_StatType", 0);
            index++;
        }
        UpgradeUI.instance.RefreshStatPreview();
    }
}
