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
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // 인스펙터에 playerStat가 할당되지 않았다면 PlayerStat.instance 가져오기
        if (playerStat == null)
        {
            playerStat = PlayerStat.instance;
        }

        // playerStat이 유효하면 Stat 변경 이벤트를 통해 UI를 갱신하도록 구독
        if (playerStat != null)
        {
            playerStat.OnStatChanged += UpdateUI;
        }

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
