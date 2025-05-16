using UnityEngine;

public class SkillHitbox : MonoBehaviour
{
    public int damage = 5;
    public float duration = 0.3f;

    void Start()
    {
        Destroy(gameObject, duration);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent<IDamageable>(out var target))
            {
                Vector2 dir = other.transform.position - transform.position;
                target.TakeDamage(damage, transform.position, dir.normalized);
            }
        }
    }
}