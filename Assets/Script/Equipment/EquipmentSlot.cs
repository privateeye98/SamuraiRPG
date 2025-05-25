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
        if (item != null)
        {
            icon.sprite = item.itemData.icon;
            icon.enabled = true;
            levelText.text = $"Lv {item.level}";
        }
        else
        {
            icon.enabled = false;
            levelText.text = string.Empty;
        }
    }


    public void SetLevelText(string text)
    {
        icon.enabled = false;
        levelText.text = text;
    }


    // ��� ����: Ŭ�� �Ǵ� ����Ŭ��
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        if(currentItem != null)
        {
            EquipmentManager.instance.UnequipItem(partType);
        }
        else
        {
            var invItem = Inventory.instance.items.FirstOrDefault(i => i.itemData.part == partType);
            if (invItem != null)
            {
                EquipmentManager.instance.EquipItem(invItem);
            }
            else
            {
                Debug.Log($"�κ��丮�� {partType} ��� �����ϴ�.");
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
