using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [SerializeField] GameObject dialoguePanel;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] Image portraitImage;

    string[] lines;
    int currentLine = 0;
    System.Action onDialogueComplete;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // 중복 제거
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartDialogue(string npcName, string[] dialogueLines, Sprite portrait = null, System.Action onComplete = null)
    {
        if (dialogueLines == null || dialogueLines.Length == 0) return;

        lines = dialogueLines;
        currentLine = 0;
        onDialogueComplete = onComplete;

        nameText.text = npcName;
        dialogueText.text = lines[currentLine];
        if (portraitImage && portrait) portraitImage.sprite = portrait;

        dialoguePanel.SetActive(true);
    }

    void Update()
    {
        if (!dialoguePanel.activeSelf || !Input.GetKeyDown(KeyCode.Z)) return;

        Debug.Log($"[DEBUG] currentLine: {currentLine}");
        Debug.Log($"[DEBUG] lines: {lines}");
        Debug.Log($"[DEBUG] dialogueText: {dialogueText}");

        currentLine++;
        if (currentLine < lines.Length)
        {
            dialogueText.text = lines[currentLine];
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        Debug.Log("💬 EndDialogue() 호출됨");
        dialoguePanel.SetActive(false);
        onDialogueComplete?.Invoke();
    }

    public bool IsDialogueActive() => dialoguePanel.activeSelf;

    public void ForceCloseDialogue()
    {
        dialoguePanel.SetActive(false);
        onDialogueComplete?.Invoke();
    }
}
