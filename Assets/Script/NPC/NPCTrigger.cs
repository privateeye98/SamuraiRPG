using UnityEngine;
using System.Collections.Generic;

public class NPCTrigger : MonoBehaviour
{
    public GameObject questIndicator;
    public string npcName;
    public Sprite portrait;
    [TextArea] public string[] greetDialogue;
    public List<QuestData> availableQuests;
    bool isPlayerInRange = false;

    void Start()
    {
        RefreshQuestIndicator();

    }
    void OnEnable()
    {
        QuestManager.OnQuestAccepted += RefreshQuestIndicator;
    }
    void OnDisable()
    {
        QuestManager.OnQuestAccepted -= RefreshQuestIndicator;
    }

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
    void RefreshQuestIndicator(QuestData _)
    {
        // 이미 수락된 퀘스트는 숨기기
        availableQuests.RemoveAll(q => QuestManager.instance.HasQuest(q));
        questIndicator.SetActive(availableQuests.Count > 0);
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
    public void RefreshQuestIndicator()
    {
        // 아직 수락하지 않은 퀘스트만 남김
        availableQuests.RemoveAll(q => QuestManager.instance.HasQuest(q));
        // 느낌표 On/Off
        if (questIndicator != null)
            questIndicator.SetActive(availableQuests.Count > 0);
    }

}