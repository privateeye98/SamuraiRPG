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

        // ���� �� �̸� ����
        string sceneName = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("SceneName", sceneName);

        // ��ġ ����
        PlayerPrefs.SetFloat("PlayerX", player.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.position.y);
        PlayerPrefs.SetFloat("PlayerZ", player.position.z);

        // ����, ����ġ ����
        PlayerPrefs.SetInt("PlayerLevel", PlayerLevel.instance.currentLevel);
        PlayerPrefs.SetFloat("PlayerExp", PlayerLevel.instance.currentExp);

        PlayerPrefs.Save();
        Debug.Log("���� ���� �Ϸ�");
    }

    public void LoadGame()
    {
        string sceneName = PlayerPrefs.GetString("SceneName", SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(sceneName);

        // ���� �ε�� �� ��ġ �� ���� ������ �ݹ� �Ǵ� StartCoroutine ���
        StartCoroutine(LoadAfterSceneLoaded());
    }

    System.Collections.IEnumerator LoadAfterSceneLoaded()
    {
        yield return new WaitForSeconds(0.1f); // �� �ε� ���

        if (player == null) yield break;

        float x = PlayerPrefs.GetFloat("PlayerX", player.position.x);
        float y = PlayerPrefs.GetFloat("PlayerY", player.position.y);
        float z = PlayerPrefs.GetFloat("PlayerZ", player.position.z);
        player.position = new Vector3(x, y, z);

        PlayerLevel.instance.currentLevel = PlayerPrefs.GetInt("PlayerLevel", 1);
        PlayerLevel.instance.currentExp = PlayerPrefs.GetInt("PlayerExp", 0);

        Debug.Log("���� �ҷ����� �Ϸ�");
    }
}
