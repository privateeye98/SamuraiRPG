using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Linq;

public class EquipmentSlot : MonoBehaviour, IPointerClickHandler,IPointerExitHandler,IPointerEnterHandler
{
    public ItemPartType partType;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI levelText;
    public InventoryItem currentItem;


    public void SetItem(InventoryItem item)
    {
        currentItem = item;

        if (icon == null)
        {
            // icon이 파괴되었거나 할당 누락된 경우, 레벨 텍스트만 지우고 리턴
            if (levelText != null) levelText.text = string.Empty;
            return;
        }

        if (item != null)
        {
            icon.sprite = item.itemData.icon;
            icon.enabled = true;
            if (levelText != null) levelText.text = $"Lv {item.level}";
        }
        else
        {
            icon.enabled = false;
            if (levelText != null) levelText.text = string.Empty;
        }
    }


    public void SetLevelText(string text)
    {
        icon.enabled = false;
        levelText.text = text;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"EquipmentSlot.OnPointerClick: partType={partType}, currentItem={(currentItem == null ? "null" : currentItem.itemData.itemName)}");

        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        if(currentItem != null)
        {
            Debug.Log($"  → UnequipItem 호출: {partType}");

            EquipmentManager.instance.UnequipItem(partType);
        }
        else
        {
            var invItem = Inventory.instance.items.FirstOrDefault(i => i.itemData.part == partType);
            if (invItem != null)
            {
                Debug.Log($"  → EquipItem 호출: {invItem.itemData.itemName}");
                EquipmentManager.instance.EquipItem(invItem);
            }
            else
            {
                Debug.Log($"인벤토리에 {partType} 장비가 없습니다.");
            }
        }
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentItem != null)
            TooltipUI.instance.Show(currentItem, Input.mousePosition);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUI.instance.Hide();
    }

}
