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
    }
}
