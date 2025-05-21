using System.Collections.Generic;

[System.Serializable]
public class SavedQuest
{
    public string questID;
    public int currentAmount;
    public QuestState state;

}

public List<SavedQuest> savedQuests = new();
