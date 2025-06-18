using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ShopItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI priceText;
    ItemData item;

    public void Setup(ItemData newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        nameText.text = item.itemName;
        priceText.text = $"{item.price} 골드";
    }

    public void OnClickBuy()
    {
        Debug.Log($"[클릭] {item.itemName} 구매 버튼 클릭됨");
        ShopManager.instance.OpenBuyAmountPopup(item);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null) return;

        InventoryItem tempItem = new InventoryItem(item, 1, 0);
        TooltipUI.instance.Show(tempItem, Input.mousePosition);
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUI.instance.Hide();
    }
}