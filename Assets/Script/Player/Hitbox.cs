using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] float lifeTime = 0.05f;
    Collider2D col;

    void Awake() => col = GetComponent<Collider2D>();
    void OnEnable() => Invoke(nameof(Disable), lifeTime);
    void Disable() => gameObject.SetActive(false);

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IDamageable>(out var target))
        {
            Debug.Log("Hit " + other.name);
            Vector2 contact = other.ClosestPoint(transform.position);
            Vector2 dir = (other.transform.position - transform.position).normalized;

            // 데미지 계산
            bool isCrit = PlayerStat.instance.IsCriticalHit();
            int damage = PlayerStat.instance.GetAttackDamage();

            if (isCrit)
            {
                float critMulti = PlayerStat.instance.GetCriticalMultiplier();
                damage = Mathf.RoundToInt(damage * critMulti);
                Debug.Log($"크리티컬 발생! x{critMulti} → {damage} 데미지");
            }

            // 몬스터 머리 위로 띄우기
            Vector3 topOfTarget = other.bounds.center + Vector3.up * (other.bounds.extents.y + 0.5f);
            DamageTextSpawner.I.Spawn(damage, topOfTarget, isCrit);

            // 데미지 적용
            target.TakeDamage(damage, contact, dir);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (TryGetComponent(out Collider2D c))
            Gizmos.DrawWireCube(c.bounds.center, c.bounds.size);
    }
}
