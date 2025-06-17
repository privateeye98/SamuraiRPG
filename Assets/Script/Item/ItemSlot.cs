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
        // ����������������������������������������������������������������������������������������������������������������������
        // 1) ��Ŭ��: �Ǹ� ó��
        // ����������������������������������������������������������������������������������������������������������������������
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // 1-1) ���Կ� �������� �ִ��� Ȯ��
            if (currentItem == null || currentItem.itemData == null)
                return;

            // 1-2) �� �� ���� ������(Quest ��)���� Ȯ��
            if (currentItem.itemData.type == ItemType.Quest)
            {
                Debug.LogWarning("[ItemSlot] ����Ʈ �������� �Ǹ��� �� �����ϴ�.");
                return;
            }

            // (�ɼ�) ���� ���� ���� �Ǹ� �� �ϵ��� ���� ������ ���⼭ üũ
            // if (currentItem.isEquipped) { Debug.LogWarning("���� ���� �� �Ǹ��ϼ���."); return; }

            // 1-3) ���� �Ǹ� ����
            //    SellItem(itemData, quantity) : Inventory.cs�� �����Ǿ� �־�� ��
            Inventory.instance.SellItem(currentItem.itemData, 1);

            // 1-4) �κ��丮 UI ��� ����
            //    SellItem ���ο��� NotifyItemChanged() �� InventoryUI.UpdateUI()�� �ɷ� �ִٸ�, 
            //    �Ʒ� ���� ��� ������, ���� �ڵ� ������ �� �Ǹ� ���� ȣ���ϼ���.
            if (InventoryUI.instance != null)
                InventoryUI.instance.UpdateUI();

            // 1-5) ���� ���ο��� ����(x)�� �ٲ� �� �ְ�, 
            //    quantity�� 0�� �Ǹ� �ش� ������ ���ֵ��� InventoryUI.UpdateUI()���� ó��
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
