using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI quantityText;

    ItemData item;
    InventoryItem currentItem;

    private float lastClickTime = 0f;
    private const float doubleClickThreshold = 0.3f;


    private CanvasGroup canvasGroup;
    private Transform originalParent;


    public void SetItem(InventoryItem invItem)
    {
        currentItem = invItem;
        item = invItem.itemData;

        icon.sprite = invItem.itemData.icon;
        icon.enabled = true;
        quantityText.text = $"x{invItem.quantity}";
        quantityText.color = Color.white;
    }
    private void Awake()
    {
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"[ItemSlot] PointerEnter on {currentItem?.itemData.itemName}");


        if (currentItem == null || currentItem.itemData == null)
            return;
        TooltipUI.instance.Show(currentItem, Input.mousePosition);
    }

public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUI.instance.Hide();
    }

    public void AssignToHotbar(int hotbarIndex)
    {
        if (currentItem == null || currentItem.itemData == null) return;
        HotbarManager.instance.slots[hotbarIndex].assignedItemData = currentItem.itemData;
        HotbarManager.instance.RefreshAllSlots();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item == null) return;
        // ───────────────────────────────────────────────────────────
        // 1) 우클릭: 판매 처리
        // ───────────────────────────────────────────────────────────
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // 1-1) 슬롯에 아이템이 있는지 확인
            if (currentItem == null || currentItem.itemData == null)
                return;

            // 1-2) 팔 수 없는 아이템(Quest 등)인지 확인
            if (currentItem.itemData.type == ItemType.Quest)
            {
                Debug.LogWarning("[ItemSlot] 퀘스트 아이템은 판매할 수 없습니다.");
                return;
            }

            // (옵션) 장착 중인 장비는 판매 못 하도록 막고 싶으면 여기서 체크
            // if (currentItem.isEquipped) { Debug.LogWarning("장착 해제 후 판매하세요."); return; }

            // 1-3) 실제 판매 수행
            //    SellItem(itemData, quantity) : Inventory.cs에 구현되어 있어야 함
            Inventory.instance.SellItem(currentItem.itemData, 1);

            // 1-4) 인벤토리 UI 즉시 갱신
            //    SellItem 내부에서 NotifyItemChanged() → InventoryUI.UpdateUI()가 걸려 있다면, 
            //    아래 줄은 없어도 되지만, 만약 자동 갱신이 안 되면 직접 호출하세요.
            if (InventoryUI.instance != null)
                InventoryUI.instance.UpdateUI();

            // 1-5) 슬롯 내부에서 수량(x)도 바뀔 수 있고, 
            //    quantity가 0이 되면 해당 슬롯을 없애도록 InventoryUI.UpdateUI()에서 처리
            return;
        }
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            float time = Time.unscaledTime;
            if (time - lastClickTime < doubleClickThreshold)
            {
                OnClickUse();
                lastClickTime = 0f;
            }
            else
            {
                lastClickTime = time;
            }
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentItem == null ) return;

        DragItemUI.instance.Show(currentItem.itemData.icon);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentItem == null) return;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragItemUI.instance.Hide();
    }
    public void OnClickUse()
    {
        if (currentItem == null)
            return;

        if (currentItem.itemData.type == ItemType.Equipment)
        {
            EquipmentManager.instance.EquipItem(currentItem);
        }
        else
        {
            Inventory.instance.UseItem(item);
        }
    }
    public InventoryItem GetCurrentItem()
    {
        return currentItem;
    }
    public void Clear()
    {
        if (icon == null) return;
        currentItem = null;
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        quantityText.text = "";
    }
}
