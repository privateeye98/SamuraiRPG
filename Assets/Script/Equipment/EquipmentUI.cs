using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    public static EquipmentUI instance;
    public EquipmentSlot[] slots;

    void Awake() => instance = this;
     void Start()
    {
        RefreshUI();
    }
    public void RefreshUI()
    {
        foreach (var slot in slots)
        {
            InventoryItem item = null;
            EquipmentManager.instance.equippedItems.TryGetValue(slot.partType, out item);
            slot.SetItem(item);
        }
    }
}
