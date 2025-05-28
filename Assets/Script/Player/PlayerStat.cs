using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public static PlayerStat instance;

    public int MinDamage => Mathf.RoundToInt(GetBaseAttack() * 0.9f);
    public int MaxDamage => Mathf.RoundToInt(GetBaseAttack() * 1.1f);

    private const int BASE_HP = 100;
    private const int BASE_MP = 50;
    private const int BASE_STR = 10;
    private const int BASE_DEX = 5;
    private const int BASE_CRIT = 1;

    public float expMultiplier = 1f;

    public int maxHP;
    public int maxMP;
    public int strength;
    public int dexterity;
    public int critical;

    public int currentHP;
    public int currentMP;

    private int bonusHP = 0;
    private int bonusMP = 0;
    private int bonusSTR = 0;
    private int bonusDEX = 0;
    private int bonusCRIT = 0;

    private Dictionary<ItemPartType, ItemData> equippedItems;

    private Dictionary<StatType, int> equipmentBonuses = new Dictionary<StatType, int>();

    public event Action OnStatChanged;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void LevelUpBonus(int level)
    {
        bonusHP += 10;
        bonusMP += 5;
        bonusSTR += 2;
        bonusDEX += 1;
        if (level % 3 == 0) bonusCRIT += 1;

        Debug.Log($"[������ ���ʽ�] HP+10, MP+5, STR+2, DEX+1, (Lv%3��CRIT+1)");

        RecalculateStats();
        currentHP = maxHP;
        currentMP = maxMP;
    }
    public void ApplyEquipmentStats(
        Dictionary<ItemPartType, ItemData> items,
        Dictionary<ItemPartType, int> levels)
    {
        equippedItems = items;
        RecalculateStats(levels);
    }


    public void ResetEquipmentBonuses()
    {
        equipmentBonuses.Clear();
    }

    public void AddEquipmentBonus(StatType stat, int amount)
    {
        if (!equipmentBonuses.ContainsKey(stat))
            equipmentBonuses[stat] = 0;
        equipmentBonuses[stat] += amount;
    }

    public int GetStat(StatType stat)
    {
        int baseValue = stat switch
        {
            StatType.HP => BASE_HP + bonusHP,
            StatType.MP => BASE_MP + bonusMP,
            StatType.STR => BASE_STR + bonusSTR,
            StatType.DEX => BASE_DEX + bonusDEX,
            StatType.CRIT => BASE_CRIT + bonusCRIT,
            _ => 0
        };
        equipmentBonuses.TryGetValue(stat, out int bonus);
        return baseValue + bonus;
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
                int lv = 1;
                maxHP += item.hpBonusPerLevel * lv;
                maxMP += item.mpBonusPerLevel * lv;
                strength += item.strBonusPerLevel * lv;
                dexterity += item.dexBonusPerLevel * lv;
                critical += item.critBonusPerLevel * lv;
            }
        }

        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        currentMP = Mathf.Clamp(currentMP, 0, maxMP);

        Debug.Log($"[���� ����] HP:{maxHP}, MP:{maxMP}, STR:{strength}, DEX:{dexterity}, CRIT:{critical}");
        OnStatChanged?.Invoke();
    }

    public void RecalculateStats(Dictionary<ItemPartType, int> levels)
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
                int lv = levels.ContainsKey(pair.Key) ? levels[pair.Key] : 1;
                maxHP += item.hpBonusPerLevel * lv;
                maxMP += item.mpBonusPerLevel * lv;
                strength += item.strBonusPerLevel * lv;
                dexterity += item.dexBonusPerLevel * lv;
                critical += item.critBonusPerLevel * lv;
            }
        }

        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        currentMP = Mathf.Clamp(currentMP, 0, maxMP);

        Debug.Log($"[���� ����] HP:{maxHP}, MP:{maxMP}, STR:{strength}, DEX:{dexterity}, CRIT:{critical}");
        OnStatChanged?.Invoke();
    }

    private float GetBaseAttack()
    {
        float baseatk = 5;
        float strBonus = strength * 2f;
        float dexBonus = dexterity * 1.2f;
        float weaponbonus = 0f;

        if (equippedItems != null &&
            equippedItems.TryGetValue(ItemPartType.Weapon, out var weapon))
        {
            weaponbonus += weapon.strBonusPerLevel * weapon.level;
            weaponbonus += weapon.dexBonusPerLevel * weapon.level;
        }
        return baseatk + strBonus + dexBonus + weaponbonus;
    }

    public int GetAttackDamage()
    {
        float raw = UnityEngine.Random.Range(MinDamage, MaxDamage + 1);
        if (IsCriticalHit())
            raw *= GetCriticalMultiplier();
        return Mathf.RoundToInt(raw);
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

    public float GetCriticalMultiplier() => 1.2f;

    public void RecoverMana(int amount)
    {
        currentMP = Mathf.Min(currentMP + amount, maxMP);
        OnStatChanged?.Invoke();
    }

    public void NotifyStatChanged() => OnStatChanged?.Invoke();
}
