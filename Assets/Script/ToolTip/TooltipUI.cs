using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI instance;
    public GameObject tooltipGameObject;

    [Header("UI References")]
    public GameObject tooltipPanel;            // ���� ��ü �г�
    public TextMeshProUGUI nametext;      // ������ �̸� + ����
    public TextMeshProUGUI desciptionText; // ������ ���� (�߰� �����, �ʿ�� ���)
    public TextMeshProUGUI requirementText; // ������ �䱸 ���� (����, ���� ��)
    //[Header("Star (Enhancement Level)")]
    //[SerializeField] private Transform starContainer;
    //[SerializeField] private GameObject starPrefab;

    [Header("Stat Bonuses")]
    [SerializeField] private Transform statContainer;
    [SerializeField] private TextMeshProUGUI statTextPrefab;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        // ���� �� ���� ��Ȱ��ȭ
        if (tooltipPanel != null)
            tooltipPanel.SetActive(false);
    }

    /// <summary>
    /// invItem�� null�̰ų� itemData�� null�̸� �ǲ� �����, 
    /// Equipment Ÿ���̸� ���⺻ ����(Base Stats) / ��ȭ ���ʽ�(Enhancement Bonus)���� �и� ����մϴ�.
    /// </summary>
    public void Show(InventoryItem invItem, Vector2 pos)
    {
        if (invItem == null || invItem.itemData == null)
        {
            Hide();
            return;
        }

        tooltipPanel.SetActive(true);

        // 1) ��(��)�� ��ȭ ���� ǥ��
        /* foreach (Transform child in starContainer)
            Destroy(child.gameObject);
        int currentLevel = invItem.level;
        for (int i = 0; i < currentLevel; i++)
            Instantiate(starPrefab, starContainer);
        */
        // 2) statContainer �ʱ�ȭ
        foreach (Transform child in statContainer)
            Destroy(child.gameObject);

        ItemData data = invItem.itemData;
        int lvl = invItem.level;

        string levelText = invItem.level > 0 ? $" <color=#FFD700>(+{invItem.level})</color>" : "";
        nametext.text = $"<size=38><b>{data.itemName}{levelText}</b></size>";
        desciptionText.text = $"<size=20><color=#FFFFFF>{data.description}</color></size>";
        requirementText.text = GetRequirementText(invItem);
        // 4) Ÿ�Ժ� �б�
        if (data.type == ItemType.Equipment)
        {
            ShowEquipment(invItem);
        }
        else if (data.type == ItemType.Consumable)
        {
            ShowConsumable(invItem);
        }
        // (Quest �� �ٸ� Ÿ���� ���� �̸�+���� ���� �ְ� statContainer�� ��� �д�)

        // 5) ���� �г� ��ġ ���� (���콺 Ŀ�� ���� ������)
        tooltipPanel.transform.position = pos + new Vector2(30, -30);
    }

    private void ShowConsumable(InventoryItem invItem)
    {
        // �Ҹ�ǰ(Consumable)�� ��� healAmount, ManaAmount�� ���
        ItemData data = invItem.itemData;
        if (data.healAmount > 0)
        {
            var line = Instantiate(statTextPrefab, statContainer);
            line.text = $"Heal: {data.healAmount}";
        }
        if (data.ManaAmount > 0)
        {
            var line = Instantiate(statTextPrefab, statContainer);
            line.text = $"Mana: {data.ManaAmount}";
        }
    }

    /// <summary>
    /// ��� Ÿ���� ��: ���⺻ ����(Base Stats)���� ����ȭ ���ʽ�(Enhancement Bonus)���� �и��Ͽ� ���
    /// </summary>
    private void ShowEquipment(InventoryItem invItem)
    {
        ItemData data = invItem.itemData;
        int lvl = invItem.level;

        // ���� �⺻ ����(Base Stats) ��� ����
        var baseHeader = Instantiate(statTextPrefab, statContainer);
        baseHeader.text = "<b><color=#FFFFFF>���� �⺻ ���� (���� �� ����) ����</color></b>";

        // (A) �⺻ ���� ���
        if (data.baseATK != 0)
        {
            var line = Instantiate(statTextPrefab, statContainer);
            line.text = $"ATK: {data.baseATK}";
        }
        if (data.baseDEF != 0)
        {
            var line = Instantiate(statTextPrefab, statContainer);
            line.text = $"DEF: {data.baseDEF}";
        }
        if (data.baseHP != 0)
        {
            var line = Instantiate(statTextPrefab, statContainer);
            line.text = $"HP: {data.baseHP}";
        }
        if (data.baseMP != 0)
        {
            var line = Instantiate(statTextPrefab, statContainer);
            line.text = $"MP: {data.baseMP}";
        }
        if (data.baseSTR != 0)
        {
            var line = Instantiate(statTextPrefab, statContainer);
            line.text = $"STR: {data.baseSTR}";
        }
        if (data.baseDEX != 0)
        {
            var line = Instantiate(statTextPrefab, statContainer);
            line.text = $"DEX: {data.baseDEX}";
        }
        if (data.baseCRIT != 0)
        {
            var line = Instantiate(statTextPrefab, statContainer);
            line.text = $"CRIT: {data.baseCRIT}%";
        }

        // ���� ��ȭ ���ʽ�(Enhancement Bonus) ��� ����
        if (lvl > 1)
        {
            var enhHeader = Instantiate(statTextPrefab, statContainer);
            enhHeader.text = $"<b><color=#FFD700>���� ��ȭ ���ʽ� (Lv {lvl}) ����</color></b>";

            int mult = lvl - 1;

            if (data.perLevelATK != 0)
            {
                var line = Instantiate(statTextPrefab, statContainer);
                line.text = $"ATK: +{data.perLevelATK * mult}";
                line.color = new Color32(255, 215, 0, 255);
            }
            if (data.perLevelDEF != 0)
            {
                var line = Instantiate(statTextPrefab, statContainer);
                line.text = $"DEF: +{data.perLevelDEF * mult}";
                line.color = new Color32(255, 215, 0, 255);
            }
            if (data.perLevelHP != 0)
            {
                var line = Instantiate(statTextPrefab, statContainer);
                line.text = $"HP: +{data.perLevelHP * mult}";
                line.color = new Color32(255, 215, 0, 255);
            }
            if (data.perLevelMP != 0)
            {
                var line = Instantiate(statTextPrefab, statContainer);
                line.text = $"MP: +{data.perLevelMP * mult}";
                line.color = new Color32(255, 215, 0, 255);
            }
            if (data.perLevelSTR != 0)
            {
                var line = Instantiate(statTextPrefab, statContainer);
                line.text = $"STR: +{data.perLevelSTR * mult}";
                line.color = new Color32(255, 215, 0, 255);
            }
            if (data.perLevelDEX != 0)
            {
                var line = Instantiate(statTextPrefab, statContainer);
                line.text = $"DEX: +{data.perLevelDEX * mult}";
                line.color = new Color32(255, 215, 0, 255);
            }
            if (data.perLevelCRIT != 0)
            {
                var line = Instantiate(statTextPrefab, statContainer);
                line.text = $"CRIT: +{data.perLevelCRIT * mult}%";
                line.color = new Color32(255, 215, 0, 255);
            }
        }
    }


        private string GetRequirementText(InventoryItem item)
    {
        var data = item.itemData;
        Dictionary<StatType, int> required = new Dictionary<StatType, int>();

        // 1. level
        string lvColor = PlayerLevel.instance.currentLevel >= data.requiredLevel ? "#FFFFFF" : "#FF5555";
        string levelText = $"<color={lvColor}>LV: {data.requiredLevel}</color>";

        foreach (StatType stat in System.Enum.GetValues(typeof(StatType)))
            required[stat] = 0;

        foreach (var req in data.requiredStats)
            required[req.stat] = req.value;

        List<string> parts = new List<string> { levelText };

        foreach (var kv in required)
        {
            StatType stat = kv.Key;
            int value = kv.Value;
            int current = PlayerStat.instance.GetStat(stat);

            string color;
            if (value == 0)
                color = "#FFFFFF";
            else if (current >= value)
                color = "#888888";
            else
                color = "#FF5555";

            parts.Add($"<color={color}>{stat}: {value}</color>");
        }

        return string.Join("   ", parts);
    }



public void Hide()
    {
        if (tooltipPanel == null)
        {
            Debug.LogWarning("TooltipUI.Hide: tooltipPanel�� �Ҵ���� �ʾҽ��ϴ�.");
            return;
        }

        if (tooltipPanel.activeSelf)
        {
            tooltipPanel.SetActive(false);

            // ȭ�鿡�� ���̴� ��� ���� ���ΰ� ��(��)�� ����
            foreach (Transform child in statContainer)
                Destroy(child.gameObject);
          /*  foreach (Transform child in starContainer)
                Destroy(child.gameObject); */
        }
    }
}

