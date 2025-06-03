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

        // 1) ��(��ȭ ����) ǥ�� �κ�: ���� ���� ����
        foreach (Transform child in starContainer)
            Destroy(child.gameObject);

        // invItem.level�� ��Ÿ�� ��ȭ ����
        int currentLevel = invItem.level;
        for (int i = 0; i < currentLevel; i++)
            Instantiate(starPrefab, starContainer);

        // 2) ���(Stat) ����Ʈ �ʱ�ȭ
        foreach (Transform child in statContainer)
            Destroy(child.gameObject);

        ItemData data = invItem.itemData;
        int lvl = invItem.level;

        // 3) ���⺻ ���ȡ��� ����ȭ ���ʽ��� ���� ���
        //   ItemData���� bonusStats[](������ ���ʽ�)�� ����Ǿ� �ִٰ� ����.
        //   ����: bonusStats �迭 �ȿ� (StatType.ATK, 5), (StatType.HP, 10) ���� ��� ����.

        // ���� ���� (���ϴ� ��� StatType ������ �ٲټŵ� �˴ϴ�)
        StatType[] order = {
            StatType.ATK,
            StatType.STR,
            StatType.DEX,
            StatType.CRIT,
            StatType.HP,
            StatType.MP
        };

        // �� StatType���� ���� 1 ���� ���⺻������ ����ȭ������ ���
        // bonusStats �迭 �ȿ� ������ 1�� ���������� ����Ǿ� �ִٰ� ����
        // ��: bonusStats�� (StatType.HP, 10)�� ������
        //   �⺻ HP = 10 * 1
        //   ��ȭ ���ʽ� = 10 * (lvl - 1)

        // ���� �⺻ ���� ��� ���� 
        var baseHeader = Instantiate(statTextPrefab, statContainer);
        baseHeader.text = "<b><color=#D3D3D3>���� �⺻ ���� (Lv 1) ����</color></b>";

        // ���� ��ȭ ���ʽ� ��� ����
        var enhHeader = Instantiate(statTextPrefab, statContainer);
        enhHeader.text = "<b><color=#FFD700>���� ��ȭ ���ʽ� (Lv " + lvl + ") ����</color></b>";

        // bonusStats ��ųʸ� ���·� ��ȯ (StatType �� ������ ��ġ)
        var perLevelDict = new Dictionary<StatType, int>();
        foreach (var mod in data.bonusStats)
        {
            perLevelDict[mod.stat] = mod.amount;
        }

        // ������� StatType ���
        foreach (var stat in order)
        {
            // �ش� StatType�� bonusStats�� ���ٸ� ��ŵ
            if (!perLevelDict.ContainsKey(stat))
                continue;

            int perLevelValue = perLevelDict[stat];
            // 1) ���� 1 ���� �⺻��
            int baseValue = perLevelValue * 1;
            // 2) ���� ���� ���� ��ȭ ���ʽ� = perLevelValue * (lvl - 1)
            int enhValue = perLevelValue * (lvl - 1);

            // �⺻ ���� ���� ����
            var baseLine = Instantiate(statTextPrefab, statContainer);
            baseLine.text = $"{stat}: {baseValue}";

            // ������ 1�̶�� ��ȭ ���ʽ��� 0�̹Ƿ�, ��+0���� ǥ���ϰ� ���� �ʴٸ� ���� �������� ���μ���.
            if (lvl > 1)
            {
                // ��ȭ ���ʽ� ���� ����
                var enhLine = Instantiate(statTextPrefab, statContainer);
                enhLine.text = $"{stat}  +{enhValue}";
                enhLine.color = new Color32(255, 215, 0, 255); // �����(FFD700)
            }
        }

        // 4) ������ �̸� �� ���� ���
        //    (���� �������� ���Ͻø� ���⿡ ������ �߰�)
        tooltipText.text =
            $"<size=18><b>{data.itemName}</b></size>\n" +
            $"{data.description}";

        // 5) ������ ���콺 �Ǵ� ������ ��ǥ �������� �ణ �������ؼ� ��ġ
        tooltipPanel.transform.position = pos + new Vector2(30, -30);
    }
    public void Hide()
    {
        if (tooltipPanel == null)
        {
            Debug.LogWarning("TooltipUI.Hide: tooltipPanel�� �Ҵ���� �ʾҽ��ϴ�.");
            return;
        }
         if (tooltipPanel.activeSelf)
            tooltipPanel.SetActive(false);
    }
}

