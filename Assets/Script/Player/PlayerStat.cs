using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerStat : MonoBehaviour
{
    public static PlayerStat instance;

    // 기본값
    private const int BASE_HP = 100;
    private const int BASE_MP = 50;
    private const int BASE_STR = 10;
    private const int BASE_DEX = 5;
    private const int BASE_CRIT = 1;
    public float expMultiplier = 1f;
    // 최종 적용된 스탯
    public int maxHP;
    public int maxMP;
    public int strength;
    public int dexterity;
    public int critical;

    public int currentHP;
    public int currentMP;

    // 레벨업 보너스 누적 값
    private int bonusHP = 0, bonusMP = 0;
    private int bonusSTR = 0, bonusDEX = 0, bonusCRIT = 0;

    // 장비 적용용
    private Dictionary<ItemPartType, ItemData> equippedItems;
    public event Action OnStatChanged;
    void Awake()
    {
        instance = this;
    }

    public void LevelUpBonus(int level)
    {
        bonusHP += 10;
        bonusMP += 5;
        bonusSTR += 2;
        bonusDEX += 1;

        if (level % 3 == 0)
            bonusCRIT += 1;

        Debug.Log($"[레벨업 보너스] HP+10, STR+2, DEX+1, (Lv%3→CRIT+1)");

        RecalculateStats();
        currentHP = maxHP;
        currentMP = maxMP;
    }

    public void ApplyEquipmentStats(Dictionary<ItemPartType, ItemData> items)
    {
        equippedItems = items;
        RecalculateStats();
    }

    public void RecalculateStats()
    {
        maxHP = BASE_HP + bonusHP;
        maxMP = BASE_MP + bonusMP;
        strength = BASE_STR + bonusSTR;
        dexterity = BASE_DEX + bonusDEX;
        critical = BASE_CRIT + bonusCRIT;

        if (equippedItems != null)
        {
            foreach (var pair in equippedItems)
            {
                var item = pair.Value;
                int lv = item.level;

                maxHP += item.hpBonusPerLevel * lv;
                maxMP += item.mpBonusPerLevel * lv;
                strength += item.strBonusPerLevel * lv;
                dexterity += item.dexBonusPerLevel * lv;
                critical += item.critBonusPerLevel * lv;
            }
        }

        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        currentMP = Mathf.Clamp(currentMP, 0, maxMP);


        Debug.Log($"[최종 스탯] HP:{maxHP}, MP:{maxMP}, STR:{strength}, DEX:{dexterity}, CRIT:{critical}");

        OnStatChanged?.Invoke();
    }

    public int GetAttackDamage()
    {
        int baseDamage = 5;
        float strengthBonus = strength * 2f;
        float dexBonus = dexterity * 1.2f;
        return Mathf.RoundToInt(baseDamage + strengthBonus + dexBonus);
    }

    public bool IsCriticalHit()
    {
        return UnityEngine.Random.Range(0f, 100f) < critical;
    }
    public bool UseMana(int amount)
    {
        if (currentMP >= amount)
        {
            currentMP -= amount;
            OnStatChanged?.Invoke();
            return true;
        }
        return false;
    }
    public float GetCriticalMultiplier()
    {
        return 1.5f;
    }
    public void NotifyStatChanged()
    {
        OnStatChanged?.Invoke();
    }
    public void RecoverMana(int amount)
    {
        currentMP = Mathf.Min(currentMP + amount, maxMP);
        NotifyStatChanged();
    }
}

