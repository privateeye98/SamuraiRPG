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

    // 외부 접근 허용을 위해 public으로 변경
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
            Debug.LogWarning("upgradeItems가 초기화되지 않았습니다.");
            return;
        }

        if (!System.Enum.IsDefined(typeof(ItemPartType), index))
        {
            Debug.LogWarning("잘못된 파트 인덱스입니다.");
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
            itemNameText.text = "해당 아이템 없음";
            levelText.text = "-";
            costText.text = "-";
        }
    }

    void TryUpgrade()
    {
        if (currentItem == null)
        {
            resultText.text = "강화할 아이템이 없습니다.";
            return;
        }

        if (currentItem.level >= currentItem.maxLevel)
        {
            resultText.text = "이미 최대 레벨입니다.";
            return;
        }

        int cost = currentItem.upgradeCost * currentItem.level;

        if (!GoldManager.instance.SpendGold(cost))
        {
            resultText.text = "골드가 부족합니다.";
            return;
        }

        float successRate = currentItem.GetSuccessRate();
        float roll = Random.value;

        if (roll < successRate)
        {
            currentItem.level++;
            resultText.text = $"강화 성공! Lv.{currentItem.level}";

            PlayerStat.instance?.ApplyEquipmentStats(upgradeItems);
            FindObjectOfType<StatUI>()?.UpdateUI();
        }
        else
        {
            resultText.text = "강화 실패...";
        }

        OnDropdownChanged(partDropdown.value);
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
                item.statType = saved.statType;
                upgradeItems[part] = item;
            }
        }

        RefreshStatPreview();
    }


}
