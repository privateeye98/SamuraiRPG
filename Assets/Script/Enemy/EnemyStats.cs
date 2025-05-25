using UnityEngine;

[CreateAssetMenu(menuName = "Game/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    public int baseHP;
    public int baseMinAtk;
    public int baseMaxAtk;
    public int baseDefense;
    public int baseEvade;

    [Tooltip("레벨당 증가량")]
    public float hpPerLevel;
    public float atkPerLevel;
    public float defPerLevel;
    public float evadePerLevel;

    [Tooltip("레벨 스케일링 곡선(옵션)")]
    public AnimationCurve difficultyCurve;
}
