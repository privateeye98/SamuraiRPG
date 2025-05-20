using UnityEngine;
using TMPro;

public class StatUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI strText;
    [SerializeField] TextMeshProUGUI dexText;
    [SerializeField] TextMeshProUGUI critText;

    [SerializeField] PlayerStat playerStat;
    [SerializeField] UpgradePanelToggle upgradePanelToggle;

    void Awake()
    {
        if (playerStat == null)
            playerStat = PlayerStat.instance;
    }
    public void UpdateUI()
    {
        if (playerStat == null) return;

        levelText.text = "LV : " + PlayerLevel.instance.currentLevel;
        strText.text = "STR : " + playerStat.strength;
        dexText.text = "DEX : " + playerStat.dexterity;
        critText.text = "CRIT : " + playerStat.critical;
    }

    void OnEnable()
    {
        // 🔒 playerStat 연결 보장
        if (playerStat == null)
            playerStat = PlayerStat.instance;

        // 🔒 장비 정보 가져오기
        var toggle = FindObjectOfType<UpgradePanelToggle>();
        if (toggle != null)
        {
            var items = toggle.GetUpgradeItems();
            if (items != null && items.Count > 0)
            {
                PlayerStat.instance?.ApplyEquipmentStats(items);
            }
        }

        // 🔁 UI 갱신
        UpdateUI();
    }
}
