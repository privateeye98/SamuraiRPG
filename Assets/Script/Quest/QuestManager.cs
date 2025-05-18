using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public List<Quest> activeQuests = new List<Quest>();

    void Awake()
    {
        if (instance && instance != this) { Destroy(gameObject); return; }
        instance = this;
    }

    public void AcceptQuest(QuestData data)
    {
        foreach (var quest in activeQuests)
        {
            if (quest.data == data)
            {
                if (quest.state == QuestState.Completed)
                {
                    QuestPopupUI.instance?.ShowProgress("🏁 이미 완료한 퀘스트입니다.");
                }
                else
                {
                    QuestPopupUI.instance?.ShowProgress("⚠ 이미 수락한 퀘스트입니다.");
                }
                return;
            }
        }

        Quest newQuest = new Quest(data);
        newQuest.state = QuestState.InProgress;
        activeQuests.Add(newQuest);

        QuestPopupUI.instance?.ShowProgress("✅ 퀘스트 수락됨!");
    }



    public void UpdateQuestProgress(string target)
    {
        foreach (var quest in activeQuests)
        {
            quest.Progress(target);  // 모든 진행 중 퀘스트에 대해 처리
        }
    }
}

