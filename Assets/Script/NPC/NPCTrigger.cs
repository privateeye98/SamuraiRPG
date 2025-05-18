using UnityEngine;

public class NPCTrigger : MonoBehaviour
{
    public string npcName;
    public Sprite portrait;
    [TextArea] public string[] greetDialogue;
    public QuestData questData;

    bool isPlayerInRange = false;

    void Update()
    {
        if (!isPlayerInRange) return;
        if (DialogueManager.instance.IsDialogueActive()) return;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            DialogueManager.instance.StartDialogue(npcName, greetDialogue, portrait, () =>
            {
                // NPCOptionsUI가 null이면 호출하지 않음
                if (NPCOptionsUI.instance != null)
                {
                    NPCOptionsUI.instance.OpenOptions(this);
                }
                else
                {
                    Debug.LogError("NPCOptionsUI.instance가 null입니다. Singleton 초기화가 아직 안 되었을 수 있음.");
                }
            });
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            isPlayerInRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            isPlayerInRange = false;
    }
}
