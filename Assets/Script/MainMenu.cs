using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void OnClickGameStart()
    {
        SceneManager.LoadScene("Game");

    }

    public void OnclickSetting()
    {
        //����
    }

    public void OnclickExit()
    {
        Application.Quit();
        Debug.Log("���� ����");
    }
}
