using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public List<Quest> activeQuests = new List<Quest>();
    public QuestData GetQuestDataByID(string id)
    {
    
        QuestData[] allData = Resources.LoadAll<QuestData>("Quest"); 
        foreach (var q in allData)
        {
            if (q.questID == id)
                return q;
        }
        Debug.LogWarning($"QuestData ID '{id}'를 찾을 수 없습니다.");
        return null;
    }
    void Awake()
    {
        if (instance && instance != this) { Destroy(gameObject); return; }
        instance = this;

        if (instance != null && instance != this)
        {
            Destroy(gameObject); // 중복 제거
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AcceptQuest(QuestData data)
    {
        foreach (var quest in activeQuests)
        {
            if (quest.data == data)
            {
                if (quest.state == QuestState.Completed)
                {
                    QuestPopupUI.instance?.ShowProgress("이미 완료한 퀘스트입니다.");
                }
                else
                {
                    QuestPopupUI.instance?.ShowProgress("이미 수락한 퀘스트입니다.");
                }
                return;
            }
        }

        Quest newQuest = new Quest(data);
        newQuest.state = QuestState.InProgress;
        activeQuests.Add(newQuest);

        QuestPopupUI.instance?.ShowProgress("퀘스트 수락됨!");
    }
    public bool HasQuest(QuestData data)
    {
        return activeQuests.Exists(q => q.data == data);
    }
    public void UpdateQuestProgress(string target)
    {
        foreach (var quest in activeQuests)
        {
            quest.Progress(target);  // 모든 진행 중 퀘스트에 대해 처리
        }
    }
}

