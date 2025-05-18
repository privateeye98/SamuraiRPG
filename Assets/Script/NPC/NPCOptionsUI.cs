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
        var quest = currentNPC.questData;
        if (quest == null)
        {
            Debug.LogWarning("퀘스트 없음");
            return;
        }

        DialogueManager.instance.StartDialogue(quest.npcName, quest.dialogueLines, currentNPC.portrait, () =>
        {
            questDecisionPanel.SetActive(true);
        });

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
        Debug.Log("▶ AcceptQuest() 호출됨");

        if (currentNPC == null)
        {
            Debug.LogError("❌ currentNPC == null");
            return;
        }

        if (QuestManager.instance == null)
        {
            Debug.LogError("❌ QuestManager.instance == null");
            return;
        }

        if (currentNPC.questData == null)
        {
            Debug.LogError("❌ currentNPC.questData == null");
            return;
        }

        QuestManager.instance.AcceptQuest(currentNPC.questData);

        questDecisionPanel.SetActive(false);
        Time.timeScale = 1f;
    }



    public void DeclineQuest()
    {
        DialogueManager.instance.ForceCloseDialogue();
        questDecisionPanel.SetActive(false);
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
