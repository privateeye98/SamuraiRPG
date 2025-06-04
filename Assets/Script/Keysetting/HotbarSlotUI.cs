using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class HotbarSlotUI : MonoBehaviour, IPointerClickHandler, IDropHandler
{
    [SerializeField] private Image iconImage;            // ������ ������ (�̹��� ������Ʈ)
    [SerializeField] private TextMeshProUGUI keyText;    // �Ҵ�� Ű ǥ��
    [SerializeField] private TextMeshProUGUI qtyText;    // ������ ���� ǥ��

    private int slotIndex;

    /// <summary>
    /// ������ �ʱ�ȭ�� ��, HotbarManager ���� slots �迭�� �ε����� �Ѱ� �ݴϴ�.
    /// </summary>
    public void Initialize(int index)
    {
        slotIndex = index;
        RefreshUI();
    }

    /// <summary>
    /// HotbarManager.slots[slotIndex] �����͸� �������
    /// ������/����/Ű �ؽ�Ʈ�� ������Ʈ�մϴ�.
    /// </summary>
    public void RefreshUI()
    {
        var slot = HotbarManager.instance.slots[slotIndex];

        // 1) Ű �ؽ�Ʈ ǥ�� (KeyCode.Alpha1 �� "1", KeyCode.Insert �� "Insert" �� �״�� ǥ��)
        string codeName = slot.assignedKey.ToString();
        if (codeName.StartsWith("Alpha"))
            codeName = codeName.Replace("Alpha", "");
        keyText.text = codeName;

        // 2) ������ ������ & ���� ǥ��
        if (slot.assignedItemData != null)
        {
            iconImage.sprite = slot.assignedItemData.icon;
            iconImage.enabled = true;

            // Inventory���� ���� ���� ã�Ƽ� ǥ�� (��: "x3")
            var invItem = Inventory.instance.items.Find(i => i.itemData == slot.assignedItemData);
            qtyText.text = invItem != null ? $"x{invItem.quantity}" : "";
        }
        else
        {
            iconImage.sprite = null;
            iconImage.enabled = false;
            qtyText.text = "";
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // (���⿡�� Ű ���Ҵ� ����� �־ �ǰ�, ��� �ξ �˴ϴ�.)
    }

    /// <summary>
    /// IDropHandler �������̽�: �κ��丮 ������ �� �ֹ� ���� ���� ������� �� ȣ��˴ϴ�.
    /// </summary>
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log($"[HotbarSlotUI] OnDrop ȣ��! slotIndex={slotIndex}, pointerDrag={eventData.pointerDrag?.name}");

        // 1) ��ӵ� ������Ʈ�� ItemSlot(=�κ��丮 ����)���� Ȯ��
        ItemSlot invSlot = eventData.pointerDrag?.GetComponent<ItemSlot>();
        if (invSlot == null)
        {
            Debug.Log("[HotbarSlotUI] OnDrop: pointerDrag�� ItemSlot ������Ʈ�� �����ϴ�.");
            return;
        }

        InventoryItem invItem = invSlot.GetCurrentItem();
        if (invItem == null)
        {
            Debug.Log("[HotbarSlotUI] OnDrop: �ش� InventorySlot�� �������� �����ϴ�.");
            return;
        }
        if (invItem.itemData.type == ItemType.Quest)
        {
            Debug.LogWarning("[Hotbar] ����Ʈ �������� �ֹٿ� �Ҵ��� �� �����ϴ�.");
            return;
        }
        Debug.Log($"[HotbarSlotUI] OnDrop ����! Assigning item '{invItem.itemData.itemName}' to slot {slotIndex}.");

        HotbarManager.instance.slots[slotIndex].assignedItemData = invItem.itemData;
        HotbarManager.instance.RefreshAllSlots();
    }
}
