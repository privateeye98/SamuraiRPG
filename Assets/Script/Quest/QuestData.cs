using UnityEngine;

public enum QuestConditionType { Kill, Gather, Talk }

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quest/QuestData")]
public class QuestData : ScriptableObject
{
    public string questID;
    public string npcName;

    [Header("����")]
    public QuestConditionType conditionType;
    public string targetName;
    public int requiredAmount = 1;
    public int rewardGold;

    [TextArea]
    public string[] dialogueLines;
}
