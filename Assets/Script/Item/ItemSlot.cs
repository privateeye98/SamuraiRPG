using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ItemSlot : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI quantityText;
    ItemData item;

    InventoryItem currentItem;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClickUse);
    }

    public void SetItem(InventoryItem invItem)
    {
        currentItem = invItem;
        item = invItem.itemData;

        icon.sprite = invItem.itemData.icon;
        icon.enabled = true;

        Debug.Log($"[SetItem] {invItem.itemData.itemName} ¼ö·®: {invItem.quantity}");
     
        quantityText.text = $"x{invItem.quantity}";
        quantityText.color = Color.white; 
    }

    public void OnClickUse()
    {
        if(item != null)
      Inventory.instance.UseItem(item);

    }

    public void Clear()
    {
        currentItem = null;
        icon.sprite = null;
        icon.enabled = false;
        quantityText.text = "";
    }

}
