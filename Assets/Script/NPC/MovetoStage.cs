using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class MovetoStage : MonoBehaviour
{
    [Header("Settings")]
    public string sceneToLoad;               // �̵��� �� �̸�
    [TextArea]
    public string dialogueMessage;           // ��Ż�� ��� �Է�

    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public Button yesButton;
    public Button noButton;
    public Image portraitImage;
    public Sprite npcPortrait;

    private bool isPlayerInRange = false;

    void Start()
    {
        dialoguePanel.SetActive(false);

        // ��ư ������ ����
        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();

        yesButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(sceneToLoad);
        });

        noButton.onClick.AddListener(() =>
        {
            dialoguePanel.SetActive(false);
        });

        if (portraitImage && npcPortrait)
            portraitImage.sprite = npcPortrait;
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.Z))
        {
            ShowDialogue(dialogueMessage);
        }
    }

    public void ShowDialogue(string message)
    {
        dialoguePanel.SetActive(true);
        dialogueText.text = message;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            isPlayerInRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            dialoguePanel.SetActive(false);
        }
    }

}

