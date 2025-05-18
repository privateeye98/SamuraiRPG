using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSaveManager : MonoBehaviour
{
    public static GameSaveManager I;

    [SerializeField] Transform player;

    void Awake()
    {
        if (I == null) { I = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
    }

    public void SaveGame()
    {
        if (player == null) return;

        // 현재 씬 이름 저장
        string sceneName = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("SceneName", sceneName);

        // 위치 저장
        PlayerPrefs.SetFloat("PlayerX", player.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.position.y);
        PlayerPrefs.SetFloat("PlayerZ", player.position.z);

        // 레벨, 경험치 저장
        PlayerPrefs.SetInt("PlayerLevel", PlayerLevel.instance.currentLevel);
        PlayerPrefs.SetFloat("PlayerExp", PlayerLevel.instance.currentExp);

        PlayerPrefs.Save();
        Debug.Log("게임 저장 완료");
    }

    public void LoadGame()
    {
        string sceneName = PlayerPrefs.GetString("SceneName", SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(sceneName);

        // 씬이 로드된 후 위치 및 스탯 복원은 콜백 또는 StartCoroutine 사용
        StartCoroutine(LoadAfterSceneLoaded());
    }

    System.Collections.IEnumerator LoadAfterSceneLoaded()
    {
        yield return new WaitForSeconds(0.1f); // 씬 로딩 대기

        if (player == null) yield break;

        float x = PlayerPrefs.GetFloat("PlayerX", player.position.x);
        float y = PlayerPrefs.GetFloat("PlayerY", player.position.y);
        float z = PlayerPrefs.GetFloat("PlayerZ", player.position.z);
        player.position = new Vector3(x, y, z);

        PlayerLevel.instance.currentLevel = PlayerPrefs.GetInt("PlayerLevel", 1);
        PlayerLevel.instance.currentExp = PlayerPrefs.GetInt("PlayerExp", 0);

        Debug.Log("게임 불러오기 완료");
    }
}
