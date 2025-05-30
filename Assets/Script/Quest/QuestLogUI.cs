using UnityEngine;

public class QuestLogUI : MonoBehaviour
{
    [Header("UI ������Ʈ ����")]
    public GameObject questLogPanel;      // ����Ʈâ �г� (�Ѱ�/����)
    public Transform contentParent;       // Content ������Ʈ (Vertical Layout Group)
    public GameObject questEntryPrefab;   // QuestEntryUI ������

    // ũ�� ���� ����
    public RectTransform questLogPanelRect;   // Panel�� RectTransform
    public RectTransform contentRect;
    void Awake()
    {
        // ���� �� ����Ʈâ ��
        questLogPanel.SetActive(false);
    }

    void Update()
    {
        // JŰ�� ���
        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log("JŰ ����!");
            if (questLogPanel.activeSelf) CloseLog();
            else OpenLog();
        }
    }

    public void OpenLog()
    {
        Debug.Log("����Ʈ �α� ����!");
        RefreshLog();
        questLogPanel.SetActive(true);
    }
     void LateUpdate()
    {
        float minHeight = 80f;  // ����Ʈ �ϳ��� ���� �� �ּ� ����
        float maxHeight = 800f; // �ʹ� ���� �� �ִ� ����
        float targetHeight = Mathf.Clamp(contentRect.rect.height, minHeight, maxHeight);

        questLogPanelRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, targetHeight);

    }
    public void CloseLog()
    {
        questLogPanel.SetActive(false);
    }

    void RefreshLog()
    {
        Debug.Log("����Ʈ �α� ���ΰ�ħ! ���� ����Ʈ ��: " + QuestManager.instance.activeQuests.Count);

        foreach (Transform t in contentParent) Destroy(t.gameObject);

        foreach (var quest in QuestManager.instance.activeQuests)
        {
            Debug.Log("����Ʈ ���: " + quest.data.questTitle);

            var go = Instantiate(questEntryPrefab, contentParent);

            // (4) �ڽ� �ؽ�Ʈ ������Ʈ ã��
            var titleText = go.transform.Find("TitleText")?.GetComponent<TMPro.TextMeshProUGUI>();
            var infoText = go.transform.Find("InfoText")?.GetComponent<TMPro.TextMeshProUGUI>();

            if (titleText == null) Debug.LogError("TitleText ���� �ȵ�!");
            if (infoText == null) Debug.LogError("InfoText ���� �ȵ�!");

            // (5) ����/���� ���� ���
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
