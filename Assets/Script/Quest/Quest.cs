using UnityEngine;
public class Quest
{
    public QuestData data;
    public int currentAmount = 0;
    public QuestState state = QuestState.NotStarted;

    public Quest(QuestData data)
    {
        this.data = data;
    }
    public void Progress(string target)
    {
        if (state != QuestState.InProgress) return;

        if (target == data.targetName)
        {
            currentAmount++;
            Debug.Log($"[Äù½ºÆ® ÁøÇà] {data.questID}: {currentAmount}/{data.requiredAmount}");

            if (currentAmount >= data.requiredAmount)
            {
                Complete();
            }
        }
    }



    public void Complete()
    {
        state = QuestState.Completed;
        GoldManager.instance?.AddGold(data.rewardGold);
    }
}
