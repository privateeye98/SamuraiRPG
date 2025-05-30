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
    public void Progress(string target = null)
    {
        switch (data.conditionType)
        {
            case QuestConditionType.KillTarget:
                if (target == data.targetName)
                {
                    currentAmount++;
                    if (currentAmount >= data.requiredAmount) Complete();
                }
                break;

            case QuestConditionType.CollectItem:
                int invCount = Inventory.instance.GetItemCount(data.targetName);
                currentAmount = invCount;
                if (currentAmount >= data.requiredAmount)
                {
                    Inventory.instance.RemoveItemByName(data.targetName, data.requiredAmount);
                    Complete();
                }
                break;

            case QuestConditionType.TalkToNPC:
                if (target == data.targetName)
                    Complete();
                break;
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
