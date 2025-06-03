using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Dropdown partDropdown;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private Button upgradeButton;

    private Dictionary<ItemPartType, InventoryItem> upgradeItems;
    private int currentDropdownIndex = 0;

    public static UpgradeUI instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);


    }

    void OnEnable()
    {
        // 드롭다운 값이 바뀔 때 콜백 연결
        partDropdown.onValueChanged.RemoveAllListeners();
        partDropdown.onValueChanged.AddListener(OnDropdownChanged);

        // 강화 버튼 클릭 시 콜백 연결
        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
    }

    void OnDisable()
    {
        partDropdown.onValueChanged.RemoveAllListeners();
        upgradeButton.onClick.RemoveAllListeners();
    }

    /// <summary>
    /// 강화창을 처음 열 때 호출합니다.
    /// </summary>
    public void Open(Dictionary<ItemPartType, InventoryItem> items)
    {
        upgradeItems = items;
        gameObject.SetActive(true);

        // 드롭다운 초기화
        partDropdown.ClearOptions();
        var options = System.Enum.GetNames(typeof(ItemPartType)).ToList();
        partDropdown.AddOptions(options);

        // 드롭다운 첫 항목 선택(인덱스 0)
        partDropdown.value = 0;
        partDropdown.RefreshShownValue();

        OnDropdownChanged(0);
    }

    /// <summary>
    /// 장비가 바뀐 경우, 강화창이 열려 있을 때 외부에서 호출하여 화면을 갱신합니다.
    /// </summary>
    public void Refresh(Dictionary<ItemPartType, InventoryItem> items)
    {
        upgradeItems = items;
        OnDropdownChanged(currentDropdownIndex);
    }

    private void OnDropdownChanged(int index)
    {
        currentDropdownIndex = index;

        if (upgradeItems == null)
            return;
        if (!System.Enum.IsDefined(typeof(ItemPartType), index))
            return;

        ItemPartType selectedPart = (ItemPartType)index;

        if (!upgradeItems.TryGetValue(selectedPart, out var invItem) || invItem == null)
        {
            // 해당 파트에 장착된 아이템이 없을 때
            itemNameText.text = "해당 아이템 없음";
            levelText.text = "-";
            costText.text = "-";
            resultText.text = string.Empty;
            upgradeButton.interactable = false;
            return;
        }

        // 장착 중인 아이템 정보 화면에 반영
        itemNameText.text = invItem.itemData.itemName;
        levelText.text = $"Lv. {invItem.level} / {invItem.itemData.maxLevel}";
        int cost = invItem.itemData.upgradeCost * invItem.level;
        costText.text = $"{cost} Gold";
        resultText.text = string.Empty;
        upgradeButton.interactable = (invItem.level < invItem.itemData.maxLevel);
    }

    private void OnUpgradeButtonClicked()
    {
        if (upgradeItems == null)
        {
            resultText.text = "강화할 아이템이 없습니다.";
            return;
        }
        if (!System.Enum.IsDefined(typeof(ItemPartType), currentDropdownIndex))
            return;

        ItemPartType selectedPart = (ItemPartType)currentDropdownIndex;
        if (!upgradeItems.TryGetValue(selectedPart, out var invItem) || invItem == null)
        {
            resultText.text = "해당 아이템 없음";
            return;
        }

        if (invItem.level >= invItem.itemData.maxLevel)
        {
            resultText.text = "이미 최대 레벨입니다.";
            return;
        }

        int cost = invItem.itemData.upgradeCost * invItem.level;
        if (!GoldManager.instance.SpendGold(cost))
        {
            resultText.text = "골드가 부족합니다.";
            return;
        }

        float successRate = invItem.itemData.GetSuccessRate(invItem.level);
        float roll = Random.value;
        if (roll < successRate)
        {
            invItem.level++;
            resultText.text = $"강화 성공! Lv. {invItem.level}";
        }
        else
        {
            resultText.text = "강화 실패...";
        }

        // 강화 후 화면 갱신
        OnDropdownChanged(currentDropdownIndex);
        EquipmentManager.instance.ApplyEquipmentEffects();
    }
}
