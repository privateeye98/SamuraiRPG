using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI quantityText;
    ItemData item;
    InventoryItem currentItem;

    private float lastClickTime = 0f;
    private const float doubleClickThreshold = 0.3f;

    public void SetItem(InventoryItem invItem)
    {
        currentItem = invItem;
        item = invItem.itemData;

        icon.sprite = invItem.itemData.icon;
        icon.enabled = true;
        quantityText.text = $"x{invItem.quantity}";
        quantityText.color = Color.white;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"[ItemSlot] PointerEnter on {currentItem?.itemData.itemName}");


        if (currentItem == null || currentItem.itemData == null)
            return;
        TooltipUI.instance.Show(currentItem, Input.mousePosition);
    }

public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUI.instance.Hide();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (item == null) return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            float time = Time.unscaledTime;
            if (time - lastClickTime < doubleClickThreshold)
            {
                OnClickUse();
                lastClickTime = 0f;
            }
            else
            {
                lastClickTime = time;
            }
        }
    }

    public void OnClickUse()
    {
        if (currentItem == null)
            return;

        if (currentItem.itemData.type == ItemType.Equipment)
        {
            EquipmentManager.instance.EquipItem(currentItem);
        }
        else
        {
            Inventory.instance.UseItem(item);
        }
    }

    public void Clear()
    {
        if (icon == null) return;
        currentItem = null;
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        quantityText.text = "";
    }
}
