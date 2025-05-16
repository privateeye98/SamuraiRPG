using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemSlot : MonoBehaviour
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
        priceText.text = item.price.ToString();
    }

    public void OnClickBuy()
    {
        Debug.Log($"[클릭] {item.itemName} 구매 버튼 클릭됨");
        ShopManager.instance.BuyItem(item);
    }
}