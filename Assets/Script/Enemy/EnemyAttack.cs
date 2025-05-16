using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    [Header("공격력")]
    public int attackDamage = 1;

    [Header("공격 쿨타임")]
    public float attackCooldown = 1f;

    private float lastAttackTime = -999f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // 쿨타임 체크
        if (Time.time < lastAttackTime + attackCooldown) return;

        if (other.TryGetComponent<IDamageable>(out var target))
        {
            Vector2 contact = other.ClosestPoint(transform.position);
            Vector2 dir = (other.transform.position - transform.position).normalized;

            target.TakeDamage(attackDamage, contact, dir);
            lastAttackTime = Time.time;
        }
    }
}
