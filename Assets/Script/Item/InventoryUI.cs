using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform slotParent;
    ItemSlot[] slots;

    void Start()
    {
        
            Debug.Log("[InventoryUI] Ω√¿€µ ");
        
        if (Inventory.instance == null)
        {
            Debug.LogWarning("Inventory.instance is null in InventoryUI.cs");
            return;
        }

        Inventory.instance.onItemChangedCallback += UpdateUI;

        slots = new ItemSlot[Inventory.instance.capacity];

        for (int i = 0; i < Inventory.instance.capacity; i++)
        {
            GameObject go = Instantiate(slotPrefab, slotParent);
            slots[i] = go.GetComponent<ItemSlot>();
        }

        UpdateUI();

    }


    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < Inventory.instance.items.Count)
                slots[i].SetItem(Inventory.instance.items[i]);
            else
                slots[i].Clear();
        }

    }
}