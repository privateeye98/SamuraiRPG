using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestEntryUI : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI progressText;
    public Image progressBar;

    Quest questData;

    public void Initialize(Quest q)
    {
        questData = q;
        titleText.text = q.data.questTitle;
        UpdateProgress();

        QuestManager.OnQuestProgress += OnQuestProgress;
        QuestManager.OnQuestCompleted += OnQuestCompleted;
    }

    void OnDestroy()
    {
        QuestManager.OnQuestProgress -= OnQuestProgress;
        QuestManager.OnQuestCompleted -= OnQuestCompleted;
    }

    void OnQuestProgress(Quest q)
    {
        if (q == questData) UpdateProgress();
    }
    void OnQuestCompleted(QuestData d)
    {
        if (d == questData.data) UpdateProgress();
    }

    void UpdateProgress()
    {
        int cur = questData.currentAmount;
        int req = questData.data.requiredAmount;
        progressText.text = $"{cur}/{req}";
        if (progressBar != null)
            progressBar.fillAmount = (float)cur / req;
    }
}
