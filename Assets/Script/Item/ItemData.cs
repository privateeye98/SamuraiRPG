using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    // ��������������������������������������������������������������������������������������������������������������������������������
    // 1) �⺻ ����
    // ��������������������������������������������������������������������������������������������������������������������������������
    [Header("���� �⺻ ���� ����")]
    [Tooltip("������ ���� ID")]
    public int id;

    [Tooltip("�κ��丮�� ǥ�õ� �̸�")]
    public string itemName;

    [Tooltip("�κ��丮 �� ������ ����� ������ ��������Ʈ")]
    public Sprite icon;

    [Tooltip("������ Ÿ�� (Consumable, Equipment, Quest ��)")]
    public ItemType type;

    [Tooltip("��� ��Ʈ (Head, Body, Weapon ��)")]
    public ItemPartType part;

    [TextArea(2, 5)]
    [Tooltip("���� �� ������ ���� ����")]
    public string description;

    [Tooltip("�κ��丮�� ���� �� �ִ��� ���� (�Һ� �����۸�)")]
    public bool isStackable = false;

    [Tooltip("�ִ� ���� �� (��: ���� 200������)")]
    public int maxStack = 1;

    [Tooltip("�������� �Ǹ� �� ȸ���� ��� ���� (0.0 ~ 1.0)")]
    [Range(0f, 1f)]
    public float sellRatio = 0.5f;

    [Tooltip("�������� ���� �� �⺻ ����")]
    public int price;

    [Space(10)]
    // ��������������������������������������������������������������������������������������������������������������������������������
    // 2) ���� �ּ� ����
    // ��������������������������������������������������������������������������������������������������������������������������������
    [Header("���� ���� �ּ� ���� ����")]
    [Tooltip("���� ������ �ּ� ����")]
    public int requiredLevel = 1;

    [Tooltip("���� �������� �ʿ��� ���� ��� (��: STR �� 10)")]
    public StatRequirement[] requiredStats;

    [Serializable]
    public struct StatRequirement
    {
        [Tooltip("�ʿ��� ���� ���� (STR, DEX, CRIT ��)")]
        public StatType stat;
        [Tooltip("�ʿ��� �ּ� ��")]
        public int value;
    }

    [Space(10)]
    // ��������������������������������������������������������������������������������������������������������������������������������
    // 3) �⺻ ���� (Base Stats)
    // ��������������������������������������������������������������������������������������������������������������������������������
    [Header("���� �⺻ ���� (���� �� ���� ���ʽ�) ����")]
    [Tooltip("��� ���� �� �߰��Ǵ� ���ݷ� (������)")]
    public int baseATK;

    [Tooltip("��� ���� �� �߰��Ǵ� ���� (������)")]
    public int baseDEF;

    [Tooltip("��� ���� �� �߰��Ǵ� �ִ� ü�� (������)")]
    public int baseHP;

    [Tooltip("��� ���� �� �߰��Ǵ� �ִ� ���� (������)")]
    public int baseMP;

    [Tooltip("��� ���� �� �߰��Ǵ� STR (������)")]
    public int baseSTR;

    [Tooltip("��� ���� �� �߰��Ǵ� DEX (������)")]
    public int baseDEX;

    [Tooltip("��� ���� �� �߰��Ǵ� CRIT Ȯ�� (������, �ۼ�Ʈ)")]
    public int baseCRIT;


    [Space(10)]
    // ��������������������������������������������������������������������������������������������������������������������������������
    // 4) ��ȭ ������ ���ʽ� (Per-Level Bonuses)
    // ��������������������������������������������������������������������������������������������������������������������������������
    [Header("���� ��ȭ ������ ���ʽ� (Per Level) ����")]
    [Tooltip("��ȭ 1������ �߰��Ǵ� ���ݷ�")]
    public int perLevelATK;

    [Tooltip("��ȭ 1������ �߰��Ǵ� ����")]
    public int perLevelDEF;

    [Tooltip("��ȭ 1������ �߰��Ǵ� ü��")]
    public int perLevelHP;

    [Tooltip("��ȭ 1������ �߰��Ǵ� ����")]
    public int perLevelMP;

    [Tooltip("��ȭ 1������ �߰��Ǵ� STR")]
    public int perLevelSTR;

    [Tooltip("��ȭ 1������ �߰��Ǵ� DEX")]
    public int perLevelDEX;

    [Tooltip("��ȭ 1������ �߰��Ǵ� CRIT Ȯ�� (�ۼ�Ʈ)")]
    public int perLevelCRIT;


    [Space(10)]
    // ��������������������������������������������������������������������������������������������������������������������������������
    // 5) ��ȭ ���� ����
    // ��������������������������������������������������������������������������������������������������������������������������������
    [Header("���� ��ȭ ���� ���� ����")]
    [Tooltip("������ ��ȭ �ִ� ����")]
    public int maxLevel = 10;

    [Tooltip("��ȭ ��� (������ �������� �ݾ�)")]
    public int upgradeCost = 100;

    [Tooltip("������ �ּ� ���� Ȯ�� (��ȭ ���� �ÿ��� ����)")]
    public float minSuccessRate = 0.1f;

    [Tooltip("�⺻ ��ȭ ���� Ȯ�� (��: 1��2���� �� 50%)")]
    public float baseSuccessRate = 0.5f;

    [Tooltip("��ȭ ������ �г�Ƽ (��: ���� �ϳ��� -1% ����)")]
    public float penaltyPerLevel = 0.01f;

    /// <summary>
    /// ���� ���� Ȯ�� ��� (currentLevel ���� ���ڷ� �޾� ���).
    /// </summary>
    public float GetSuccessRate(int currentLevel)
    {
        float rate = baseSuccessRate - (currentLevel * penaltyPerLevel);
        return Mathf.Clamp(rate, minSuccessRate, 1f);
    }


    [Space(10)]
    // ��������������������������������������������������������������������������������������������������������������������������������
    // 6) ��Ÿ �Ӽ� (�Һ� ������ ��)
    // ��������������������������������������������������������������������������������������������������������������������������������
    [Header("���� �Һ� ������ �Ӽ� ����")]
    [Tooltip("���� (�Һ� ������)")]
    public int healAmount;

    [Tooltip("���� ȸ���� (�Һ� ������)")]
    public int ManaAmount;

    [Tooltip("������ �߰��� ������ ���� ���� (�ʿ� �� Ȯ��)")]
    public StatType statType;  // (��: ������ ���� �������� CRIT Ȯ���� ������ŵ�ϴ١� ��)


    [Space(10)]
    // ��������������������������������������������������������������������������������������������������������������������������������
    // 7) ��Ÿ�ӿ� ���� �ʵ� (�ν����� ���� ���ʿ�)
    // ��������������������������������������������������������������������������������������������������������������������������������
    [HideInInspector]
    public StatModifier[] bonusStats; // InventoryItem.PopulateBonusStats() �ܰ迡�� ä�����ϴ�.

    [Serializable]
    public struct StatModifier
    {
        public StatType stat;
        public int amount; // Per-Level ���ʽ� ��
    }
}
