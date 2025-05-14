using UnityEngine;
using UnityEngine.SceneManagement;   

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject settingPanel;
   
    public void OnClickGameStart()
    {
       
        SceneManager.LoadScene("Game");
    }

    // ③ 설정 버튼 (예: 설정 패널 열기)
    public void OnClickSetting()
    {
        Debug.Log("SETTING 버튼 눌림");
        settingPanel.SetActive(true); // 설정 패널 활성화
       
    }

    // ④ 종료 버튼
    public void OnClickExit()
    {
#if UNITY_EDITOR          // 에디터에서 테스트할 때
        UnityEditor.EditorApplication.isPlaying = false;
#else                     // 빌드된 실행 파일에서는
        Application.Quit();
#endif
    }
}
