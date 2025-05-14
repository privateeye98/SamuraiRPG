using UnityEngine;
using UnityEngine.SceneManagement;   

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject settingPanel;
   
    public void OnClickGameStart()
    {
       
        SceneManager.LoadScene("Game");
    }

    // �� ���� ��ư (��: ���� �г� ����)
    public void OnClickSetting()
    {
        Debug.Log("SETTING ��ư ����");
        settingPanel.SetActive(true); // ���� �г� Ȱ��ȭ
       
    }

    // �� ���� ��ư
    public void OnClickExit()
    {
#if UNITY_EDITOR          // �����Ϳ��� �׽�Ʈ�� ��
        UnityEditor.EditorApplication.isPlaying = false;
#else                     // ����� ���� ���Ͽ�����
        Application.Quit();
#endif
    }
}
