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
        //구현
    }

    public void OnclickExit()
    {
        Application.Quit();
        Debug.Log("게임 종료");
    }
}
