using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public static event Action<QuestData> OnQuestAccepted;
    public List<Quest> activeQuests = new List<Quest>();


    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // 중복 제거
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

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

    public void AcceptQuest(QuestData data)
    {
        if (data == null) return;

        // 이미 수락(또는 완료)한 퀘스트라면 팝업만 띄우고 종료
        if (HasQuest(data))
        {
            Quest quest = activeQuests.Find(q => q.data == data);
            string msg = quest.state == QuestState.Completed
                       ? "이미 완료한 퀘스트입니다."
                       : "이미 수락한 퀘스트입니다.";
            QuestPopupUI.instance?.ShowProgress(msg);
            Debug.LogWarning($"[Quest] 중복 수락 시도: {data.questID}");
            return;
        }

        // 신규 퀘스트 등록
        Quest newQuest = new Quest(data) { state = QuestState.InProgress };
        activeQuests.Add(newQuest);
        OnQuestAccepted?.Invoke(data);
        // UI 알림
        QuestPopupUI.instance?.ShowAccept(newQuest);  // "퀘스트 수락됨!" 등
        Debug.Log($"[Quest] 수락: {data.questID}");
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

