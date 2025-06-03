using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform slotParent;
    ItemSlot[] slots;
    public static InventoryUI instance;

    void Awake()
    {
       if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (Inventory.instance == null)
        {
            Debug.LogWarning("InventoryUI.Start: Inventory.instance가 null입니다.");
            return;
        }

        if (slotPrefab == null)
        {
            Debug.LogError("InventoryUI.Start: slotPrefab이 할당되지 않았습니다!");
            return;
        }
        if (slotParent == null)
        {
            Debug.LogError("InventoryUI.Start: slotParent가 할당되지 않았습니다!");
            return;
        }

        Inventory.instance.OnItemChangedCallback += UpdateUI;

        slots = new ItemSlot[Inventory.instance.capacity];
        for (int i = 0; i < Inventory.instance.capacity; i++)
        {
            GameObject go = Instantiate(slotPrefab, slotParent,false);
            slots[i] = go.GetComponent<ItemSlot>();
        }
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (slots == null)
        {
            Debug.LogWarning("InventoryUI.UpdateUI: slots가 아직 초기화되지 않았습니다.");
            return;
        }
        if (Inventory.instance == null)
        {
            Debug.LogWarning("InventoryUI.UpdateUI: Inventory.instance가 null입니다.");
            return;
        }
        if (Inventory.instance.items == null)
        {
            Debug.LogWarning("InventoryUI.UpdateUI: Inventory.instance.items 리스트가 null입니다.");
            return;
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < Inventory.instance.items.Count)
                slots[i].SetItem(Inventory.instance.items[i]);
            else
                slots[i].Clear();
        }
    }
}
