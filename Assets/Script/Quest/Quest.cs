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
            Debug.Log($"[퀘스트 진행] {data.questID}: {currentAmount}/{data.requiredAmount}");

            QuestManager.ReportProgress(this);


            if (currentAmount >= data.requiredAmount)
            {
                Complete();
            }
        }
    }
    public void Complete()
    {
        state = QuestState.Completed;

        Debug.Log($"[퀘스트 완료] {data.questTitle}");


        if (data.rewardGold > 0)
        {
            GoldManager.instance?.AddGold(data.rewardGold);
            Debug.Log($"+{data.rewardGold} 골드 획득");
        }

        if (data.rewardItemId != 0)
        {
            ItemData rewardItem = ItemDatabase.instance.GetItemById(data.rewardItemId);

            if (rewardItem != null)
            {
                if (Inventory.instance.HasRoom())
                {
                    for (int i = 0; i < data.rewardItemAmount; i++)
                        Inventory.instance.AddItem(rewardItem);
                }
                Debug.Log($"+{data.rewardItemAmount}개 [{rewardItem.itemName}] 획득");
            }
            else
            {
                Debug.LogWarning($"[보상 실패] ID {data.rewardItemId}에 해당하는 아이템 없음");
            }
        }
    }


}
