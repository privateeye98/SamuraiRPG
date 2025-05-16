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
            Debug.Log("대사 넘기기 시도 중");

            currentLine++;
            if (currentLine < lines.Length)
            {
                Debug.Log($"현재 줄: {currentLine} / 총 줄 수: {lines.Length}");
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
            Debug.LogWarning("잘못된 currentLine 인덱스 접근");
            return;
        }

        dialogueText.text = lines[currentLine];
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        onDialogueComplete?.Invoke(); // 여기서 상점 열기 등 가능
    }
}
