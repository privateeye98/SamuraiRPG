using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ȭ�鿡 ������ �������� ǥ���ϴ� UI. 
/// - �� A��B��A �Ŀ��� �����ϵ���, �Ź� OnEnable���� ���Ե��� �����(Rebuild)�ϰ� RefreshUI.
/// - InventoryManager�� EquipmentManager�� �����Ͱ� �ٲ� ������ RefreshUI�� ȣ��.
/// </summary>
public class EquipmentUI : MonoBehaviour
{
    public static EquipmentUI instance;

    [Header("Slot Parent (�� Inspector���� ����)")]
    [SerializeField] private Transform slotParent;

    // Runtime�� �籸���Ǵ� ���� ����Ʈ
    private List<EquipmentSlot> slots = new List<EquipmentSlot>();

    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    void OnEnable()
    {
        // ���� Ȱ��ȭ�� ������ ������ �ٽ� ���� �� �ı��� ������ ���� ���� ����
        RebuildSlots();
        RefreshUI();
    }

    /// <summary>
    /// slotParent �Ʒ��� �پ� �ִ� ��� EquipmentSlot ������Ʈ�� �����ؼ� ����Ʈ�� ����
    /// </summary>
    private void RebuildSlots()
    {
        slots.Clear();

        if (slotParent == null)
        {
            Debug.LogWarning("[EquipmentUI] slotParent�� �Ҵ���� �ʾҽ��ϴ�!");
            return;
        }

        foreach (Transform child in slotParent)
        {
            var slotComp = child.GetComponent<EquipmentSlot>();
            if (slotComp != null)
                slots.Add(slotComp);
        }
    }

    /// <summary>
    /// EquipmentManager.instance.equippedItems�� ��������,
    /// ��� ���Ը��� SetItem(������ ���) �Ǵ� ClearSlot(�� ���)�� ȣ��
    /// </summary>
    public void RefreshUI()
    {
        if (EquipmentManager.instance == null) return;
        var equippedDict = EquipmentManager.instance.equippedItems;

        foreach (var slot in slots)
        {
            // 1) icon(Image) ���� UI ������Ʈ�� �ı��Ǿ��ų� �Ҵ� �� �� ���, �� ����ó�� ó��
            if (slot.icon == null)
            {
                slot.SetItem(null);
                continue;
            }

            // 2) ���� ���� ������ ������ SetItem, �ƴϸ� �� �������� Clear
            if (equippedDict.TryGetValue(slot.partType, out var invItem))
            {
                slot.SetItem(invItem);
            }
            else
            {
                slot.SetItem(null);
            }
        }
    }
}
