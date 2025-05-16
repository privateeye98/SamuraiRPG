using UnityEngine;
using TMPro;

public class DialogueManager: MonoBehaviour
{
    public static DialogueManager instance;

    [SerializeField] UnityEngine.UI.Image portraitImage;
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI dialogueText;

    string[] lines;
    int currentLine = 0;
    [TextArea]

    System.Action onDialogueComplete;

    void Awake()
    {
        instance = this;
    }

    public void StartDialogue(string npcName, string[] dialogueLines, Sprite portrait = null, System.Action onComplete = null)
    {
        dialoguePanel.SetActive(true);
        nameText.text = npcName;
        lines = dialogueLines;
        currentLine = 0;
        this.onDialogueComplete = onComplete;

        if (portraitImage != null && portrait != null)
            portraitImage.sprite = portrait;

        ShowLine();
    }

    void Update()
    {
        if (dialoguePanel.activeSelf && Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("��� �ѱ�� �õ� ��");

            currentLine++;
            if (currentLine < lines.Length)
            {
                Debug.Log($"���� ��: {currentLine} / �� �� ��: {lines.Length}");
                ShowLine();
            }
            else
            {
                EndDialogue();
            }
        }
    }

    void ShowLine()
    {
        if (currentLine < 0 || currentLine >= lines.Length)
        {
            Debug.LogWarning("�߸��� currentLine �ε��� ����");
            return;
        }

        dialogueText.text = lines[currentLine];
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        onDialogueComplete?.Invoke(); // ���⼭ ���� ���� �� ����
    }
}
