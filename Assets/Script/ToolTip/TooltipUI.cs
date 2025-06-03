using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI instance;
    public GameObject tooltipGameObject;

    [Header("UI References")]
    public GameObject tooltipPanel;
    public TextMeshProUGUI tooltipText;

    [Header("Star (Enhancement Level)")]
    [SerializeField] private Transform starContainer;
    [SerializeField] private GameObject starPrefab;

    [Header("Stat Bonuses")]
    [SerializeField] private Transform statContainer;
    [SerializeField] private TextMeshProUGUI statTextPrefab;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public void Show(InventoryItem invItem, Vector2 pos)
    {
        if (invItem == null || invItem.itemData == null)
        {
            Hide();
            return;
        }

        tooltipPanel.SetActive(true);

        // 1) 별(강화 레벨) 표시 부분: 기존 로직 재사용
        foreach (Transform child in starContainer)
            Destroy(child.gameObject);

        // invItem.level은 런타임 강화 레벨
        int currentLevel = invItem.level;
        for (int i = 0; i < currentLevel; i++)
            Instantiate(starPrefab, starContainer);

        // 2) 통계(Stat) 리스트 초기화
        foreach (Transform child in statContainer)
            Destroy(child.gameObject);

        ItemData data = invItem.itemData;
        int lvl = invItem.level;

        // 3) “기본 스탯”과 “강화 보너스” 구분 출력
        //   ItemData에는 bonusStats[](레벨당 보너스)만 저장되어 있다고 가정.
        //   예시: bonusStats 배열 안에 (StatType.ATK, 5), (StatType.HP, 10) 등이 들어 있음.

        // 순서 지정 (원하는 대로 StatType 순서를 바꾸셔도 됩니다)
        StatType[] order = {
            StatType.ATK,
            StatType.STR,
            StatType.DEX,
            StatType.CRIT,
            StatType.HP,
            StatType.MP
        };

        // 각 StatType별로 레벨 1 기준 ‘기본값’과 ‘강화값’을 계산
        // bonusStats 배열 안에 “레벨 1당 증가량”이 저장되어 있다고 가정
        // 예: bonusStats에 (StatType.HP, 10)이 있으면
        //   기본 HP = 10 * 1
        //   강화 보너스 = 10 * (lvl - 1)

        // ── 기본 스탯 출력 ── 
        var baseHeader = Instantiate(statTextPrefab, statContainer);
        baseHeader.text = "<b><color=#D3D3D3>── 기본 스탯 (Lv 1) ──</color></b>";

        // ── 강화 보너스 출력 ──
        var enhHeader = Instantiate(statTextPrefab, statContainer);
        enhHeader.text = "<b><color=#FFD700>── 강화 보너스 (Lv " + lvl + ") ──</color></b>";

        // bonusStats 딕셔너리 형태로 변환 (StatType → 레벨당 수치)
        var perLevelDict = new Dictionary<StatType, int>();
        foreach (var mod in data.bonusStats)
        {
            perLevelDict[mod.stat] = mod.amount;
        }

        // 순서대로 StatType 출력
        foreach (var stat in order)
        {
            // 해당 StatType이 bonusStats에 없다면 스킵
            if (!perLevelDict.ContainsKey(stat))
                continue;

            int perLevelValue = perLevelDict[stat];
            // 1) 레벨 1 기준 기본값
            int baseValue = perLevelValue * 1;
            // 2) 현재 레벨 기준 강화 보너스 = perLevelValue * (lvl - 1)
            int enhValue = perLevelValue * (lvl - 1);

            // 기본 스탯 라인 생성
            var baseLine = Instantiate(statTextPrefab, statContainer);
            baseLine.text = $"{stat}: {baseValue}";

            // 레벨이 1이라면 강화 보너스가 0이므로, “+0”을 표시하고 싶지 않다면 다음 조건으로 감싸세요.
            if (lvl > 1)
            {
                // 강화 보너스 라인 생성
                var enhLine = Instantiate(statTextPrefab, statContainer);
                enhLine.text = $"{stat}  +{enhValue}";
                enhLine.color = new Color32(255, 215, 0, 255); // 노란색(FFD700)
            }
        }

        // 4) 아이템 이름 및 설명 출력
        //    (별도 디테일을 원하시면 여기에 포맷을 추가)
        tooltipText.text =
            $"<size=18><b>{data.itemName}</b></size>\n" +
            $"{data.description}";

        // 5) 툴팁을 마우스 또는 지정한 좌표 기준으로 약간 오프셋해서 배치
        tooltipPanel.transform.position = pos + new Vector2(30, -30);
    }
    public void Hide()
    {
        if (tooltipPanel == null)
        {
            Debug.LogWarning("TooltipUI.Hide: tooltipPanel이 할당되지 않았습니다.");
            return;
        }
         if (tooltipPanel.activeSelf)
            tooltipPanel.SetActive(false);
    }
}

