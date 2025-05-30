using UnityEngine;

public class QuestLogUI : MonoBehaviour
{
    [Header("UI 오브젝트 연결")]
    public GameObject questLogPanel;      // 퀘스트창 패널 (켜고/끄기)
    public Transform contentParent;       // Content 오브젝트 (Vertical Layout Group)
    public GameObject questEntryPrefab;   // QuestEntryUI 프리팹

    // 크기 동적 조절
    public RectTransform questLogPanelRect;   // Panel의 RectTransform
    public RectTransform contentRect;
    void Awake()
    {
        // 시작 시 퀘스트창 끔
        questLogPanel.SetActive(false);
    }

    void Update()
    {
        // J키로 토글
        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log("J키 눌림!");
            if (questLogPanel.activeSelf) CloseLog();
            else OpenLog();
        }
    }

    public void OpenLog()
    {
        Debug.Log("퀘스트 로그 오픈!");
        RefreshLog();
        questLogPanel.SetActive(true);
    }
     void LateUpdate()
    {
        float minHeight = 80f;  // 퀘스트 하나도 없을 때 최소 높이
        float maxHeight = 800f; // 너무 많을 때 최대 높이
        float targetHeight = Mathf.Clamp(contentRect.rect.height, minHeight, maxHeight);

        questLogPanelRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, targetHeight);

    }
    public void CloseLog()
    {
        questLogPanel.SetActive(false);
    }

    void RefreshLog()
    {
        Debug.Log("퀘스트 로그 새로고침! 현재 퀘스트 수: " + QuestManager.instance.activeQuests.Count);

        foreach (Transform t in contentParent) Destroy(t.gameObject);

        foreach (var quest in QuestManager.instance.activeQuests)
        {
            Debug.Log("퀘스트 출력: " + quest.data.questTitle);

            var go = Instantiate(questEntryPrefab, contentParent);

            // (4) 자식 텍스트 오브젝트 찾기
            var titleText = go.transform.Find("TitleText")?.GetComponent<TMPro.TextMeshProUGUI>();
            var infoText = go.transform.Find("InfoText")?.GetComponent<TMPro.TextMeshProUGUI>();

            if (titleText == null) Debug.LogError("TitleText 연결 안됨!");
            if (infoText == null) Debug.LogError("InfoText 연결 안됨!");

            // (5) 제목/진행 정보 출력
            titleText.text = $"[{quest.data.questTitle}]";

            string typeStr = quest.data.conditionType switch
            {
                QuestConditionType.KillTarget => "Kill",
                QuestConditionType.CollectItem => "Collect",
                QuestConditionType.TalkToNPC => "Talk",
                QuestConditionType.ReachLocation => "Reach",
                _ => ""
            };

            infoText.text = $"- {typeStr} {quest.currentAmount} / {quest.data.requiredAmount}";
        }
    }
}
