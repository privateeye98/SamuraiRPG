using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject[] buttons; // START, OPTION, EXIT 등
    [SerializeField] GameObject titleLogo;
    [SerializeField] GameObject settingPanel;

    public void OnClickGameStart()
    {
        Debug.Log("🎯 START 버튼 눌림 테스트용");  // 
        SceneManager.LoadScene("Game");
    }


    public void OnClickSetting()
    {
        settingPanel.SetActive(true);

        foreach (var btn in buttons)
            btn.SetActive(false);

        if (titleLogo != null)
            titleLogo.SetActive(false);
    }

    public void OnClickExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnCloseSetting()
    {
        settingPanel.SetActive(false);

        foreach (var btn in buttons)
            btn.SetActive(true);

        if (titleLogo != null)
            titleLogo.SetActive(true);
    }
}
