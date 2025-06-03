using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager instance;

    // 현재 장착된 파트별 InventoryItem 사전
    public Dictionary<ItemPartType, InventoryItem> equippedItems = new Dictionary<ItemPartType, InventoryItem>();

    void Awake()
    {
        // 싱글턴 보장 + 씬 전환 시 파괴되지 않도록 설정
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void EquipItem(InventoryItem item)
    {
        if (item == null) return;
        var part = item.itemData.part;
        var data = item.itemData;

        // 레벨 제한 검사
        if (PlayerLevel.instance != null && PlayerLevel.instance.currentLevel < data.requiredLevel)
        {
            Debug.LogWarning($"Lv {data.requiredLevel} 이상만 착용할 수 있습니다.");
            return;
        }

        // 스탯 제한 검사
        if (PlayerStat.instance == null)
        {
            Debug.LogWarning("PlayerStat.instance가 아직 없습니다.");
            return;
        }
        foreach (var req in data.requiredStats)
        {
            if (PlayerStat.instance.GetStat(req.stat) < req.value)
            {
                Debug.LogWarning($"{req.stat} {req.value} 이상이어야 착용할 수 있습니다.");
                return;
            }
        }

        // 이미 해당 파트에 아이템이 있으면 인벤토리로 되돌리기
        if (equippedItems.TryGetValue(part, out var oldItem))
        {
            Inventory.instance?.items.Add(oldItem);
            equippedItems.Remove(part);
        }

        // 새 아이템 장착
        equippedItems[part] = item;
        Inventory.instance?.items.Remove(item);

        // 스탯 보너스 및 UI 갱신
        ApplyEquipmentEffects();

        // 강화창이 열려 있으면 실시간 갱신
        if (UpgradeUI.instance != null && UpgradeUI.instance.gameObject.activeSelf)
        {
            UpgradeUI.instance.Refresh(equippedItems);
        }

        Inventory.instance?.NotifyItemChanged();
    }

    /// <summary>
    /// 장착 해제
    /// </summary>
    public void UnequipItem(ItemPartType part)
    {
        if (!equippedItems.ContainsKey(part)) return;

        Inventory.instance.items.Add(equippedItems[part]);
        equippedItems.Remove(part);

        ApplyEquipmentEffects();

        // 강화창이 열려 있으면 실시간 갱신
        if (UpgradeUI.instance != null && UpgradeUI.instance.gameObject.activeSelf)
        {
            UpgradeUI.instance.Refresh(equippedItems);
        }

        Inventory.instance?.NotifyItemChanged();
    }

    /// <summary>
    /// 장착된 아이템들의 레벨별 스탯 보너스를 계산하고, 관련 UI를 업데이트합니다.
    /// </summary>
    public void ApplyEquipmentEffects()
    {
        // PlayerStat이 준비되지 않았다면 아무 것도 하지 않는다
        if (PlayerStat.instance == null)
            return;

        // 1) 이전에 더해 준 모든 장비 보너스를 초기화
        PlayerStat.instance.ResetEquipmentBonuses();

        // 2) equippedItems 한 벌씩 순회
        foreach (var kv in equippedItems)
        {
            InventoryItem invItem = kv.Value;
            ItemData data = invItem.itemData;  // ← 여기서 ItemData 참조를 가져옵니다

            // (A) “기본 스탯(Base Stats)” 보너스가 있다면 추가
            // 예: 기본 공격력(atk)이 baseATK 필드에 들어 있다면 아래처럼
            if (data.baseATK != 0)
            {
                PlayerStat.instance.AddEquipmentBonus(StatType.ATK, data.baseATK);
            }

            if (data.baseHP != 0)
            {
                PlayerStat.instance.AddEquipmentBonus(StatType.HP, data.baseHP);
            }
            if (data.baseMP != 0)
            {
                PlayerStat.instance.AddEquipmentBonus(StatType.MP, data.baseMP);
            }
            if (data.baseSTR != 0)
            {
                PlayerStat.instance.AddEquipmentBonus(StatType.STR, data.baseSTR);
            }
            if (data.baseDEX != 0)
            {
                PlayerStat.instance.AddEquipmentBonus(StatType.DEX, data.baseDEX);
            }
            if (data.baseCRIT != 0)
            {
                PlayerStat.instance.AddEquipmentBonus(StatType.CRIT, data.baseCRIT);
            }

            // (B) “강화 보너스(Per-Level × level)” 계산
            // invItem.GetEnhancedStats()가 내부적으로 (perLevelXXX × invItem.level)을 계산해서 돌려줍니다.
            Dictionary<StatType, int> enhanced = invItem.GetEnhancedStats();
            foreach (var pair in enhanced)
            {
                // 예: pair.Key = StatType.HP, pair.Value = (perLevelHP × level)
                PlayerStat.instance.AddEquipmentBonus(pair.Key, pair.Value);
            }
        }

        // 3) 변경된 스탯을 PlayerStat에게 알림
        PlayerStat.instance.NotifyStatChanged();

        // 4) 화면(UI) 갱신
        if (StatUI.instance != null)
            StatUI.instance.UpdateUI();

        if (EquipmentUI.instance != null)
            EquipmentUI.instance.RefreshUI();

        if (InventoryUI.instance != null)
            InventoryUI.instance.UpdateUI();
    }
}
