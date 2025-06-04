using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SellItemSlot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Button sellButton;

    private InventoryItem invItem; 


    public void Setup(InventoryItem item)
    {
        invItem = item;
        icon.sprite = item.itemData.icon;
        nameText.text = item.itemData.itemName;

        int sellPrice = Mathf.FloorToInt(item.itemData.price * item.itemData.sellRatio);
        priceText.text = $"�ǸŰ�: {sellPrice}G";

        sellButton.onClick.RemoveAllListeners();
        sellButton.onClick.AddListener(OnClickSell);
    }

    private void OnClickSell()
    {
        if (invItem == null) return;

        if (invItem.itemData.type == ItemType.Quest)
        {
            Debug.LogWarning("[SellItemSlot] ����Ʈ �������� �Ǹ��� �� �����ϴ�.");
            return;
        }


        Inventory.instance.SellItem(invItem.itemData, 1);


        InventoryUI.instance.UpdateUI();
        Destroy(gameObject);
    }
}
