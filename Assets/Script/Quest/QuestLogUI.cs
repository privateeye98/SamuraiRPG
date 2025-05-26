using UnityEngine;

public class QuestLogUI : MonoBehaviour
{
    [Header("UI ÂüÁ¶")]
    public GameObject questLogPanel;
    public Transform contentParent;
    public GameObject questEntryPrefab;

    void Awake()
    {
        questLogPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (questLogPanel.activeSelf) CloseLog();
            else OpenLog();
        }
    }

    public void OpenLog()
    {
        RefreshLog();
        questLogPanel.SetActive(true);
    }

    public void CloseLog()
    {
        questLogPanel.SetActive(false);
    }

    void RefreshLog()
    {
        foreach (Transform t in contentParent) Destroy(t.gameObject);
        foreach (var quest in QuestManager.instance.activeQuests)
        {
            var entry = Instantiate(questEntryPrefab, contentParent)
                             .GetComponent<QuestEntryUI>();
            entry.Initialize(quest);
        }
    }
}
