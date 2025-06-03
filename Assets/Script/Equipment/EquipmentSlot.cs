using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Linq;

/// <summary>
/// 개별 장비 슬롯. 
/// - InventoryItem이 들어오면 icon(enabled=true, sprite 갱신, levelText 갱신)
/// - null이 들어오면 icon.enabled=false, levelText.Clear
/// - 클릭 시 Equip/Unequip을 호출
/// - 마우스 오버 시 TooltipUI에 장착 아이템 정보 표시
/// </summary>
public class EquipmentSlot : MonoBehaviour,
    IPointerClickHandler,
    IPointerEnterHandler,
    IPointerExitHandler
{
    public ItemPartType partType;             // 이 슬롯이 담당하는 부위(Head, Body, Leg 등)
    [SerializeField] public Image icon;       // Inspector에서 Image 컴포넌트 연결 필수
    [SerializeField] private TextMeshProUGUI levelText;  // Inspector에서 TextMeshProUGUI 연결

    [HideInInspector] public InventoryItem currentItem;

    /// <summary>
    /// 장착된 아이템 정보를 받아서 슬롯 아이콘과 레벨 텍스트를 설정하거나 초기화
    /// </summary>
    public void SetItem(InventoryItem item)
    {
        currentItem = item;

        // 1) icon이나 levelText가 할당되지 않았거나, 파괴되었으면 그냥 리턴(방어 처리)
        if (icon == null)
        {
            if (levelText != null)
                levelText.text = string.Empty;
            return;
        }

        // 2) item이 null이면 빈 슬롯으로 처리
        if (item == null)
        {
            icon.enabled = false;
            if (levelText != null) levelText.text = string.Empty;
        }
        else
        {
            // 3) item이 있으면 icon 활성화 + sprite 갱신 + 레벨 표시
            icon.sprite = item.itemData.icon;
            icon.enabled = true;
            if (levelText != null)
                levelText.text = $"Lv {item.level}";
        }
    }

    /// <summary>
    /// 클릭 시 수행: 
    /// - 현재 장착된 아이템이 있으면 Unequip, 
    /// - 없으면 인벤토리에서 같은 파트의 아이템을 찾아 Equip 시도
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        if (currentItem != null)
        {
            EquipmentManager.instance.UnequipItem(partType);
        }
        else
        {
            // 인벤토리에서 같은 partType 아이템을 찾아서 Equip
            var invItem = Inventory.instance.items
                .FirstOrDefault(i => i.itemData.part == partType);

            if (invItem != null)
                EquipmentManager.instance.EquipItem(invItem);
            else
                Debug.Log($"인벤토리에 {partType} 장비가 없습니다.");
        }
    }

    /// <summary>
    /// 마우스 오버 시 현재 장착된 아이템이 있으면 Tooltip 띄우기
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentItem != null)
            TooltipUI.instance.Show(currentItem, Input.mousePosition);
    }

    /// <summary>
    /// 마우스 포인터가 슬롯을 벗어날 때 Tooltip 숨기기
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUI.instance.Hide();
    }
}
