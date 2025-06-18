using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI instance;
    public GameObject tooltipGameObject;

    [Header("UI References")]
    public GameObject tooltipPanel;            // 툴팁 전체 패널
    public TextMeshProUGUI nametext;      // 아이템 이름 + 설명
    public TextMeshProUGUI desciptionText; // 아이템 설명 (추가 설명용, 필요시 사용)
    public TextMeshProUGUI requirementText; // 아이템 요구 사항 (레벨, 스탯 등)
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

        // 시작 시 툴팁 비활성화
        if (tooltipPanel != null)
            tooltipPanel.SetActive(false);
    }

    /// <summary>
    /// invItem이 null이거나 itemData가 null이면 꽁꽁 숨기고, 
    /// Equipment 타입이면 “기본 스탯(Base Stats) / 강화 보너스(Enhancement Bonus)”를 분리 출력합니다.
    /// </summary>
    public void Show(InventoryItem invItem, Vector2 pos)
    {
        if (invItem == null || invItem.itemData == null)
        {
            Hide();
            return;
        }

        tooltipPanel.SetActive(true);

        // 1) 별(★)로 강화 레벨 표시
        /* foreach (Transform child in starContainer)
            Destroy(child.gameObject);
        int currentLevel = invItem.level;
        for (int i = 0; i < currentLevel; i++)
            Instantiate(starPrefab, starContainer);
        */
        // 2) statContainer 초기화
        foreach (Transform child in statContainer)
            Destroy(child.gameObject);

        ItemData data = invItem.itemData;
        int lvl = invItem.level;

        string levelText = invItem.level > 0 ? $" <color=#FFD700>(+{invItem.level})</color>" : "";
        nametext.text = $"<size=38><b>{data.itemName}{levelText}</b></size>";
        desciptionText.text = $"<size=20><color=#FFFFFF>{data.description}</color></size>";
        requirementText.text = GetRequirementText(invItem);
        // 4) 타입별 분기
        if (data.type == ItemType.Equipment)
        {
            ShowEquipment(invItem);
        }
        else if (data.type == ItemType.Consumable)
        {
            ShowConsumable(invItem);
        }
        // (Quest 등 다른 타입일 때는 이름+설명만 보여 주고 statContainer는 비워 둔다)

        // 5) 툴팁 패널 위치 조정 (마우스 커서 기준 오프셋)
        tooltipPanel.transform.position = pos + new Vector2(30, -30);
    }

    private void ShowConsumable(InventoryItem invItem)
    {
        // 소모품(Consumable)일 경우 healAmount, ManaAmount만 출력
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
    /// 장비 타입일 때: “기본 스탯(Base Stats)”과 “강화 보너스(Enhancement Bonus)”를 분리하여 출력
    /// </summary>
    private void ShowEquipment(InventoryItem invItem)
    {
        ItemData data = invItem.itemData;
        int lvl = invItem.level;

        // ── 기본 스탯(Base Stats) 헤더 ──
        var baseHeader = Instantiate(statTextPrefab, statContainer);
        baseHeader.text = "<b><color=#FFFFFF>── 기본 스탯 (착용 시 고정) ──</color></b>";

        // (A) 기본 스탯 출력
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

        // ── 강화 보너스(Enhancement Bonus) 헤더 ──
        if (lvl > 1)
        {
            var enhHeader = Instantiate(statTextPrefab, statContainer);
            enhHeader.text = $"<b><color=#FFD700>── 강화 보너스 (Lv {lvl}) ──</color></b>";

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
            Debug.LogWarning("TooltipUI.Hide: tooltipPanel이 할당되지 않았습니다.");
            return;
        }

        if (tooltipPanel.activeSelf)
        {
            tooltipPanel.SetActive(false);

            // 화면에서 보이던 모든 스탯 라인과 별(★)을 제거
            foreach (Transform child in statContainer)
                Destroy(child.gameObject);
          /*  foreach (Transform child in starContainer)
                Destroy(child.gameObject); */
        }
    }
}

