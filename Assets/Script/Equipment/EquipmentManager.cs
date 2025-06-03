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

    /// <summary>
    /// 아이템 장착
    /// </summary>
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
        if (PlayerStat.instance == null) return;

        // 1) 이전에 더해 준 모든 장비 보너스를 초기화
        PlayerStat.instance.ResetEquipmentBonuses();

        // 2) equippedItems 한 벌씩 순회하며, GetEnhancedStats() 결과를 꺼내서 더해 준다
        foreach (var kv in equippedItems)
        {
            InventoryItem invItem = kv.Value;

            // GetEnhancedStats()는 “현재 레벨(level) × Per‐Level 보너스”를 계산한 Dictionary
            Dictionary<StatType, int> enhanced = invItem.GetEnhancedStats();

            // 각 StatType별 보너스를 PlayerStat에 더해 준다
            foreach (var pair in enhanced)
            {
                // 예) pair.Key = StatType.HP, pair.Value = (hpBonusPerLevel × lev)
                PlayerStat.instance.AddEquipmentBonus(pair.Key, pair.Value);
            }
        }

        // 3) 스탯이 모두 더해졌으면, PlayerStat에게 “변경 알림”
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
