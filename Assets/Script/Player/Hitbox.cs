using UnityEngine;


public class Hitbox : MonoBehaviour
{
    [SerializeField] float damage = 1f;
    [SerializeField] float lifeTime = 0.05f;
    Collider2D col;

    void Awake() => col = GetComponent<Collider2D>();
    void OnEnable() => Invoke(nameof(Disable), lifeTime);
    void Disable() => gameObject.SetActive(false);

    void OnTriggerEnter2D(Collider2D other)
    {

        if(other.TryGetComponent<IDamageable>(out var target))
        {
            Debug.Log("Hit " + other.name);
            Vector2 contact = other.ClosestPoint(transform.position);
            Vector2 dir = (other.transform.position - transform.position).normalized;

            target.TakeDamage((int)damage, contact, dir);
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (TryGetComponent(out Collider2D c))
            Gizmos.DrawWireCube(c.bounds.center, c.bounds.size);
    }
}
