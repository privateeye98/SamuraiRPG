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
            Debug.LogWarning("InventoryUI.Start: Inventory.instance�� null�Դϴ�.");
            return;
        }

        if (slotPrefab == null)
        {
            Debug.LogError("InventoryUI.Start: slotPrefab�� �Ҵ���� �ʾҽ��ϴ�!");
            return;
        }
        if (slotParent == null)
        {
            Debug.LogError("InventoryUI.Start: slotParent�� �Ҵ���� �ʾҽ��ϴ�!");
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
            Debug.LogWarning("InventoryUI.UpdateUI: slots�� ���� �ʱ�ȭ���� �ʾҽ��ϴ�.");
            return;
        }
        if (Inventory.instance == null)
        {
            Debug.LogWarning("InventoryUI.UpdateUI: Inventory.instance�� null�Դϴ�.");
            return;
        }
        if (Inventory.instance.items == null)
        {
            Debug.LogWarning("InventoryUI.UpdateUI: Inventory.instance.items ����Ʈ�� null�Դϴ�.");
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
