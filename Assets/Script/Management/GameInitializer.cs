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
            Destroy(gameObject); // 중복 방지
            return;
        }

        initialized = true;
        DontDestroyOnLoad(gameObject);

        // Player (Tag: "Player" 지정 필요)
        if (GameObject.FindGameObjectWithTag("Player") == null)
        {
            var player = Instantiate(playerPrefab);
            player.name = "Player";
            DontDestroyOnLoad(player);
        }

        // UI Canvas (Tag: "Canvas" 지정 필요)
        if (GameObject.FindGameObjectWithTag("Canvas") == null)
        {
            var canvas = Instantiate(uiCanvasPrefab);
            canvas.name = "Canvas";
            DontDestroyOnLoad(canvas);
        }

        // Manager Prefabs
        foreach (var prefab in managerPrefabs)
        {
            // 싱글톤 컴포넌트 또는 이름 중복 체크
            if (GameObject.Find(prefab.name) == null)
            {
                var obj = Instantiate(prefab);
                obj.name = prefab.name;
                DontDestroyOnLoad(obj);
            }
        }
    }
}
