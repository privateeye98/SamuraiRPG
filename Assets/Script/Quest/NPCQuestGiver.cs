using System.Collections.Generic;
using UnityEngine;

public class NPCQuestGiver : MonoBehaviour
{
    public List<QuestData> availableQuests;

    public GameObject questIndicator;

    void Start()
    {
        UpdateQuestIndicator();
    }

    public void UpdateQuestIndicator()
    {
        bool hasAvailable = false;
        foreach (var quest in availableQuests)
        {
            if (!QuestManager.instance.HasQuest(quest))
            {
                hasAvailable = true;
                break;
            }
        }
        questIndicator.SetActive(hasAvailable);
    }

    public void Interact()
    {
        QuestSelectionUI.instance.Open(this);
    }
}
