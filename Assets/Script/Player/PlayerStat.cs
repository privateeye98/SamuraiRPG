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
            Debug.Log("PlayerStat Awake: instance 할당됨");
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

        Debug.Log($"[레벨업 보너스] HP+10, MP+5, STR+2, DEX+1, (Lv%3→CRIT+1)");

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
        // 1) 레벨업 보너스가 반영된 캐릭터 고유값 세팅
        maxHP = BASE_HP + bonusHP;
        maxMP = BASE_MP + bonusMP;
        strength = BASE_STR + bonusSTR;
        dexterity = BASE_DEX + bonusDEX;
        critical = BASE_CRIT + bonusCRIT;

        // 2) 장착된 아이템이 있으면, 각각의 “기본 스탯(Base)”과 “강화 레벨당 보너스(Per-Level)”를 계산해 더해 줍니다.
        if (equippedItems != null)
        {
            foreach (var pair in equippedItems)
            {
                ItemData item = pair.Value;
                // (A) 아이템의 “기본 스탯(Base)”을 더해 준다
                if (item.baseHP != 0) maxHP += item.baseHP;
                if (item.baseMP != 0) maxMP += item.baseMP;
                if (item.baseSTR != 0) strength += item.baseSTR;
                if (item.baseDEX != 0) dexterity += item.baseDEX;
                if (item.baseCRIT != 0) critical += item.baseCRIT;
                if (item.baseATK != 0) Debug.LogWarning("baseATK은 전투 계산에서만 사용됩니다.");

                // (B) 아이템의 강화 레벨당 보너스를 계산해 더해 준다
                //     InventoryItem에서 item.level을 관리하므로, 우선 InventoryItem을 직접 참조하는 것이 가장 정확하지만, 
                //     여기서는 “장착된 아이템(ItemData)만 참조”하기 때문에, 레벨 정보를 별도로 전달받아야 합니다.
                //     예를 들어 EquipmentManager.ApplyEquipmentStats에서 “Dictionary<ItemPartType,int> levels”를 넘겨줄 수 있습니다.
                //     현재 simplest 버전에서는 “레벨 1 기준 Per-Level만 적용” 예시로 두지만, 
                //     보다 정확히 하려면 InventoryItem.GetEnhancedStats()를 쓰는 쪽으로 대체하세요.
                // 
                //     ────────────────────────────────────────────────────────────────────
                //     아래 코드는 “강화 레벨이 1인 상태(초기 장착 상태)”만 가정(Per-Level×1).
                //      실제 레벨 정보를 받으려면, RecalculateStats(Dictionary<ItemPartType,int> levels) 메서드를 이용하세요.
                // ────────────────────────────────────────────────────────────────────
                int assumedLevel = 1;
                if (item.perLevelHP != 0) maxHP += item.perLevelHP * assumedLevel;
                if (item.perLevelMP != 0) maxMP += item.perLevelMP * assumedLevel;
                if (item.perLevelSTR != 0) strength += item.perLevelSTR * assumedLevel;
                if (item.perLevelDEX != 0) dexterity += item.perLevelDEX * assumedLevel;
                if (item.perLevelCRIT != 0) critical += item.perLevelCRIT * assumedLevel;
                // perLevelATK은 전투 로직에서만 사용 → 여기선 무시
            }
        }

        // 3) 현재 HP/MP 값이 범위 밖으로 벗어나지 않도록 클램핑
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        currentMP = Mathf.Clamp(currentMP, 0, maxMP);

        Debug.Log($"[최종 스탯] HP:{maxHP}, MP:{maxMP}, STR:{strength}, DEX:{dexterity}, CRIT:{critical}");
        OnStatChanged?.Invoke();
    }

    public void RecalculateStats(Dictionary<ItemPartType, int> levels)
    {
        // 1) 레벨업 보너스가 반영된 캐릭터 고유값 세팅
        maxHP = BASE_HP + bonusHP;
        maxMP = BASE_MP + bonusMP;
        strength = BASE_STR + bonusSTR;
        dexterity = BASE_DEX + bonusDEX;
        critical = BASE_CRIT + bonusCRIT;

        // 2) 장착된 아이템이 있으면, 기본 + 강화 보너스를 “levels” 사전에서 읽어 반영
        if (equippedItems != null)
        {
            foreach (var pair in equippedItems)
            {
                ItemPartType part = pair.Key;
                ItemData item = pair.Value;

                // (A) 기본 스탯(Base) 적용
                if (item.baseHP != 0) maxHP += item.baseHP;
                if (item.baseMP != 0) maxMP += item.baseMP;
                if (item.baseSTR != 0) strength += item.baseSTR;
                if (item.baseDEX != 0) dexterity += item.baseDEX;
                if (item.baseCRIT != 0) critical += item.baseCRIT;

                // (B) “강화 레벨당 보너스(Per-Level)”를 levels에서 읽은 레벨만큼 곱해서 적용
                int level = levels.ContainsKey(part) ? levels[part] : 1;
                if (item.perLevelHP != 0) maxHP += item.perLevelHP * level;
                if (item.perLevelMP != 0) maxMP += item.perLevelMP * level;
                if (item.perLevelSTR != 0) strength += item.perLevelSTR * level;
                if (item.perLevelDEX != 0) dexterity += item.perLevelDEX * level;
                if (item.perLevelCRIT != 0) critical += item.perLevelCRIT * level;
            }
        }

        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        currentMP = Mathf.Clamp(currentMP, 0, maxMP);

        Debug.Log($"[최종 스탯] HP:{maxHP}, MP:{maxMP}, STR:{strength}, DEX:{dexterity}, CRIT:{critical}");
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
            int weaponLevel = 1;
            if (weapon is ItemData)
            {
                weaponLevel = 1;
            }

            weaponbonus += weapon.perLevelSTR * weaponLevel;
            weaponbonus += weapon.perLevelDEX * weaponLevel;
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
