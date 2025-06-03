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
            // icon�� �ı��Ǿ��ų� �Ҵ� ������ ���, ���� �ؽ�Ʈ�� ����� ����
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
            Debug.Log($"  �� UnequipItem ȣ��: {partType}");

            EquipmentManager.instance.UnequipItem(partType);
        }
        else
        {
            var invItem = Inventory.instance.items.FirstOrDefault(i => i.itemData.part == partType);
            if (invItem != null)
            {
                Debug.Log($"  �� EquipItem ȣ��: {invItem.itemData.itemName}");
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
