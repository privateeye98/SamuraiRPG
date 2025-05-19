using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject uiCanvasPrefab;
    public GameObject[] managerPrefabs;

    private static bool initialized = false;

    void Awake()
    {
        if (initialized)
        {
            Destroy(gameObject); // �ߺ� ����
            return;
        }

        initialized = true;
        DontDestroyOnLoad(gameObject);

        // Player (Tag: "Player" ���� �ʿ�)
        if (GameObject.FindGameObjectWithTag("Player") == null)
        {
            var player = Instantiate(playerPrefab);
            player.name = "Player";
            DontDestroyOnLoad(player);
        }

        // UI Canvas (Tag: "Canvas" ���� �ʿ�)
        if (GameObject.FindGameObjectWithTag("Canvas") == null)
        {
            var canvas = Instantiate(uiCanvasPrefab);
            canvas.name = "Canvas";
            DontDestroyOnLoad(canvas);
        }

        // Manager Prefabs
        foreach (var prefab in managerPrefabs)
        {
            // �̱��� ������Ʈ �Ǵ� �̸� �ߺ� üũ
            if (GameObject.Find(prefab.name) == null)
            {
                var obj = Instantiate(prefab);
                obj.name = prefab.name;
                DontDestroyOnLoad(obj);
            }
        }
    }
}
