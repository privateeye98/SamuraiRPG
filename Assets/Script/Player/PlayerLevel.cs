using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PlayerLevel : MonoBehaviour
{
    public static PlayerLevel instance;

    [Header("레벨 설정")]
    public int currentLevel = 1;
    public int currentExp = 0;
    public int maxLevel = 30;

    [Header("경험치 수식 설정")]
    public int baseExp = 100;         // 1레벨 기준 필요 경험치
    public float expGrowthFactor = 1.2f; // 성장 계수 (1.2배씩 증가)

    [Header("UI")]
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI expText;
    public Slider expSlider;

    void Awake()
    {
        instance = this;
        UpdateUI();
    }

    public void AddExp(int amount)
    {
        if (currentLevel >= maxLevel)
        {
            Debug.Log("🧱 최대 레벨 도달");
            return;
        }

        currentExp += amount;
        Debug.Log($"+{amount} EXP 획득! (현재: {currentExp})");

        while (currentLevel < maxLevel && currentExp >= GetRequiredExp(currentLevel))
        {
            currentExp -= GetRequiredExp(currentLevel);
            currentLevel++;
            Debug.Log($"🎉 레벨업! → Lv.{currentLevel}");
        }

        UpdateUI();
    }

    public int GetRequiredExp(int level)
    {
        return Mathf.RoundToInt(baseExp * Mathf.Pow(expGrowthFactor, level - 1));
    }

    void UpdateUI()
    {
        int required = GetRequiredExp(currentLevel);

        if (levelText)
            levelText.text = $"Lv. {currentLevel}";

        if (expText)
            expText.text = $"{currentExp} / {required}";

        if (expSlider)
        {
            expSlider.maxValue = required;
            expSlider.value = currentExp;
        }
    }
}
