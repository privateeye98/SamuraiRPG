using UnityEngine;
using UnityEngine.UI;
public class ItemSlot : MonoBehaviour
{
    [SerializeField] Image icon;
    ItemData item;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClickUse);
    }

    public void SetItem(ItemData newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
    }

    public void OnClickUse()
    {
        if(item != null)
      Inventory.instance.UseItem(item);

    }

    public void Clear()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
    }

}
