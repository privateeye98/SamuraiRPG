using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject settingPanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool now = !settingPanel.activeSelf;
            settingPanel.SetActive(now);
            Time.timeScale = now ? 0f : 1f;   // 게임 일시정지
        }
    }
}
