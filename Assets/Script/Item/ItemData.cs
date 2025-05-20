using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Iventory/Item")]
public class ItemData : ScriptableObject
{
    [Header("��ȭ Ȯ�� ����")]
    public float baseSuccessRate = 0.5f;       // �⺻ ������ 50%
    public float penaltyPerLevel = 0.01f;       // �ܰ�� -10%
    public float minSuccessRate = 0.1f;

    public int level = 1;
    public int maxLevel = 10;
    public int upgradeCost = 100;

    // -- ���ʽ� ���� 
    public int hpBonusPerLevel;
    public int mpBonusPerLevel;
    public int strBonusPerLevel;
    public int dexBonusPerLevel;
    public int critBonusPerLevel;

    public int id;
    public string itemName;
    public Sprite icon;
    public ItemType type;
    public int healAmount;

    public ItemPartType part;
    public StatType statType; // ������ �ʵ� �߰�

    [TextArea]
    public string description;
    public int price;

    public float GetSuccessRate()
    {
        float rate = baseSuccessRate - (level * penaltyPerLevel);
        return Mathf.Clamp(rate, minSuccessRate, 1f);
    }
}