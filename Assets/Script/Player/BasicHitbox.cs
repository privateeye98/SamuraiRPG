using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BasicHitbox : MonoBehaviour
{
    public int damage = 5;
    public float duration = 0.2f;

    void Start()
    {
        Destroy(gameObject, duration);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && other.TryGetComponent<IDamageable>(out var target))
        {
            Vector2 dir = (other.transform.position - transform.position).normalized;
            target.TakeDamage(damage, transform.position, dir);
        }
    }
}
