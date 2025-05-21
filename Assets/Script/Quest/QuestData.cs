using UnityEngine;
public enum QuestConditionType
{
    KillTarget,
    CollectItem,
    TalkToNPC,
    ReachLocation
}

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quest/QuestData")]
public class QuestData : ScriptableObject
{
    public string questID;
    public string questTitle;
    public string npcName;

    [Header("조건")]
    public QuestConditionType conditionType;
    public string targetName;
    public int requiredAmount = 1;

    [Header("보상")]
    public int rewardGold;
    public int rewardItemId;

    public int rewardItemAmount = 1;

    [TextArea]
    public string[] dialogueLines;
}

