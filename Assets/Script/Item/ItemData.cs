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
    public int atk;
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
    public StatType statType;


    //�Һ���ø
    public bool isStackable = false;
    public int maxStack = 200;


    [TextArea]
    public string description;
    public int price;

    [Header("���� ���ȱ� ����")]
    [Range(0f, 1f)] public float sellRatio = 0.5f;
    public float GetSuccessRate()
    {
        float rate = baseSuccessRate - (level * penaltyPerLevel);
        return Mathf.Clamp(rate, minSuccessRate, 1f);
    }
    public int SellPrice
    {
        get { return Mathf.FloorToInt(price * sellRatio); }
    }
}