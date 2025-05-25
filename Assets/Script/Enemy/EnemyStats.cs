using UnityEngine;

[CreateAssetMenu(menuName = "Game/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    public int baseHP;
    public int baseMinAtk;
    public int baseMaxAtk;
    public int baseDefense;
    public int baseEvade;

    [Tooltip("������ ������")]
    public float hpPerLevel;
    public float atkPerLevel;
    public float defPerLevel;
    public float evadePerLevel;

    [Tooltip("���� �����ϸ� �(�ɼ�)")]
    public AnimationCurve difficultyCurve;
}
