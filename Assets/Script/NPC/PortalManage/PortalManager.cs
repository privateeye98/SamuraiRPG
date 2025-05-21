using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class PortalManager : MonoBehaviour
{
    public static PortalManager Instance;

    [Header("UI")]
    public GameObject panel;
    public TextMeshProUGUI dialogueText;
    public Button yesButton;
    public Button noButton;
    public Image portrait;

    [System.Serializable]
    public class PortalData
    {
        public string portalID;
        public string sceneName;
        [TextArea] public string dialogue;
        public Sprite portraitSprite;

        public string spawnPointName;
    }

    [Header("Portal Data")]
    public List<PortalData> portalList = new();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 중복 제거
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        panel.SetActive(false);
    }

    public void OpenPortal(string portalID)
    {
        PortalData data = portalList.Find(p => p.portalID == portalID);
        if (data == null)
        {
            Debug.LogWarning("해당 ID의 포탈 데이터를 찾을 수 없습니다: " + portalID);
            return;
        }

        // UI 표시
        panel.SetActive(true);
        dialogueText.text = data.dialogue;
        portrait.sprite = data.portraitSprite;

        // 버튼 연결
        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();

        yesButton.onClick.AddListener(() =>
        {
            PlayerPrefs.SetString("SpawnPoint", data.spawnPointName); // 동적 저장
            PlayerPrefs.Save();

            SceneManager.LoadScene(data.sceneName);
        });

        noButton.onClick.AddListener(() =>
        {
            panel.SetActive(false);
        });
    }

}
