using UnityEngine;
using TMPro;

public class StatUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI atkText;
    [SerializeField] TextMeshProUGUI strText;
    [SerializeField] TextMeshProUGUI dexText;
    [SerializeField] TextMeshProUGUI critText;
    [SerializeField] TextMeshProUGUI expText;

    [SerializeField] PlayerStat playerStat;
    public static StatUI instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        instance = this;
        if (playerStat == null)
            playerStat = PlayerStat.instance;

    }

    void OnEnable()
    {
        if (playerStat != null)
            playerStat.OnStatChanged += UpdateUI;

        UpdateUI(); 
    }

    void OnDisable()
    {
        if (playerStat != null)
            playerStat.OnStatChanged -= UpdateUI;
    }

    public void UpdateUI()
    {
        if (playerStat == null || PlayerLevel.instance == null) return;

        levelText.text = "LV : " + PlayerLevel.instance.currentLevel;
        atkText.text = $"ATK : {playerStat.MinDamage} ~ {playerStat.MaxDamage}";
        strText.text = "STR : " + playerStat.strength;
        dexText.text = "DEX : " + playerStat.dexterity;
        critText.text = "CRIT : " + playerStat.critical;
        expText.text = $"EXP : {PlayerLevel.instance.currentExp} / {PlayerLevel.instance.GetRequiredExp(PlayerLevel.instance.currentLevel)}";
    }
}
