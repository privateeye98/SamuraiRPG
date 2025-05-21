using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class QuestSelectionUI : MonoBehaviour
{
    public static QuestSelectionUI instance;

    [Header("UI 구성요소")]
    public GameObject questButtonPrefab;
    public Transform contentParent;
    public TextMeshProUGUI npcNameText;

    private NPCQuestGiver currentGiver;

    void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    public void Open(NPCQuestGiver giver)
    {
        currentGiver = giver;
        gameObject.SetActive(true);
        npcNameText.text = giver.name;

        // 기존 버튼 제거
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var quest in giver.availableQuests)
        {
            if (!QuestManager.instance.HasQuest(quest))
            {
                GameObject buttonObj = Instantiate(questButtonPrefab, contentParent);
                buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = quest.questTitle;

                buttonObj.GetComponent<Button>().onClick.AddListener(() =>
                {
                    QuestManager.instance.AcceptQuest(quest);
                    giver.UpdateQuestIndicator();
                    gameObject.SetActive(false);
                });
            }
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
