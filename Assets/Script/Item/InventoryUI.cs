using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform slotParent;
    ItemSlot[] slots;
    public static InventoryUI instance;
    void Start()
    {
        if (Inventory.instance == null)
        {
            Debug.LogWarning("인스턴스가 없습니다.");
            return;
        }

        Inventory.instance.OnItemChangedCallback += UpdateUI;

        slots = new ItemSlot[Inventory.instance.capacity];
        for (int i = 0; i < Inventory.instance.capacity; i++)
        {
            GameObject go = Instantiate(slotPrefab, slotParent);
            slots[i] = go.GetComponent<ItemSlot>();
        }
        UpdateUI();
    }

    public void UpdateUI()
    {
        Debug.Log($"UpdateUI CALLED? items.Count={Inventory.instance.items.Count}");

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < Inventory.instance.items.Count)
                slots[i].SetItem(Inventory.instance.items[i]);
            else
                slots[i].Clear();
        }
    }
}
