using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Linq;

/// <summary>
/// ���� ��� ����. 
/// - InventoryItem�� ������ icon(enabled=true, sprite ����, levelText ����)
/// - null�� ������ icon.enabled=false, levelText.Clear
/// - Ŭ�� �� Equip/Unequip�� ȣ��
/// - ���콺 ���� �� TooltipUI�� ���� ������ ���� ǥ��
/// </summary>
public class EquipmentSlot : MonoBehaviour,
    IPointerClickHandler,
    IPointerEnterHandler,
    IPointerExitHandler
{
    public ItemPartType partType;             // �� ������ ����ϴ� ����(Head, Body, Leg ��)
    [SerializeField] public Image icon;       // Inspector���� Image ������Ʈ ���� �ʼ�
    [SerializeField] private TextMeshProUGUI levelText;  // Inspector���� TextMeshProUGUI ����

    [HideInInspector] public InventoryItem currentItem;

    /// <summary>
    /// ������ ������ ������ �޾Ƽ� ���� �����ܰ� ���� �ؽ�Ʈ�� �����ϰų� �ʱ�ȭ
    /// </summary>
    public void SetItem(InventoryItem item)
    {
        currentItem = item;

        // 1) icon�̳� levelText�� �Ҵ���� �ʾҰų�, �ı��Ǿ����� �׳� ����(��� ó��)
        if (icon == null)
        {
            if (levelText != null)
                levelText.text = string.Empty;
            return;
        }

        // 2) item�� null�̸� �� �������� ó��
        if (item == null)
        {
            icon.enabled = false;
            if (levelText != null) levelText.text = string.Empty;
        }
        else
        {
            // 3) item�� ������ icon Ȱ��ȭ + sprite ���� + ���� ǥ��
            icon.sprite = item.itemData.icon;
            icon.enabled = true;
            if (levelText != null)
                levelText.text = $"Lv {item.level}";
        }
    }

    /// <summary>
    /// Ŭ�� �� ����: 
    /// - ���� ������ �������� ������ Unequip, 
    /// - ������ �κ��丮���� ���� ��Ʈ�� �������� ã�� Equip �õ�
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        if (currentItem != null)
        {
            EquipmentManager.instance.UnequipItem(partType);
        }
        else
        {
            // �κ��丮���� ���� partType �������� ã�Ƽ� Equip
            var invItem = Inventory.instance.items
                .FirstOrDefault(i => i.itemData.part == partType);

            if (invItem != null)
                EquipmentManager.instance.EquipItem(invItem);
            else
                Debug.Log($"�κ��丮�� {partType} ��� �����ϴ�.");
        }
    }

    /// <summary>
    /// ���콺 ���� �� ���� ������ �������� ������ Tooltip ����
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentItem != null)
            TooltipUI.instance.Show(currentItem, Input.mousePosition);
    }

    /// <summary>
    /// ���콺 �����Ͱ� ������ ��� �� Tooltip �����
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUI.instance.Hide();
    }
}
