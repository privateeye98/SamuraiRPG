using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("��ð��� ����")]
    public EnemyNightBuff NightBuff;
    bool nightbuffapplied = false;


    [Header("���ݷ�")]
    public int attackDamage = 1;

    [Header("���� ��Ÿ��")]
    public float attackCooldown = 1f;

    private float lastAttackTime = -999f;

     void Update()
    {
        if(!nightbuffapplied && DayNightCycle.currentTime > NightBuff.nightStartTime)
        {
            attackDamage = Mathf.RoundToInt(attackDamage*NightBuff.attackMultiplier);
            nightbuffapplied = true;

            Debug.Log($"{gameObject.name} ���ݷ� ��ȭ Enemy TK:{attackDamage}");

        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // ��Ÿ�� üũ
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
