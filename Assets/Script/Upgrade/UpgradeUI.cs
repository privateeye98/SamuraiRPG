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
        // ��Ӵٿ� ���� �ٲ� �� �ݹ� ����
        partDropdown.onValueChanged.RemoveAllListeners();
        partDropdown.onValueChanged.AddListener(OnDropdownChanged);

        // ��ȭ ��ư Ŭ�� �� �ݹ� ����
        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
    }

    void OnDisable()
    {
        partDropdown.onValueChanged.RemoveAllListeners();
        upgradeButton.onClick.RemoveAllListeners();
    }

    /// <summary>
    /// ��ȭâ�� ó�� �� �� ȣ���մϴ�.
    /// </summary>
    public void Open(Dictionary<ItemPartType, InventoryItem> items)
    {
        upgradeItems = items;
        gameObject.SetActive(true);

        // ��Ӵٿ� �ʱ�ȭ
        partDropdown.ClearOptions();
        var options = System.Enum.GetNames(typeof(ItemPartType)).ToList();
        partDropdown.AddOptions(options);

        // ��Ӵٿ� ù �׸� ����(�ε��� 0)
        partDropdown.value = 0;
        partDropdown.RefreshShownValue();

        OnDropdownChanged(0);
    }

    /// <summary>
    /// ��� �ٲ� ���, ��ȭâ�� ���� ���� �� �ܺο��� ȣ���Ͽ� ȭ���� �����մϴ�.
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
            // �ش� ��Ʈ�� ������ �������� ���� ��
            itemNameText.text = "�ش� ������ ����";
            levelText.text = "-";
            costText.text = "-";
            resultText.text = string.Empty;
            upgradeButton.interactable = false;
            return;
        }

        // ���� ���� ������ ���� ȭ�鿡 �ݿ�
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
            resultText.text = "��ȭ�� �������� �����ϴ�.";
            return;
        }
        if (!System.Enum.IsDefined(typeof(ItemPartType), currentDropdownIndex))
            return;

        ItemPartType selectedPart = (ItemPartType)currentDropdownIndex;
        if (!upgradeItems.TryGetValue(selectedPart, out var invItem) || invItem == null)
        {
            resultText.text = "�ش� ������ ����";
            return;
        }

        if (invItem.level >= invItem.itemData.maxLevel)
        {
            resultText.text = "�̹� �ִ� �����Դϴ�.";
            return;
        }

        int cost = invItem.itemData.upgradeCost * invItem.level;
        if (!GoldManager.instance.SpendGold(cost))
        {
            resultText.text = "��尡 �����մϴ�.";
            return;
        }

        float successRate = invItem.itemData.GetSuccessRate(invItem.level);
        float roll = Random.value;
        if (roll < successRate)
        {
            invItem.level++;
            resultText.text = $"��ȭ ����! Lv. {invItem.level}";
        }
        else
        {
            resultText.text = "��ȭ ����...";
        }

        // ��ȭ �� ȭ�� ����
        OnDropdownChanged(currentDropdownIndex);
        EquipmentManager.instance.ApplyEquipmentEffects();
    }
}
