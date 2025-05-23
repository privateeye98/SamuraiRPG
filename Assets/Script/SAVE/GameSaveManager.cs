using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    private string saveFilePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            saveFilePath = Path.Combine(Application.persistentDataPath, "saveData.json");
            Debug.Log($"[SaveManager] ��� ����: {saveFilePath}");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// ���� ���� ���¸� ���Ϸ� �����մϴ�.
    /// </summary>
    public void SaveGame()
    {
        // 1) ������ ����
        var data = new GameSaveData
        {
            playerPosition = PlayerController.Instance.transform.position,
            playerLevel = PlayerController.Instance.Level,
            currentExp = PlayerController.Instance.Exp,
            playerStats = PlayerStat.Instance.GetStats(),           // ��: PlayerStats ��ü ��ȯ
            inventoryItems = InventoryManager.Instance.GetSaveList(),  // List<InventoryItem>
            questDataList = QuestManager.Instance.GetSaveList(),      // List<QuestSaveData>
            currentScene = SceneManager.GetActiveScene().name
        };

        // 2) JSON ����ȭ �� ���� ����
        string json = JsonUtility.ToJson(data, prettyPrint: true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log($"[SaveManager] ���� ���� �Ϸ� �� {saveFilePath}");
    }

    /// <summary>
    /// ����� ���Ͽ��� ���� ���¸� �ҷ��ɴϴ�.
    /// </summary>
    public void LoadGame()
    {
        if (!File.Exists(saveFilePath))
        {
            Debug.LogWarning("[SaveManager] ���� ������ ã�� �� �����ϴ�.");
            return;
        }

        // 1) JSON �б� �� ������ȭ
        string json = File.ReadAllText(saveFilePath);
        var data = JsonUtility.FromJson<GameSaveData>(json);

        // 2) ������ ����
        PlayerController.Instance.transform.position = data.playerPosition;
        PlayerController.Instance.SetLevelAndExp(data.playerLevel, data.currentExp);

        PlayerStat.Instance.ApplyStats(data.playerStats);                    // ��: ���� ����
        InventoryManager.Instance.LoadFromSave(data.inventoryItems);          // �κ��丮 ����
        QuestManager.Instance.LoadFromSave(data.questDataList);               // ����Ʈ ����

        // 3) �� �ε� (�ʿ� ��)
        if (SceneManager.GetActiveScene().name != data.currentScene)
        {
            SceneManager.LoadScene(data.currentScene);
        }

        Debug.Log("[SaveManager] ���� �ε� �Ϸ�");
    }
}
