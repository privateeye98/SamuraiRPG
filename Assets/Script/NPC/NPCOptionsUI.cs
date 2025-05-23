using UnityEngine;

public class NPCOptionsUI : MonoBehaviour
{
    public static NPCOptionsUI instance;

    [Header("UI")]
    public GameObject optionsPanel;
    public GameObject questDecisionPanel;

    NPCTrigger currentNPC;

    void Awake()
    {
        instance = this;
        Debug.Log("🟢 NPCOptionsUI Singleton 초기화됨");
    }

    public void OpenOptions(NPCTrigger npc)
    {
        Debug.Log("🟡 OpenOptions 호출됨, 버튼 패널 열기 시도");
        currentNPC = npc;
        optionsPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void OnTalk()
    {
        DialogueManager.instance.StartDialogue(currentNPC.npcName, currentNPC.greetDialogue, currentNPC.portrait, () =>
        {
            optionsPanel.SetActive(true);
        });

        optionsPanel.SetActive(false);
    }

    public void OnQuest()
    {
        var quests = currentNPC.availableQuests;
        if (quests == null || quests.Count == 0)
        {
            Debug.LogWarning("퀘스트 없음");
            return;
        }


        QuestData firstQuest = quests[0];

        DialogueManager.instance.StartDialogue(
            firstQuest.npcName,
            firstQuest.dialogueLines,
            currentNPC.portrait,
            () => questDecisionPanel.SetActive(true)
            );

        optionsPanel.SetActive(false);
    }

    public void OnShop()
    {
        ShopManager.instance.OpenShop();
        CloseAll();
    }

    public void OnClose()
    {
        CloseAll();
    }

    public void AcceptQuest()
    {
        if (currentNPC == null || currentNPC.availableQuests == null || currentNPC.availableQuests.Count == 0)
    {
        Debug.LogError("❌ 퀘스트 데이터가 없습니다");
        return;
    }

    QuestData firstQuest = currentNPC.availableQuests[0]; // 나중엔 선택한 퀘스트로 확장 가능
    QuestManager.instance.AcceptQuest(firstQuest);
        CloseAll();
    questDecisionPanel.SetActive(false);
    Time.timeScale = 1f;

    }



    public void DeclineQuest()
    {
        DialogueManager.instance.ForceCloseDialogue();
        questDecisionPanel.SetActive(false);
        CloseAll();
        Time.timeScale = 1f;
    }

    void CloseAll()
    {
        DialogueManager.instance.ForceCloseDialogue();
        optionsPanel.SetActive(false);
        questDecisionPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
