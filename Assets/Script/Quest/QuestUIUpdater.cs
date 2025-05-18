using UnityEngine;
using TMPro;
using System.Text;
using System.Collections.Generic;

public class QuestUIUpdater : MonoBehaviour
{
    public TextMeshProUGUI questText;

    void Update()
    {
        var quests = QuestManager.instance?.activeQuests;

        if (quests == null || quests.Count == 0)
        {
            questText.text = "";
            return;
        }

        StringBuilder sb = new StringBuilder();

        foreach (var q in quests)
        {
            switch (q.state)
            {
                case QuestState.InProgress:
                    sb.AppendLine($"{q.data.questID} {q.currentAmount}/{q.data.requiredAmount}");
                    break;
                case QuestState.Completed:
                    sb.AppendLine($"[¿Ï·á] {q.data.questID}");
                    break;
            }
        }

        questText.text = sb.ToString();
    }

}