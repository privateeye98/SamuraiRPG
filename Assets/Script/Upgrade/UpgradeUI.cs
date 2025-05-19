using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class UpgradeUI : MonoBehaviour
{
    [Header("UI Components")]
    public TMP_Dropdown partDropdown;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI resultText;
    public Button upgradeButton;

    private Dictionary<ItemPartType, ItemData> upgradeItems;
    private ItemData currentItem;

    void Start()
    {
        upgradeButton.onClick.AddListener(() => TryUpgrade());
        partDropdown.onValueChanged.AddListener(OnDropdownChanged);
    }

    public void Open(Dictionary<ItemPartType, ItemData> data)
    {
        upgradeItems = data;
        gameObject.SetActive(true);
        partDropdown.value = 0;
        OnDropdownChanged(0);
    }

    void OnDropdownChanged(int index)
    {
        if (upgradeItems == null)
        {
            Debug.LogWarning("upgradeItems�� �ʱ�ȭ���� �ʾҽ��ϴ�.");
            return;
        }

        ItemPartType selectedPart = (ItemPartType)index;

        if (upgradeItems.TryGetValue(selectedPart, out var item) && item != null)
        {
            currentItem = item;
            itemNameText.text = item.itemName;
            levelText.text = $"Lv.{item.level}/{item.maxLevel}";
            costText.text = $"{item.upgradeCost * item.level} Gold";
        }
        else
        {
            currentItem = null;
            itemNameText.text = "�ش� ������ ����";
            levelText.text = "-";
            costText.text = "-";
            resultText.text = "";
        }
    }

    void TryUpgrade()
    {
        if (currentItem == null)
        {
            resultText.text = "��ȭ�� �������� �����ϴ�.";
            return;
        }

        if (currentItem.level >= currentItem.maxLevel)
        {
            resultText.text = "�̹� �ִ� �����Դϴ�.";
            return;
        }

        int cost = currentItem.upgradeCost * currentItem.level;

        if (!GoldManager.instance.SpendGold(cost))
        {
            resultText.text = "��尡 �����մϴ�.";
            return;
        }

        float successRate = currentItem.GetSuccessRate();
        float roll = Random.value;

        if (roll < successRate)
        {
            currentItem.level++;
            resultText.text = $"��ȭ ����! Lv.{currentItem.level}";

            PlayerStat.instance?.ApplyEquipmentStats(upgradeItems);
            FindObjectOfType<StatUI>()?.UpdateUI();
        }
        else
        {
            resultText.text = "��ȭ ����...";
        }

        OnDropdownChanged(partDropdown.value);
    }
}
