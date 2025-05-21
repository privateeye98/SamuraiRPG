using UnityEngine;

[System.Serializable]
public class EnemyNightBuff
{
    [Tooltip("체력 배수")]
    public float healthMultiplier = 1.5f;

    [Tooltip("공격력 배수")]
    public float attackMultiplier = 1.5f;

    [Tooltip("밤이 시작되는 시각 (초)")]
    public float nightStartTime = 40f;
}
