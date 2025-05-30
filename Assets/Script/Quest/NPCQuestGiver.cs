using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCQuestGiver : MonoBehaviour
{
    public List<QuestData> availableQuests;

    public GameObject questIndicator;

    IEnumerator Start()
    {
        yield return new WaitUntil(() => QuestManager.instance != null);
        UpdateQuestIndicator();
    }

    public void UpdateQuestIndicator()
    {
        if (QuestManager.instance == null) return;

        bool hasAvailable = false;
        foreach (var quest in availableQuests)
        {
            if (!QuestManager.instance.HasQuest(quest))
            {
                hasAvailable = true;
                break;
            }
        }

        if (questIndicator != null)
        {
            questIndicator.SetActive(hasAvailable);
        }
    }

    public void Interact()
    {
        QuestSelectionUI.instance.Open(this);

        foreach (var quest in QuestManager.instance.activeQuests)
        {
            if (quest.data.conditionType == QuestConditionType.CollectItem &&
                quest.data.npcName == this.name && // 이 NPC가 목표 NPC
                quest.state == QuestState.InProgress)
            {
                int count = Inventory.instance.GetItemCount(quest.data.targetName);
                if (count >= quest.data.requiredAmount)
                {
                    Inventory.instance.RemoveItemByName(quest.data.targetName, quest.data.requiredAmount);
                    quest.Complete();
                }
            }
        }
    }
}
