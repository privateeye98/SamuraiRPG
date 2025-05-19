using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject uiPrefab;
    public GameObject cameraPrefab;
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

        // Player
        if (GameObject.FindWithTag("Player") == null && playerPrefab != null)
        {
            var player = Instantiate(playerPrefab);
            player.name = "Player";
            DontDestroyOnLoad(player);
        }

        // Camera
        if (GameObject.FindWithTag("MainCamera") == null && cameraPrefab != null)
        {
            var cam = Instantiate(cameraPrefab);
            cam.name = "Main Camera";
            DontDestroyOnLoad(cam);
        }

        // UI
        if (GameObject.Find("Canvas") == null && uiPrefab != null)
        {
            var ui = Instantiate(uiPrefab);
            ui.name = "Canvas";
            DontDestroyOnLoad(ui);
        }

        // 기타 매니저들
        foreach (var prefab in managerPrefabs)
        {
            if (GameObject.Find(prefab.name) == null)
            {
                var manager = Instantiate(prefab);
                manager.name = prefab.name;
                DontDestroyOnLoad(manager);
            }
        }
    }
}
