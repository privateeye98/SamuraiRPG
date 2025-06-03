using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 화면에 장착된 아이템을 표시하는 UI. 
/// - 씬 A→B→A 후에도 동작하도록, 매번 OnEnable에서 슬롯들을 재수집(Rebuild)하고 RefreshUI.
/// - InventoryManager나 EquipmentManager에 데이터가 바뀔 때마다 RefreshUI를 호출.
/// </summary>
public class EquipmentUI : MonoBehaviour
{
    public static EquipmentUI instance;

    [Header("Slot Parent (씬 Inspector에서 지정)")]
    [SerializeField] private Transform slotParent;

    // Runtime에 재구성되는 슬롯 리스트
    private List<EquipmentSlot> slots = new List<EquipmentSlot>();

    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    void OnEnable()
    {
        // 씬이 활성화될 때마다 슬롯을 다시 수집 → 파괴된 참조를 절대 쓰지 않음
        RebuildSlots();
        RefreshUI();
    }

    /// <summary>
    /// slotParent 아래에 붙어 있는 모든 EquipmentSlot 컴포넌트를 수집해서 리스트를 갱신
    /// </summary>
    private void RebuildSlots()
    {
        slots.Clear();

        if (slotParent == null)
        {
            Debug.LogWarning("[EquipmentUI] slotParent가 할당되지 않았습니다!");
            return;
        }

        foreach (Transform child in slotParent)
        {
            var slotComp = child.GetComponent<EquipmentSlot>();
            if (slotComp != null)
                slots.Add(slotComp);
        }
    }

    /// <summary>
    /// EquipmentManager.instance.equippedItems를 기준으로,
    /// 모든 슬롯마다 SetItem(장착된 경우) 또는 ClearSlot(빈 경우)를 호출
    /// </summary>
    public void RefreshUI()
    {
        if (EquipmentManager.instance == null) return;
        var equippedDict = EquipmentManager.instance.equippedItems;

        foreach (var slot in slots)
        {
            // 1) icon(Image) 같은 UI 컴포넌트가 파괴되었거나 할당 안 된 경우, 빈 슬롯처럼 처리
            if (slot.icon == null)
            {
                slot.SetItem(null);
                continue;
            }

            // 2) 실제 장착 정보가 있으면 SetItem, 아니면 빈 슬롯으로 Clear
            if (equippedDict.TryGetValue(slot.partType, out var invItem))
            {
                slot.SetItem(invItem);
            }
            else
            {
                slot.SetItem(null);
            }
        }
    }
}
