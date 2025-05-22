using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuPanel;
    [SerializeField] GameObject settingMenuPanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenuPanel.activeSelf)
                ResumeGame();
            else
                OpenPauseMenu();
        }
    }

    public void OpenPauseMenu()
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnClickContinue()
    {
        ResumeGame();
    }

    public void OnClickSetting()
    {
        pauseMenuPanel.SetActive(false);
        settingMenuPanel.SetActive(true);
    }

    public void OnClickSave()
    {
        GameSaveManager.instance.SaveGame();    
    }

    public void OnClickLoad()
    {
        GameSaveManager.instance.LoadGame();
    }

    public void OnClickQuit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}

