using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class HotbarSlotUI : MonoBehaviour, IPointerClickHandler, IDropHandler
{
    [SerializeField] private Image iconImage;            // 아이템 아이콘 (이미지 컴포넌트)
    [SerializeField] private TextMeshProUGUI keyText;    // 할당된 키 표시
    [SerializeField] private TextMeshProUGUI qtyText;    // 아이템 수량 표시

    private int slotIndex;

    /// <summary>
    /// 슬롯을 초기화할 때, HotbarManager 내부 slots 배열의 인덱스를 넘겨 줍니다.
    /// </summary>
    public void Initialize(int index)
    {
        slotIndex = index;
        RefreshUI();
    }

    /// <summary>
    /// HotbarManager.slots[slotIndex] 데이터를 기반으로
    /// 아이콘/수량/키 텍스트를 업데이트합니다.
    /// </summary>
    public void RefreshUI()
    {
        var slot = HotbarManager.instance.slots[slotIndex];

        // 1) 키 텍스트 표시 (KeyCode.Alpha1 → "1", KeyCode.Insert → "Insert" 등 그대로 표시)
        string codeName = slot.assignedKey.ToString();
        if (codeName.StartsWith("Alpha"))
            codeName = codeName.Replace("Alpha", "");
        keyText.text = codeName;

        // 2) 아이템 아이콘 & 수량 표시
        if (slot.assignedItemData != null)
        {
            iconImage.sprite = slot.assignedItemData.icon;
            iconImage.enabled = true;

            // Inventory에서 실제 수량 찾아서 표시 (예: "x3")
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
        // (여기에는 키 재할당 기능을 넣어도 되고, 비워 두어도 됩니다.)
    }

    /// <summary>
    /// IDropHandler 인터페이스: 인벤토리 슬롯을 이 핫바 슬롯 위에 드롭했을 때 호출됩니다.
    /// </summary>
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log($"[HotbarSlotUI] OnDrop 호출! slotIndex={slotIndex}, pointerDrag={eventData.pointerDrag?.name}");

        // 1) 드롭된 오브젝트가 ItemSlot(=인벤토리 슬롯)인지 확인
        ItemSlot invSlot = eventData.pointerDrag?.GetComponent<ItemSlot>();
        if (invSlot == null)
        {
            Debug.Log("[HotbarSlotUI] OnDrop: pointerDrag에 ItemSlot 컴포넌트가 없습니다.");
            return;
        }

        InventoryItem invItem = invSlot.GetCurrentItem();
        if (invItem == null)
        {
            Debug.Log("[HotbarSlotUI] OnDrop: 해당 InventorySlot에 아이템이 없습니다.");
            return;
        }
        if (invItem.itemData.type == ItemType.Quest)
        {
            Debug.LogWarning("[Hotbar] 퀘스트 아이템은 핫바에 할당할 수 없습니다.");
            return;
        }
        Debug.Log($"[HotbarSlotUI] OnDrop 성공! Assigning item '{invItem.itemData.itemName}' to slot {slotIndex}.");

        HotbarManager.instance.slots[slotIndex].assignedItemData = invItem.itemData;
        HotbarManager.instance.RefreshAllSlots();
    }
}
