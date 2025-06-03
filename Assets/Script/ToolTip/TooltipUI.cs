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
        tooltipPanel.SetActive(true);


        foreach (Transform child in starContainer)
            Destroy(child.gameObject);

        for (int i = 0; i < invItem.itemData.level; i++)
            Instantiate(starPrefab, starContainer);


        foreach (Transform child in statContainer)
            Destroy(child.gameObject);

        invItem.level = invItem.itemData.level;
        var stats = invItem.GetEnhancedStats();


        StatType[] order = {
            StatType.ATK,
            StatType.STR,
            StatType.DEX,
            StatType.CRIT,
            StatType.HP,
            StatType.MP
        };

        foreach (var stat in order)
        {
            if (!stats.ContainsKey(stat)) continue;
            var line = Instantiate(statTextPrefab, statContainer);
            line.text = $"{stat}: {stats[stat]}";
        }


        tooltipText.text =
            $"{invItem.itemData.itemName}\n{invItem.itemData.description}";


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

