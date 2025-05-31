using UnityEngine;
using UnityEngine.UI;
public class NPCOptionsUI : MonoBehaviour
{
    public static NPCOptionsUI instance;

    [Header("UI")]
    public GameObject optionsPanel;
    public GameObject questDecisionPanel;
    public Button questButton;
    NPCTrigger currentNPC;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        Debug.Log($"NPCOptionsUI Awake – ID:{GetInstanceID()}");

    }

    public void OpenOptions(NPCTrigger npc)
    {
        Debug.Log("OpenOptions 호출됨, 버튼 패널 열기 시도");
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
        if (currentNPC == null)
        {
            Debug.LogError("currentNPC가 null입니다. 먼저 Z키로 NPC와 상호작용했는지 확인!");
            return;
        }

        var quests = currentNPC.availableQuests;
        if (quests == null || quests.Count == 0)
        {
            Debug.LogWarning("퀘스트가 없습니다.");
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
        if (QuestManager.instance == null)
        {
            Debug.LogError("QuestManager 인스턴스를 찾을 수 없습니다! 씬에 QuestManager 오브젝트가 있는지 확인하세요.");
            return;
        }

        if (currentNPC == null)
        {
            Debug.LogError("currentNPC가 null입니다. 먼저 Z키로 NPC와 상호작용했는지 확인!");
            return;
        }

        var quests = currentNPC.availableQuests;
        if (quests == null || quests.Count == 0)
        {
            Debug.LogWarning("퀘스트가 없습니다.");
            return;
        }

        if (currentNPC == null || currentNPC.availableQuests == null || currentNPC.availableQuests.Count == 0)
        {
            Debug.LogError("퀘스트 데이터가 없습니다");
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
    }

    void CloseAll()
    {
        DialogueManager.instance.ForceCloseDialogue();
        optionsPanel.SetActive(false);
        questDecisionPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}