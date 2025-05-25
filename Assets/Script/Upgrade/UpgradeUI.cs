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

    public static UpgradeUI instance;

    // �ܺ� ���� ����� ���� public���� ����
    public Dictionary<ItemPartType, ItemData> upgradeItems;
    private ItemData currentItem;

    void Awake()
    {
        instance = this;
    }

    void OnEnable()
    {
        upgradeButton.onClick.AddListener(() => TryUpgrade());
        partDropdown.onValueChanged.AddListener(OnDropdownChanged);
    }

    void OnDisable()
    {
        upgradeButton.onClick.RemoveAllListeners();
        partDropdown.onValueChanged.RemoveAllListeners();
    }

    public void Open(Dictionary<ItemPartType, ItemData> data,Dictionary<ItemPartType, int> levels)
    {
        upgradeItems = data;
        foreach(var kv in levels)
        {
            if (upgradeItems.TryGetValue(kv.Key, out var itemData))
                itemData.level = kv.Value;
        }
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

        if (!System.Enum.IsDefined(typeof(ItemPartType), index))
        {
            Debug.LogWarning("�߸��� ��Ʈ �ε����Դϴ�.");
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


            foreach(var kv in EquipmentManager.instance.equippedItems)
            {
                if (kv.Value.itemData == currentItem)
                    kv.Value.level = currentItem.level;
            }
        }
        else
        {
            resultText.text = "��ȭ ����...";
        }

        var levels = GetLevels(upgradeItems);
        PlayerStat.instance?.ApplyEquipmentStats(upgradeItems, levels);
        FindObjectOfType<StatUI>()?.UpdateUI();

        OnDropdownChanged(partDropdown.value);
    }

    Dictionary<ItemPartType, int> GetLevels(Dictionary<ItemPartType, ItemData> items)
    {
        var levels = new Dictionary<ItemPartType, int>();
        foreach (var pair in items)
            levels[pair.Key] = pair.Value.level;
        return levels;
    }


    public void RefreshStatPreview()
    {
        OnDropdownChanged(partDropdown.value);
    }

    public void LoadUpgrades(List<SavedItem> savedItems)
    {
        upgradeItems = new Dictionary<ItemPartType, ItemData>();

        foreach (var saved in savedItems)
        {
            if (!System.Enum.TryParse(saved.part, out ItemPartType part)) continue;

            ItemData item = ItemDatabase.instance.GetItemById(saved.itemId);
            if (item != null)
            {
                item.level = saved.level;
             //   item.statType = saved.statType;
                upgradeItems[part] = item;
            }
        }

        RefreshStatPreview();
    }


}
