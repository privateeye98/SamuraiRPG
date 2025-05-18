using UnityEngine;
using TMPro;

public class StatUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI strText;
    [SerializeField] TextMeshProUGUI dexText;
    [SerializeField] TextMeshProUGUI critText;

    [SerializeField] PlayerStat playerStat;

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
        UpdateUI(); // 패널이 켜질 때 자동 반영
    }
}
