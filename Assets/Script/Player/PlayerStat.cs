using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public int maxHP = 100;
    public int maxMP = 50;
    public int strength = 10;
    public int dexterity = 5;
    public int critical = 1;

    public static PlayerStat instance;


     void Awake()
    {
        instance = this;
    }
    public void LevelUpBonus(int level)
    {
        maxHP += 10;
        maxMP += 5;
        strength += 2;
        dexterity += 1;

        if (level % 3 == 0) // 예: 3레벨마다 치명타 증가
            critical += 1;

        Debug.Log($"[레벨업 보너스] HP:{maxHP}, STR:{strength}, DEX:{dexterity}, CRIT:{critical}");
    }
    public int GetAttackDamage()
    {
        int baseDamage = 5; // 기본값
        float strengthBonus = strength * 2f;
        float dexBonus = dexterity * 1.2f; // DEX도 영향 줌
        return Mathf.RoundToInt(baseDamage + strengthBonus + dexBonus);
    }
    public bool IsCriticalHit()
    {
        return Random.Range(0f, 100f) < critical;
    }

    public float GetCriticalMultiplier()
    {
        return 1.5f; // 또는 2.0f
    }
}
