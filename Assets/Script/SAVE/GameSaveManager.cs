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
            Debug.Log($"[SaveManager] 경로 설정: {saveFilePath}");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 현재 게임 상태를 파일로 저장합니다.
    /// </summary>
    public void SaveGame()
    {
        // 1) 데이터 수집
        var data = new GameSaveData
        {
            playerPosition = PlayerController.Instance.transform.position,
            playerLevel = PlayerController.Instance.Level,
            currentExp = PlayerController.Instance.Exp,
            playerStats = PlayerStat.Instance.GetStats(),           // 예: PlayerStats 객체 반환
            inventoryItems = InventoryManager.Instance.GetSaveList(),  // List<InventoryItem>
            questDataList = QuestManager.Instance.GetSaveList(),      // List<QuestSaveData>
            currentScene = SceneManager.GetActiveScene().name
        };

        // 2) JSON 직렬화 및 파일 쓰기
        string json = JsonUtility.ToJson(data, prettyPrint: true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log($"[SaveManager] 게임 저장 완료 → {saveFilePath}");
    }

    /// <summary>
    /// 저장된 파일에서 게임 상태를 불러옵니다.
    /// </summary>
    public void LoadGame()
    {
        if (!File.Exists(saveFilePath))
        {
            Debug.LogWarning("[SaveManager] 저장 파일을 찾을 수 없습니다.");
            return;
        }

        // 1) JSON 읽기 및 역직렬화
        string json = File.ReadAllText(saveFilePath);
        var data = JsonUtility.FromJson<GameSaveData>(json);

        // 2) 데이터 복원
        PlayerController.Instance.transform.position = data.playerPosition;
        PlayerController.Instance.SetLevelAndExp(data.playerLevel, data.currentExp);

        PlayerStat.Instance.ApplyStats(data.playerStats);                    // 예: 스탯 설정
        InventoryManager.Instance.LoadFromSave(data.inventoryItems);          // 인벤토리 복원
        QuestManager.Instance.LoadFromSave(data.questDataList);               // 퀘스트 복원

        // 3) 씬 로드 (필요 시)
        if (SceneManager.GetActiveScene().name != data.currentScene)
        {
            SceneManager.LoadScene(data.currentScene);
        }

        Debug.Log("[SaveManager] 게임 로드 완료");
    }
}
